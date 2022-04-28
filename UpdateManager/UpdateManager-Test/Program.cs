using UpdateManager_Core;
UpdateManager UpdateManager = new UpdateManager("ManamanaTheReal499", "UpdateManager");

Console.WriteLine("sending request");

var response = UpdateManager.GetFirtPackageAsync().Result;

Console.WriteLine("got response: ");

Console.WriteLine(response?.name);
Console.WriteLine(response?.assets?[0].name);

var isSame = UpdateManager.CurrentVersionMeta?.CheckVersion(response!);

Console.WriteLine($"response is the same version as stored: {isSame}");


UpdateManager.DownloadFile(response!, (path) =>
{
    Console.WriteLine($"asset saved in {path}");
    UpdateManager.CurrentVersionMeta!.Patch(response!);
    UpdateManager.Install();
});

Console.ReadKey();