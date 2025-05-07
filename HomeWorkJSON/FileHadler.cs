using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HomeWorkJSON;

public class FileHadler
{
    public static string? HeaderString { get; private set; }
    public static List<Shop> ReadFromFile()
    {
        if (Settings.DefFileType == Settings.FileType.Csv)
        {
            return ReadFromCsv();
        } 
        return ReadFromJson();
        
    }

    private static List<Shop> ReadFromJson()
    {
        var rawJson = File.ReadAllText(Settings.FullPathJSON);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var rawShops = JsonSerializer.Deserialize<List<RawShop>>(rawJson, options);
        
        return ConvertRaw(rawShops);

    }

    private static List<Shop> ReadFromCsv()
    {
        var rawShops = new List<RawShop>();
        using (StreamReader reader = File.OpenText(Settings.FullPathCSV))
        {
            HeaderString = reader.ReadLine()!;
            while (!reader.EndOfStream)
            {
                var data = reader.ReadLine()!.Split(",");
                rawShops.Add(new RawShop
                {
                    Name = data[0],
                    Capacity = int.Parse(data[1]),
                    ApplePrice = int.Parse(data[2]),
                    OrangePrice = int.Parse(data[3]),
                    AppleCount = int.Parse(data[4]),
                    AppleSold = int.Parse(data[5]),
                    OrangeCount = int.Parse(data[6]),
                    OrangeSold = int.Parse(data[7]),
                });
            }
        }
        return ConvertRaw(rawShops);
    }

    private static List<Shop> ConvertRaw(List<RawShop> rawShops)
    {
        var shops = new List<Shop>();
        foreach (var rs in rawShops!)
        {
            var shop = new Shop
            {
                Name = rs.Name,
                Capacity = rs.Capacity,
            };
            shop.AddFruits(
                name: "Apple",
                price: rs.ApplePrice,
                stock: rs.AppleCount,
                sold: rs.AppleSold
            );
            shop.AddFruits(
                name: "Oranges",
                price: rs.OrangePrice,
                stock: rs.OrangeCount,
                sold: rs.OrangeSold
            );
            shops.Add(shop);
        }
        return shops;
    }

    public static void SaveToFile(List<Shop> shops)
    {
        if (Settings.DefFileType == Settings.FileType.Csv)
        {
             SaveToCsv(shops);
        } 
         SaveToJson(shops);
    }

    private static void SaveToJson(List<Shop> shops)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(shops, options);
        string path = Settings.FilePath+"\\new"+Settings.JsonFileName;
        File.WriteAllText(path, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        Settings.FileReaded = false;
    }

    private static void SaveToCsv(List<Shop> shops)
    {
        string path = Settings.FilePath+"\\new"+Settings.CSVFileName;
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(HeaderString);
            foreach (var shop in shops)
            {
                writer.WriteLine(
                    $"{shop.Name}," +
                    $"{shop.Capacity}," +
                    $"{shop.Showcase["apple"].Price}," +
                    $"{shop.Showcase["oranges"].Price}," +
                    $"{shop.Showcase["apple"].Stock}," +
                    $"{shop.Showcase["apple"].Sold}," +
                    $"{shop.Showcase["oranges"].Stock}," +
                    $"{shop.Showcase["oranges"].Sold}");
            }
            Settings.FileReaded = false;
        }
    }
}