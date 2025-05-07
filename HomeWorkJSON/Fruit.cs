namespace HomeWorkJSON;

public class Fruit
{
    public required string Name { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
    public int Sold { get; set; }
    public int Revenue => Price * Sold;
}