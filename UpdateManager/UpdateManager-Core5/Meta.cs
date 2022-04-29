using System.IO;

namespace UpdateManager_Core
{
    public class Meta
    {
        static string filename = "vers.mta";
        public string VersionIdentifier { get; set; } = "";
        public static Meta Load()
        {
            try
            {
                if (!File.Exists(filename)) return new Meta();
                return File.ReadAllText(filename).Deserialize<Meta>();
            }
            catch { return null; }
        }

        public void Save() =>
            File.WriteAllText(filename, this.Serialize());

        public bool CheckVersion(Models.Root release)
        {
            if(VersionIdentifier.Trim() != release.id.ToString().Trim()) return false;
            return true;
        }

        public void Patch(Models.Root release)
        {
            VersionIdentifier = release.id.ToString().Trim();
            this.Save();
        }
    }
}
