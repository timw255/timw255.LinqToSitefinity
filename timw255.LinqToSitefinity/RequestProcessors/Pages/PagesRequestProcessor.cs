using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Pages;

namespace timw255.LinqToSitefinity.RequestProcessors.Pages
{
    public class PagesRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            PageViewModel[] pageViewModels = null;

            PagesServiceWrapper pagesServiceWrapper = new PagesServiceWrapper(sf);

            pageViewModels = pagesServiceWrapper.GetPages("", false, "", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "").Items.ToArray();

            return pageViewModels.OfType<T>().ToList<T>();
        }
    }
}
