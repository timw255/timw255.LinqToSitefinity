using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using timw255.Sitefinity.RestClient;

namespace timw255.LinqToSitefinity.RequestProcessors
{
    public interface IRequestProcessor<T>
    {
        List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf);
    }
}
