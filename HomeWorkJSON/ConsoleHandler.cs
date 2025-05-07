using System.Text.RegularExpressions;

namespace HomeWorkJSON;

public class ConsoleHandler
{
    private const string FileNameRegex = @"^[A-Za-z]{1,20}\.(csv|json)$";
    
    public static void PrintMenu(int marketCount)
    {
        string filePath = "";
        string fileName = "";
        string fileType = "";
        switch (Settings.DefFileType)
        {
            case Settings.FileType.Csv:
                fileName = Settings.CSVFileName;
                fileType = "CSV";
                
                break;
            case Settings.FileType.Json:
                fileName = Settings.JsonFileName;
                fileType = "JSON";
                break;
        }
        Console.Clear();
        if (Settings.FileReaded) 
        {
            IsRead(marketCount, fileType);
        }
        else
        {
            Console.Write("File is not readed.");
        }
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1 - Read the file");
        Console.WriteLine("2 - Change FileName");
        Console.WriteLine("3 - Change Path to file");
        Console.WriteLine("4 - Change File Type");
        if (!Settings.FileReaded)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        Console.WriteLine("5 - Show Markets info");
        Console.WriteLine("6 - Edit market");
        Console.WriteLine("7 - Save to file");
        Console.ResetColor();
        Console.WriteLine("0 - Exit");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
       
        Console.WriteLine($"Default path for {fileType}: {Settings.FilePath}");
        Console.WriteLine($"Default file name for {fileType}: {fileName}");
        Console.ResetColor();
        Console.Write("Enter number of commands: ");
    }

    public static void ChangeFileName()
    {
        bool exit = true;
        PrintChangeFileName();
        while (exit)
        {


            string input = Console.ReadLine().Trim().ToLower();
            switch (input)
            {
                case "abort":
                    exit = false;
                    break;
                default:
                    if (Regex.IsMatch(input, FileNameRegex))
                    {
                        Console.Write("\nNew file name is; ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(input);
                        Console.ResetColor();
                        Settings.FileType newFileType;
                        if (input.Contains("csv"))
                        {
                            newFileType = Settings.FileType.Csv;
                        }
                        else
                        {
                            newFileType = Settings.FileType.Json;
                        }
                        if (GetConfirm())
                        {
                            if (newFileType == Settings.FileType.Csv)
                            {
                                Settings.CSVFileName = input;
                            }
                            else if (newFileType == Settings.FileType.Json)
                            {
                                Settings.JsonFileName = input;
                            }
                            Settings.DefFileType = newFileType;
                            exit = false;
                            break;
                        }
                        else
                        {
                            ChangeFileName();
                            break;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.WriteLine($"File name is invalid: {input}. Please try again.");
                    }

                    break;
            }
        }
    }
    
    public static void ChangeFilePath()
    {
        bool exit = true;
        PrintChangeFilePath();
        while (exit)
        {
            string input = Console.ReadLine().Trim().ToLower();
            switch (input)
            {
                case "abort":
                    exit = false;
                    break;
                default:
                    if (IsPathValid(input))
                    {
                        Console.WriteLine($"New path to file: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(input);
                        Console.ResetColor();
                        if (GetConfirm())
                        {
                            Settings.FilePath = input;
                            exit = false;
                            break;
                        }
                        else
                        {
                            PrintChangeFileName();
                            break;
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.WriteLine($"{input}\nPath to file is not correct. Please try again.");
                    }
                    break;
            }
        }
    }

    public static void ChangeValues(List<Shop> shops)
    {
        bool exit = true;
        while (exit)
        {
            PrintMarketList(shops);
            string marketId = Console.ReadLine();
            switch (marketId)
            {
                case "0":
                    Console.Clear();
                    Console.WriteLine($"Back to Menu?");
                    if (GetConfirm())
                    {
                        exit = false;
                        break;
                    }

                    break;
                case "abort":
                    Console.Clear();
                    Console.WriteLine($"Back to Menu?");
                    if (GetConfirm())
                    {
                        exit = false;
                        break;
                    }

                    break;
                default:
                    if (DataValidator.CheckId(shops, marketId))
                    {
                        Console.WriteLine($"Specific id doesnt match market id");
                    }
                    else
                    {
                        EditMarket(shops, marketId);
                    }
                    break;
            }
            
        }
        
    }
    
    public static void PrintMarketList(List<Shop> shops)
    {
        Console.WriteLine($"Всего маркетов: {shops.Count}\n");

        string header = string.Format(
            "| {0,3} | {1,-20} | {2,12} | {3,10} | {4,8} | {5,10} |",
            "№", "Магазин", "Вместимость", "На складе", "Продано", "Выручка"
        );
        string separator = new string('-', header.Length);

        Console.WriteLine(separator);
        Console.WriteLine(header);
        Console.WriteLine(separator);

        int index = 1;
        foreach (var shop in shops)
        {
            Console.WriteLine(string.Format(
                "| {0,3} | {1,-20} | {2,12} | {3,10} | {4,8} | {5,10} |",
                index++,
                shop.Name,
                shop.Capacity,
                shop.TotalStock,
                shop.TotalSold,
                shop.TotalRevenue
            ));
        }

        Console.WriteLine(separator);
        Console.WriteLine($"abort to exit");
    }
    
    
    public static bool GetConfirm()
    {
        ConsoleKey key;
        while (true)
        {
            Console.WriteLine("\nConfirm? (y/n): ");
            key = Console.ReadKey(true)
                .Key;

            switch (key)
            {
                case ConsoleKey.Y:
                    Console.WriteLine(" — OK");
                    return true;
                case ConsoleKey.N:
                    Console.WriteLine(" — Abort");
                    return false;
                default:
                    Console.WriteLine(" — Please enter a valid key (Y or N)");
                    break;
            }
        }
    }

    static void EditMarket(List<Shop> shops, string marketId)
    {
        bool exit = true;
        while (exit)
        {
            ShowMarketInfo(shops, marketId);
            string fruitName = GetFruitName(shops, marketId);
            if (fruitName == "-0")
            {
                exit = false;
                break;
            }
            else
            {
                EditColumnValue(shops, marketId, fruitName);
            }

        }
        
    }

    private static void EditColumnValue(List<Shop> shops, string marketId, string fruitName)
    {
        int id = Convert.ToInt32(marketId) - 1;
        var shop = shops[id];
        ClearLastLines(2);
        Console.WriteLine($"Enter Column Number to Edit {fruitName}");
        bool exit = true;
        while (exit)
        {
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ClearLastLines(2);
                    Console.WriteLine($"Enter new price for {fruitName}");
                    while (true)
                    {
                        string price = Console.ReadLine();
                        if (int.TryParse(price, out int priceInt))
                        {
                            ClearLastLines(2);
                            Console.WriteLine($"New price is {priceInt}. Correct?");
                            if (GetConfirm())
                            {
                                shop.Showcase.TryGetValue(fruitName, out var fruit);
                                fruit.Price = priceInt;
                                Console.Clear();
                                ShowMarketInfo(shops, marketId);
                                Console.WriteLine($"Enter Column Number to Edit {fruitName}");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error, please try again.");
                        }
                    };
                    break;
                case "2":
                    int freeSpace = shop.Capacity - shop.TotalStock;
                    ClearLastLines(2);
                    Console.WriteLine($"Enter new quantity for {fruitName}. Available space is {freeSpace}");
                    while (true)
                    {
                        string quantity = Console.ReadLine();
                        if (int.TryParse(quantity, out int quantityInt))
                        {
                            if (shop.Capacity >= quantityInt + shop.TotalStock)
                            {
                                ClearLastLines(2);
                                Console.WriteLine($"New price is {quantityInt}. Correct?");
                                if (GetConfirm())
                                {
                                    shop.Showcase.TryGetValue(fruitName, out var fruit);
                                    fruit.Stock = quantityInt;
                                    Console.Clear();
                                    ShowMarketInfo(shops, marketId);
                                    Console.WriteLine($"Enter Column Number to Edit {fruitName}");
                                    break;
                                }
                            }
                            else
                            {
                                ClearLastLines(2);
                                Console.WriteLine($"New  value for {fruitName}: {quantityInt} is too big. Doesnt match to shop capacity");
                            }
                        }
                        else
                        {
                            ClearLastLines(2);
                            Console.WriteLine($"Error, please try again.");
                        }
                    };
                    break;
                case "3":
                    shop.Showcase.TryGetValue(fruitName, out var fruitforsale);
                    int availableStock = fruitforsale.Stock;
                    Console.WriteLine($"Make new sale for {fruitName}. Available stock is {availableStock}");
                    while (true)
                    {
                        string sale = Console.ReadLine();
                        if (int.TryParse(sale, out int saleInt))
                        {
                            if (availableStock >= saleInt)
                            {
                                ClearLastLines(2);
                                Console.WriteLine($"New price is {saleInt}. Correct?");
                                if (GetConfirm())
                                {
                                    shop.Showcase.TryGetValue(fruitName, out var fruit);
                                    fruit.Sold += saleInt;
                                    fruit.Stock -=saleInt;
                                    Console.Clear();
                                    ShowMarketInfo(shops, marketId);
                                    Console.WriteLine($"Enter Column Number to Edit {fruitName}");
                                    break;
                                }
                            }
                            else
                            {
                                ClearLastLines(2);
                                Console.WriteLine($"Not enough product for sale");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error, please try again.");
                        }
                    };
                    break;
                case "abort":
                    exit = false;
                    break;
            }
        }
        
    }

    private static string GetFruitName(List<Shop> shops, string marketId)
    {
        int id = Convert.ToInt32(marketId) - 1;
        Console.WriteLine($"Enter fruit name to edit:");
        while (true)
        {
            string fruitName = Console.ReadLine().Trim().ToLower();
            if (fruitName == "0" || fruitName == "abort") return "-0";
            if (shops[id].Showcase.ContainsKey(fruitName))
            {
                return fruitName;
            }
            else
            {
                Console.WriteLine($"{fruitName} doesnt exist, try again.");
            }
        }
    }

    private static void ShowMarketInfo(List<Shop> shops, string marketIdStr)
    {
        int id = Convert.ToInt32(marketIdStr) - 1;
        Console.WriteLine(new string('=', 70));
        Console.WriteLine($"Market Name: {shops[id].Name}");
        Console.WriteLine($"Capacity: {shops[id].Capacity}");
        Console.WriteLine($"Total stock: {shops[id].TotalStock}, Sold: {shops[id].TotalSold}, Revenue: {shops[id].TotalRevenue}");
        Console.WriteLine();
        
        string fruitHeader = string.Format(
            "  | {0,-10} | {1,8} | {2,8} | {3,8} | {4,10} |",
            "Fruit", "Price(1)", "Stock(2)", "Sold(3)", "Revenue(4)"
        );
        string fruitSeparator = "  " + new string('-', fruitHeader.Length - 2);
        Console.WriteLine(fruitSeparator);
        Console.WriteLine(fruitHeader);
        Console.WriteLine(fruitSeparator);
        foreach (var fruit in shops[id].Showcase.Values)
        {
            Console.WriteLine(string.Format(
                "  | {0,-10} | {1,8} | {2,8} | {3,8} | {4,10} |",
                fruit.Name,
                fruit.Price,
                fruit.Stock,
                fruit.Sold,
                fruit.Revenue
            ));
        }

        Console.WriteLine(fruitSeparator);
    }

    static void IsRead(int marketsCount, string fileType)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(fileType);
        Console.Write($" file successfully read and contains: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(marketsCount);
        Console.ResetColor();
    }

    static bool IsPathValid(string path)
    {
        char[] invalidChars = Path.GetInvalidPathChars();
        return !string.IsNullOrWhiteSpace(path) && path.IndexOfAny(invalidChars) == -1;
    }
    
    static void PrintChangeFileName()
    {
        Settings.FileType fileType = Settings.DefFileType;
        var fileName = "";
        if (fileType == Settings.FileType.Json)
        {
            fileName = Settings.JsonFileName;
        }
        else
        {
            fileName = Settings.CSVFileName;
        }
        Console.Clear();
        Console.Write($"Current file type: ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(fileType);
        Console.ResetColor();
        Console.Write($"\nCurrent file name is: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(fileName);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n==The File name must contain the filename extension==");
        Console.ResetColor();
        Console.WriteLine("Enter |Abort| to cansel");
        Console.WriteLine("Enter File Name: ");
        Console.WriteLine("");
    }

    static void PrintChangeFilePath()
    {
        Settings.FileType fileType = Settings.DefFileType;
        Console.Clear();
        Console.Write($"Current path to file is: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(Settings.FilePath);
        Console.ResetColor();
        Console.WriteLine("Enter |Abort| to cansel");
        Console.WriteLine("Enter the path: ");
        Console.WriteLine("");
    }

    public static void ClearLastLines(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int currentLine = Console.CursorTop - 1;
            if (currentLine < 0) break;

            Console.SetCursorPosition(0, currentLine);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }
    }
}