using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Blogs.Web.Services;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog.Web.Services;
using Telerik.Sitefinity.Modules.Libraries.Web.Services;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.ServiceWrappers.Content;
using timw255.Sitefinity.RestClient.ServiceWrappers.Ecommerce.Catalog;

namespace timw255.LinqToSitefinity.RequestProcessors.Ecommerce.Catalog
{
    public class ProductsRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            ProductViewModel[] items = null;

            ProductServiceWrapper service = new ProductServiceWrapper(sf);

            items = service.GetProducts("", "", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "", "").Items.ToArray();

            return items.OfType<T>().ToList<T>();
        }
    }
}
