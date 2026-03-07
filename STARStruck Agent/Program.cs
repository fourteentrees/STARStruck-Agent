using STARStruck_Agent;
using STARStruck_Agent.API;
using System.Security.Principal;

// Load configuration (creates default if doesn't exist)
var config = Config.Load();
var mpc = MachineProductCfg.Load(config.I2Dir);

using var client = new StarstruckClient(config);

Console.WriteLine("STARstruck Agent is running :D");
Console.WriteLine($"Using STARStruck server: {config.StarstruckURL}");
Console.WriteLine($"Using I2 directory: {config.I2Dir}");

if (!IsRunningAsAdministrator())
{
    Console.WriteLine("ERROR: This application must be run as administrator.");
    Console.WriteLine("Please right-click and select 'Run as administrator'.");
    Environment.Exit(1);
}

// Check connection
if (await client.CheckConnectionAsync())
{
    Console.WriteLine("Connected to STARStruck server");
}
else
{
    Console.WriteLine("Failed to connect to STARStruck server");
    return;
}

var headendId = mpc.GetValue("HeadendId");
var eventsDir = Path.Combine(config.I2Dir, "Managed", "Events");

// Ensure the directory exists
Directory.CreateDirectory(eventsDir);

// Download AffiliateAds.xml if enabled
if (config.SyncSettings.AffiliateAds)
{
    var content = await client.DownloadFileAsync($"/api/star/{headendId}/AffiliateAds.xml");
    if (content != null)
    {
        var savePath = Path.Combine(eventsDir, "AffiliateAds.xml");
        await File.WriteAllTextAsync(savePath, content);
        Console.WriteLine("AffiliateAds.xml downloaded successfully");
    }
}

// Download LOT8WelcomeProductTextPhrases.xml if enabled
if (config.SyncSettings.Greeting)
{
    var content = await client.DownloadFileAsync($"/api/star/{headendId}/LOT8WelcomeProductTextPhrases.xml");
    if (content != null)
    {
        var savePath = Path.Combine(eventsDir, "LOT8WelcomeProductTextPhrases.xml");
        await File.WriteAllTextAsync(savePath, content);
        Console.WriteLine("LOT8WelcomeProductTextPhrases.xml downloaded successfully");
    }
}

// Download SpecialMessage.xml if enabled
if (config.SyncSettings.SpecialMessage)
{
    var content = await client.DownloadFileAsync("/api/SpecialMessage.xml");
    if (content != null)
    {
        var savePath = Path.Combine(eventsDir, "SpecialMessage.xml");
        await File.WriteAllTextAsync(savePath, content);
        Console.WriteLine("SpecialMessage.xml downloaded successfully");
    }
}

Console.WriteLine("All downloads complete. Goodbye.");

static bool IsRunningAsAdministrator()
{
    using var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}