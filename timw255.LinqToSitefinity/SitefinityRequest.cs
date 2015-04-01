using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.LinqToSitefinity
{
    public class SitefinityRequest
    {
        public string Filter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public SitefinityRequest()
        {
            this.Filter = "";
            this.Skip = 0;
            this.Take = int.MaxValue;
        }
    }
}
