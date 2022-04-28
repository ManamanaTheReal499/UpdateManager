using System.IO.Compression;

var folder = Environment.GetCommandLineArgs()[1];
var target = Environment.GetCommandLineArgs()[2];

Console.WriteLine($"{folder}; {target}");

Directory.CreateDirectory("temp");
Directory.CreateDirectory("temp\\Package");

CopyFilesRecursively(folder, "temp\\Package");

ZipFile.CreateFromDirectory("temp\\Package", target);

static void CopyFilesRecursively(string sourcePath, string targetPath)
{
    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));

    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
}