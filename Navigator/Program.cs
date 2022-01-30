using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Navigator;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("config.json", optional: false);

IConfiguration config = builder.Build();

var commands = config.GetSection("Commands").Get<NavigatorCommand[]>()
    .ToDictionary(c => c.Short);

if (commands is null || commands.Count == 0)
{
    throw new Exception("Navigator commands were not registered");
}

static void OpenPage(string url)
{
    var myProcess = new Process();

    try
    {
        myProcess.StartInfo.UseShellExecute = true;
        myProcess.StartInfo.FileName = $"https://{url}";
        myProcess.Start();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

void HandleInput(string command)
{
    if (!commands!.TryGetValue(command, out var navigatorCommand))
    {
        throw new Exception("Navigator command does not exists");
    }

    foreach (var url in navigatorCommand.Urls)
    {
        OpenPage(url);
    } 
}

var input = Console.ReadLine();
if (input is null)
{
    throw new Exception("No command typed");
}

HandleInput(input!);