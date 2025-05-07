using System.Text.Json.Serialization;

namespace HomeWorkJSON;

public class RawShop
{
    [JsonPropertyName("MarketName")]
    public string Name {get;set;}
    [JsonPropertyName("StorageCapacity")]
    public int Capacity {get;set;}
    [JsonPropertyName("ApplePrice")]
    public int ApplePrice {get;set;}
    [JsonPropertyName("OrangePrice")]
    public int OrangePrice {get;set;}
    [JsonPropertyName("AppleCount")]
    public int AppleCount {get;set;}
    [JsonPropertyName("AppleSold")]
    public int AppleSold {get;set;}
    [JsonPropertyName("OrangeCount")]
    public int OrangeCount {get;set;}
    [JsonPropertyName("OrangeSold")]
    public int OrangeSold {get;set;}
}