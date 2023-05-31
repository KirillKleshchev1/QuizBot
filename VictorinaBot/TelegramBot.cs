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
        public static BotLogic BotLogic = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            var _keyboards = new Dictionary<string, ReplyKeyboardMarkup>();
            var keyboard1 = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Пройти викторину") },
                new[] { new KeyboardButton("Посмотреть статистику") }
            });

            _keyboards["Start"] = keyboard1;

            var keyboard2 = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("История") },
                new[] { new KeyboardButton("География") },
                new[] { new KeyboardButton("Математика") },
                new[] { new KeyboardButton("Посмотреть статистику") }
            });

            _keyboards["Themes"] = keyboard2;

            var parser = BotLogic.Parser(message.Text, chatId);

          
            if (!_keyboards.ContainsKey(parser.Item2))
                await botClient.SendTextMessageAsync(chatId: chatId, text: parser.Item1,
                    cancellationToken: cancellationToken);
            else
                await botClient.SendTextMessageAsync(chatId: chatId, text: parser.Item1,
                    replyMarkup: _keyboards[parser.Item2], cancellationToken: cancellationToken);
            
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            return Task.CompletedTask;
        }


        public static void StartBot()
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