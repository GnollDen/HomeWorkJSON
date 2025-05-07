using HomeWorkJSON;



Settings.ReadSettings();
List<Shop> shops = new List<Shop>();
while (true)
{
    ConsoleHandler.PrintMenu(shops.Count);
    string? input = Console.ReadLine();
    switch (input)
    {
        case "1":
            shops = FileHadler.ReadFromFile();
            if (shops.Count > 0)
            {
                Settings.FileReaded = true;
            }
            break;
        case "2":  
            ConsoleHandler.ChangeFileName();
            break;
        case "3":
            ConsoleHandler.ChangeFilePath();
            break;
        case "4":
            Settings.ChangeFileType();
            break;
        case "5":
            if (Settings.FileReaded)
            {
                ConsoleHandler.PrintMarketList(Shop.ByDescending(shops));
                ConsoleHandler.ClearLastLines(2);
                Console.WriteLine($"Back to main menu:");
                ConsoleHandler.GetConfirm();
            }
            break;
        case "6":
            if (Settings.FileReaded)
            {
                ConsoleHandler.ChangeValues(shops);
            }
            break;
        case "7":
            if (Settings.FileReaded)
            {
                FileHadler.SaveToFile(shops);
            }
            break;
        case "0": 
            Environment.Exit(0);
            break;
    }
}