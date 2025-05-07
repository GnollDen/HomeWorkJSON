using System.Text.Json;
using System.Reflection;

namespace HomeWorkJSON;

public static class Settings
{
    public static string JsonFileName { get;  set; }
    public static string CSVFileName { get;  set; }
    public static string FilePath { get;  set; }
    public static FileType DefFileType { get;  set; }
    public static bool FileReaded { get;  set; }

    public static string FullPathCSV => Path.Combine(FilePath ?? "", CSVFileName ?? "");
    public static string FullPathJSON => Path.Combine(FilePath ?? "", JsonFileName ?? "");

    public enum FileType
    {
        Json,
        Csv
    }

    private class SettingsModel
    {
        public string JsonFileName { get; set; }
        public string CSVFileName { get; set; }
        public string FilePath { get; set; }
        public string DefFileType { get; set; }
    }

    public static void ReadSettings()
    {
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string twoLevelsUp = Directory.GetParent(Directory.GetParent(Directory.GetParent(exePath).FullName).FullName).FullName;
            string configPath = Path.Combine(twoLevelsUp, "settings.json");
            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Settings file not found at: {configPath}");
                return;
            }

            var json = File.ReadAllText(configPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var config = JsonSerializer.Deserialize<SettingsModel>(json, options);

            JsonFileName = config.JsonFileName;
            CSVFileName = config.CSVFileName;
            FilePath = twoLevelsUp;

            DefFileType = Enum.TryParse(config.DefFileType, true, out FileType parsedType)
                ? parsedType
                : FileType.Json;

            FileReaded = false;
            Console.WriteLine("Settings loaded successfully.");
            
    }

    public static void ChangeFileType()
    {
        DefFileType = DefFileType == FileType.Json ? FileType.Csv : FileType.Json;
    }
}