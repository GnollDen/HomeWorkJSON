namespace HomeWorkJSON;

public class Shop
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public Dictionary<string, Fruit>? Showcase { get; set; } = new();
    public int TotalStock => Showcase.Values.Sum(x => x.Stock);
    public int TotalRevenue => Showcase.Values.Sum(x => x.Revenue);
    
    public int TotalSold => Showcase.Values.Sum(x => x.Sold);

    public bool AddFruits(
        string name, 
        int price,
        int stock,
        int sold)
    {
        var key = name.ToLower();
        Showcase[key] = new Fruit
        {
            Name = name, 
            Price = price,
            Stock = stock,
            Sold = sold
        };
        return true;
    }

    public bool AddStock(string name, int quantity)
    {
        name = name.ToLower();
        if (!Showcase.ContainsKey(name))
        {
            return false;
        }

        if (TotalStock + quantity > TotalStock)
        {
            return false;
        }
        Showcase[name].Stock += quantity;
        return true;
    }

    public bool AddSell(string name, int quantity)
    {
        name = name.ToLower();
        if (!Showcase.ContainsKey(name))
        {
            return false;
        }
        var fruit = Showcase[name];
        if (fruit.Stock < quantity)
        {
            return false;
        }
        fruit.Stock -= quantity;
        fruit.Sold += quantity;
        return true;
    }

    public Fruit? GetFruit(string name)
    {
        name = name.ToLower();
        if (!Showcase.ContainsKey(name))
        {
            return null;
        }
        return Showcase[name];
    }

    public  static List<Shop> ByDescending(List<Shop> shops)
    {
        var list = new List<Shop>();
        list = shops.OrderByDescending(x => x.TotalRevenue).ToList();
        return list;
    }
}