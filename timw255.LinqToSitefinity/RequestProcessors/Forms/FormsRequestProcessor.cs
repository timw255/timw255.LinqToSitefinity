using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.Model;
using timw255.Sitefinity.RestClient.ServiceWrappers.Content;
using timw255.Sitefinity.RestClient.ServiceWrappers.Forms;

namespace timw255.LinqToSitefinity.RequestProcessors.Forms
{
    public class FormsRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            FormDescriptionViewModel[] items = null;

            FormsServiceWrapper service = new FormsServiceWrapper(sf);

            items = service.GetFormDescriptions(queryParameters.Filter, "", queryParameters.Skip, queryParameters.Take, false).Items.ToArray();

            return items.OfType<T>().ToList<T>();
        }
    }
}
