using System.Xml.Serialization;

namespace STARStruck_Agent;

[XmlRoot("Config")]
public class MachineProductCfg
{
    public ConfigDef ConfigDef { get; set; } = new();

    /// <summary>
    /// Loads the MachineProductCfg.xml file from the I2 directory.
    /// Returns null if the file doesn't exist.
    /// </summary>
    public static MachineProductCfg? Load(string i2Dir)
    {
        var configPath = Path.Combine(i2Dir, "Managed", "Config", "MachineProductCfg.xml");

        if (!File.Exists(configPath))
        {
            Console.WriteLine($"It would appear your MachineProductCfg is nonexistent. Whoops.");
            return null;
        }

        try
        {
            var serializer = new XmlSerializer(typeof(MachineProductCfg));
            using var reader = new StreamReader(configPath);
            return (MachineProductCfg)serializer.Deserialize(reader)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing MachineProductCfg.xml: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets a configuration value by key, or null if not found.
    /// </summary>
    public string? GetValue(string key)
    {
        return ConfigDef.ConfigItems.FirstOrDefault(x => x.Key == key)?.Value;
    }
}

public class ConfigDef
{
    [XmlArray("ConfigItems")]
    [XmlArrayItem("ConfigItem")]
    public List<ConfigItem> ConfigItems { get; set; } = new();
}

public class ConfigItem
{
    [XmlAttribute("key")]
    public string Key { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;
}