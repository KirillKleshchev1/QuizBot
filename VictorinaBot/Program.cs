using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace VictorinaBot
{

    public class TelegramBot
    {
        private static readonly string? Token = Environment.GetEnvironmentVariable("TOKEN");
        private static readonly ITelegramBotClient Bot = new TelegramBotClient(Token);
        public static BotLogic BotLogic = new ();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            var keyboards = new List<ReplyKeyboardMarkup>();
            
            var keyboard1 = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Пройти викторину") },
            });
            
            keyboards.Add(keyboard1);

            var keyboard2 = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("История") },
                new[] { new KeyboardButton("География") },
                new[] { new KeyboardButton("Математика") },
            });
            
            keyboards.Add(keyboard2);

            var parser = BotLogic.Parser(message.Text);
            
            switch (parser.Item2)
            {
                case >= 0:
                    await botClient.SendTextMessageAsync(chatId: chatId, text: parser.Item1, replyMarkup: keyboards[parser.Item2], cancellationToken: cancellationToken);
                    break;
                case -1:
                    await botClient.SendTextMessageAsync(chatId: chatId, text: parser.Item1, cancellationToken: cancellationToken);
                    break;
            }
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            return Task.CompletedTask;
        }

            

        private static void Main()
        {
            Console.WriteLine(Bot.GetMeAsync().Result.FirstName + " started");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions();
            
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}