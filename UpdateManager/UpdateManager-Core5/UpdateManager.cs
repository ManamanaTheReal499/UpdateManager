using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpdateManager_Core.Models;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net.Http;

namespace UpdateManager_Core
{
    public class UpdateManager
    {
        public Meta CurrentVersionMeta;
        
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
            CurrentVersionMeta = Meta.Load();
        }

        public async Task<List<Root>> GetAllReleasesAsync() =>
            await api.Request<List<Models.Root>>(releasesUrl);

        public async Task<Root> GetFirtPackageAsync()
        {
            var releases = await GetAllReleasesAsync();
            if(releases == null || releases.Count == 0) return null;

            Root result = null;

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

        public void DownloadFile(Root release, Action<string> callback)
        {
            new Thread(async () =>
            {
                var url = string.Empty;
                foreach (var asset in release.assets!)
                {
                    if (asset.name == "Package.zip")
                    {
                        url = asset.browser_download_url;
                        break;
                    }
                }

                if (url == string.Empty) return;

                using (HttpClient client = new HttpClient())
                {
                    Directory.CreateDirectory("temp");
                    Directory.CreateDirectory("temp\\Package");
                    if (File.Exists(@"temp\Package.zip"))
                        File.Delete(@"temp\Package.zip");

                    await client.DownloadFileTaskAsync(url!.ToUri(), @"temp\Package.zip");
                    ZipFile.ExtractToDirectory(@"temp\Package.zip", "temp\\Package", true);
                    File.Delete(@"temp\Package.zip");
                    callback?.Invoke(@"temp\Package\");
                }
            }).Start();
        }

        public void Install(string injextCMD = "")
        {
            if (!Directory.Exists("temp")) return;
            var tempPath = $"{AppDomain.CurrentDomain.BaseDirectory}temp";
            string installerContent = $"@echo off\ntimeout 2\nXcopy {tempPath}\\Package\\ {tempPath}\\..\\ /Y /E /H /C /I\n{injextCMD}\nexit";
            File.WriteAllText("temp\\installer.cmd",installerContent);
            FileInfo f = new FileInfo("temp\\installer.cmd");

            ExecuteBatFile(f.DirectoryName!, f.FullName);
        }


        private void ExecuteBatFile(string filepath,string filename)
        {
            Process proc = null;
            try
            {
                string targetDir = string.Format(filepath); 
                proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.Arguments = string.Format("10"); 
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}
