using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Blogs.Model;
using Telerik.Sitefinity.Modules.Blogs.Web.Services;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog.Web.Services;
using Telerik.Sitefinity.Modules.Events.Web.Services;
using Telerik.Sitefinity.Modules.Forms.Web.Services.Model;
using Telerik.Sitefinity.Modules.Libraries.Documents;
using Telerik.Sitefinity.Modules.Libraries.Images;
using Telerik.Sitefinity.Modules.Libraries.Web.Services;
using Telerik.Sitefinity.Modules.News.Web.Services;
using Telerik.Sitefinity.Modules.Pages.Web.Services.Model;
using Telerik.Sitefinity.Taxonomies.Web.Services;
using timw255.LinqToSitefinity.Helper;
using timw255.LinqToSitefinity.RequestProcessors;
using timw255.LinqToSitefinity.RequestProcessors.Content;
using timw255.LinqToSitefinity.RequestProcessors.Ecommerce.Catalog;
using timw255.LinqToSitefinity.RequestProcessors.Forms;
using timw255.LinqToSitefinity.RequestProcessors.Pages;
using timw255.LinqToSitefinity.RequestProcessors.Taxonomies;
using timw255.Sitefinity.RestClient;

namespace timw255.LinqToSitefinity
{
    public class SitefinityContext : IDisposable
    {
        private string _userName;
        private string _password;
        private string _url;
        private SitefinityRestClient _sf;

        public SitefinityContext(string userName, string password, string url)
        {
            this._userName = userName;
            this._password = password;
            this._url = url;

            this._sf = new SitefinityRestClient(this._userName, this._password, this._url);
        }

        public SitefinityQueryable<ProductViewModel> Products
        {
            get
            {
                return new SitefinityQueryable<ProductViewModel>(this);
            }
        }

        public SitefinityQueryable<FormDescriptionViewModel> Forms
        {
            get
            {
                return new SitefinityQueryable<FormDescriptionViewModel>(this);
            }
        }

        public SitefinityQueryable<AlbumViewModel> Albums
        {
            get
            {
                return new SitefinityQueryable<AlbumViewModel>(this);
            }
        }

        public SitefinityQueryable<BlogViewModel> Blogs
        {
            get
            {
                return new SitefinityQueryable<BlogViewModel>(this);
            }
        }

        public SitefinityQueryable<BlogPostViewModel> BlogPosts
        {
            get
            {
                return new SitefinityQueryable<BlogPostViewModel>(this);
            }
        }

        public SitefinityQueryable<PageViewModel> Pages
        {
            get
            {
                return new SitefinityQueryable<PageViewModel>(this);
            }
        }

        public SitefinityQueryable<DocumentLibraryItemViewModel> Documents
        {
            get
            {
                return new SitefinityQueryable<DocumentLibraryItemViewModel>(this);
            }
        }

        public SitefinityQueryable<LibraryViewModel> DocumentLibraries
        {
            get
            {
                return new SitefinityQueryable<LibraryViewModel>(this);
            }
        }

        public SitefinityQueryable<EventViewModel> Events
        {
            get
            {
                return new SitefinityQueryable<EventViewModel>(this);
            }
        }

        public SitefinityQueryable<AlbumItemViewModel> Images
        {
            get
            {
                return new SitefinityQueryable<AlbumItemViewModel>(this);
            }
        }

        public SitefinityQueryable<NewsItemViewModel> News
        {
            get
            {
                return new SitefinityQueryable<NewsItemViewModel>(this);
            }
        }

        public SitefinityQueryable<WcfTaxonomy> Taxonomies
        {
            get
            {
                return new SitefinityQueryable<WcfTaxonomy>(this);
            }
        }

        protected internal virtual IRequestProcessor<T> CreateRequestProcessor<T>()
        where T : class
        {
            return this.CreateRequestProcessor<T>(typeof(T).Name);
        }

        internal IRequestProcessor<T> CreateRequestProcessor<T>(Expression expression)
        where T : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException("Expression", "Expression passed to CreateRequestProcessor must not be null.");
            }
            string name = (new MethodCallExpressionTypeFinder()).GetGenericType(expression).Name;
            return this.CreateRequestProcessor<T>(name);
        }

        protected internal IRequestProcessor<T> CreateRequestProcessor<T>(string requestType)
        where T : class
        {
            IRequestProcessor<T> requestProcessor;
            string str = requestType;
            string str1 = str;
            if (str != null)
            {
                switch (str1)
                {
                    case "ProductViewModel":
                    {
                        requestProcessor = new ProductsRequestProcessor<T>();
                        break;
                    }
                    case "FormDescriptionViewModel":
                    {
                        requestProcessor = new FormsRequestProcessor<T>();
                        break;
                    }
                    case "AlbumViewModel":
                    {
                        requestProcessor = new AlbumsRequestProcessor<T>();
                        break;
                    }
                    case "BlogViewModel":
                    {
                        requestProcessor = new BlogsRequestProcessor<T>();
                        break;
                    }
                    case "BlogPostViewModel":
                    {
                        requestProcessor = new BlogPostsRequestProcessor<T>();
                        break;
                    }
                    case "PageViewModel":
                    {
                        requestProcessor = new PagesRequestProcessor<T>();
                        break;
                    }
                    case "DocumentLibraryItemViewModel":
                    {
                        requestProcessor = new DocumentsRequestProcessor<T>();
                        break;
                    }
                    case "EventViewModel":
                    {
                        requestProcessor = new EventsRequestProcessor<T>();
                        break;
                    }
                    case "AlbumItemViewModel":
                    {
                        requestProcessor = new ImagesRequestProcessor<T>();
                        break;
                    }
                    case "NewsItemViewModel":
                    {
                        requestProcessor = new NewsRequestProcessor<T>();
                        break;
                    }
                    case "VideoLibraryItemViewModel":
                    {
                        requestProcessor = new VideosRequestProcessor<T>();
                        break;
                    }
                    case "LibraryViewModel":
                    {
                        requestProcessor = new DocumentLibrariesRequestProcessor<T>();
                        break;
                    }
                    case "WcfTaxonomy":
                    {
                        requestProcessor = new TaxonomiesRequestProcessor<T>();
                        break;
                    }
                    default:
                    {
                        throw new ArgumentException(string.Concat("Type, ", requestType, " isn't a supported LINQ to Sitefinity entity."), "requestType");
                    }
                }
                return requestProcessor;
            }
            throw new ArgumentException(string.Concat("Type, ", requestType, " isn't a supported LINQ to Sitefinity entity."), "requestType");
        }

        public virtual object Execute<T>(Expression expression, bool IsEnumerable)
        where T : class
        {
            IRequestProcessor<T> requestProcessor = this.CreateRequestProcessor<T>(expression);

            SitefinityExpressionVisitor visitor = new SitefinityExpressionVisitor(Evaluator.PartialEval(expression));
            SitefinityRequest query = visitor.Translate();

            List<T> items = requestProcessor.QuerySitefinity(query, this._sf);

            IQueryable<T> queryableItems = items.AsQueryable<T>();

            ExpressionTreeModifier<T> treeCopier = new ExpressionTreeModifier<T>(queryableItems);
            Expression newExpressionTree = treeCopier.CopyAndModify(expression);

            if (IsEnumerable)
            {
                return queryableItems.Provider.CreateQuery(newExpressionTree);
            }
            else
            {
                return queryableItems.Provider.Execute(newExpressionTree);
            }
        }

        public void Dispose()
        {
            IDisposable sf = this._sf as IDisposable;
            if (sf != null)
            {
                sf.Dispose();
            }
        }
    }
}
