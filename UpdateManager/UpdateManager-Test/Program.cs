using UpdateManager_Core;
UpdateManager UpdateManager = new UpdateManager("ManamanaTheReal499", "Webuntis-Desktop");


Console.WriteLine("sending request");

var response = UpdateManager.GetFirtPackageAsync().Result;

Console.WriteLine("got response: ");

Console.WriteLine(response?.name);
Console.WriteLine(response?.assets?[0].name);