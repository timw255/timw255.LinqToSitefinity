using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Taxonomies.Web.Services;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.ServiceWrappers.Taxonomies;

namespace timw255.LinqToSitefinity.RequestProcessors.Taxonomies
{
    public class TaxonomiesRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            WcfTaxonomy[] wcfTaxonomies = null;

            TaxonomyServiceWrapper serviceWrapper = new TaxonomyServiceWrapper(sf);

            wcfTaxonomies = serviceWrapper.GetTaxonomies("", "", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "").Items.ToArray();

            return wcfTaxonomies.OfType<T>().ToList<T>();
        }
    }
}
