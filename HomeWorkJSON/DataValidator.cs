namespace HomeWorkJSON;

public class DataValidator
{
    public static bool CheckId(List<Shop> shops, string? input)
    {
        int id = Convert.ToInt32(input);
        if (id > 0 && id < shops.Count)
        {
            return false;
        }
        return true;
    }
}