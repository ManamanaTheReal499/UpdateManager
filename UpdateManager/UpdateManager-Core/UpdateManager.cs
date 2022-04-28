using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateManager_Core.Models;

namespace UpdateManager_Core
{
    public class UpdateManager
    {
        API api;

        private string username;
        private string repository;
        private string releasesUrl
        {
            get => $"https://api.github.com/repos/{username.Trim().UrlEncode()}/{repository.Trim().UrlEncode()}/releases";
        }

        public UpdateManager(string username, string repository)
        {
            this.username = username;
            this.repository = repository;
            api = new API();
        }

        public async Task<List<Root>?> GetAllReleasesAsync() =>
            await api.Request<List<Models.Root>?>(releasesUrl);

        public async Task<Root?> GetFirtPackageAsync()
        {
            var releases = await GetAllReleasesAsync();
            if(releases == null || releases.Count == 0) return null;

            Root? result = null;

            foreach(var release in releases)
            {
                bool hasPackage= false;
                foreach(var asset in release.assets!)
                {
                    if(asset.name == "Package.zip")
                    {
                        hasPackage = true;
                        break;
                    }
                }
                if (hasPackage)
                {
                    result = release;
                    break;
                }
            }

            return result;
        }
    }
}
