using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Content;

namespace timw255.LinqToSitefinity.RequestProcessors
{
    public class VideosRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            VideoLibraryItemViewModel[] items = null;

            VideoServiceWrapper service = new VideoServiceWrapper(sf);

            items = service.GetContentItems("", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "", "").Items.ToArray();

            return items.OfType<T>().ToList<T>();
        }
    }
}
