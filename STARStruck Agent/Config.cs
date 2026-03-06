using System.Xml.Serialization;

namespace STARStruck_Agent;

[XmlRoot("AgentCfg")]
public class Config
{
    public string I2Dir { get; set; } = @"C:\Program Files (x86)\TWC\I2\";

    public string StarstruckURL { get; set; } = "http://starstruck.wmg.internal";

    public string AgentKey { get; set; } = "CHANGEME";

    public SyncSettings SyncSettings { get; set; } = new();

    private static readonly string ConfigFilePath = Path.Combine(
        AppContext.BaseDirectory,
        "config.xml"
    );

    /// <summary>
    /// Loads the configuration from config.xml in the application directory.
    /// If the file doesn't exist, creates a default one.
    /// </summary>
    /// <param name="exitIfCreated">If true, exits the application when a new config file is created.</param>
    public static Config Load(bool exitIfCreated = true)
    {
        if (!File.Exists(ConfigFilePath))
        {
            var defaultConfig = new Config();
            defaultConfig.Save();

            Console.WriteLine($"Configuration file created at: {ConfigFilePath}");
            Console.WriteLine("Before using the agent you must set the STARstruck base URL and agent token. Please do so now.");
            Environment.Exit(0);

            return defaultConfig;
        }

        var serializer = new XmlSerializer(typeof(Config));
        using var reader = new StreamReader(ConfigFilePath);
        return (Config)serializer.Deserialize(reader)!;
    }

    /// <summary>
    /// Saves the current configuration to config.xml in the application directory.
    /// </summary>
    public void Save()
    {
        var serializer = new XmlSerializer(typeof(Config));
        using var writer = new StreamWriter(ConfigFilePath);
        serializer.Serialize(writer, this);
    }
}

public class SyncSettings
{
    /// <summary>
    /// Whether to sync Managed\AffiliateAds.xml.
    /// </summary>
    public bool AffiliateAds { get; set; } = true;

    /// <summary>
    /// Whether to sync Managed\LOT8WelcomeProductTextPhrases.xml.
    /// </summary>
    public bool Greeting { get; set; } = true;

    /// <summary>
    /// Whether to sync Managed\SpecialMessage.xml.
    /// </summary>
    public bool SpecialMessage { get; set; } = true;
}