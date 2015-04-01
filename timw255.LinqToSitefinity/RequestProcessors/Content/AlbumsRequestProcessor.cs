﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Blogs.Web.Services;
using Telerik.Sitefinity.Modules.Libraries.Web.Services;
using timw255.Sitefinity.RestClient;
using timw255.Sitefinity.RestClient.ServiceWrappers.Content;

namespace timw255.LinqToSitefinity.RequestProcessors.Content
{
    public class AlbumsRequestProcessor<T> : IRequestProcessor<T>
    where T : class
    {
        public List<T> QuerySitefinity(SitefinityRequest queryParameters, SitefinityRestClient sf)
        {
            AlbumViewModel[] items = null;

            AlbumServiceWrapper service = new AlbumServiceWrapper(sf);

            items = service.GetContentItems("", queryParameters.Skip, queryParameters.Take, queryParameters.Filter, "", "").Items.ToArray();

            return items.OfType<T>().ToList<T>();
        }
    }
}