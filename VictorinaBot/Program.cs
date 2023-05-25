namespace VictorinaBot
{ 
    public class Program
    {
        private static readonly string?  Mode = Console.ReadLine();
        private static void Main()
        {
            if (Mode == "1")
                TelegramBot.StartBot();
        }
    }
}