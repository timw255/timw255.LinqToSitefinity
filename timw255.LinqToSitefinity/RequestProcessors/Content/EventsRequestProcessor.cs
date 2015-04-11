using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Content;

namespace timw255.LinqToSitefinity.RequestProcessors.Content
{
    public class EventsRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            EventViewModel[] items = null;

            EventServiceWrapper service = new EventServiceWrapper(sf);

            items = service.GetContentItems("", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "", "").Items.ToArray();

            return items.OfType<T>().ToList<T>();
        }
    }
}
