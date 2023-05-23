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

        private static int _questionIndex = -1;

        public static async Task SendQuestion(ITelegramBotClient botClient, Int64 chatId) {
            await botClient.SendTextMessageAsync(chatId: chatId,
                text: BotLogic.SendQuestion(_questionIndex));
        }

        public static async Task CheckAnswer(ITelegramBotClient botClient, Int64 chatId, Message message) {
            await botClient.SendTextMessageAsync(chatId: chatId, BotLogic.CheckAnswer(message.Text, _questionIndex));
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("Пройти викторину") },
            });

            var keyboard2 = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("История") },
                new[] { new KeyboardButton("География") },
                new[] { new KeyboardButton("Математика") },
            });

            if (_questionIndex >= 0) {
                await CheckAnswer(botClient, chatId, message);
                _questionIndex++;
                if (_questionIndex >= Data.questionsHistory.Count) {
                    await botClient.SendTextMessageAsync(chatId: chatId,
                        text: "Викторина пройдена, выберите следующую тему",
                        replyMarkup: keyboard2);
                    _questionIndex = -1;
                }
                else {
                    await SendQuestion(botClient, chatId);
                }
            }

            if (message.Text.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(chatId: chatId,
                text: "Добро пожаловать",
                replyMarkup: keyboard);
                return;
            }
            
            if (message.Text == "Пройти викторину")
            {
                await botClient.SendTextMessageAsync(chatId: chatId,
                text: "Выберите тему",
                replyMarkup: keyboard2);
                return;
            }
            
            if (message.Text == "История")
            {
                _questionIndex = 0;
                await SendQuestion(botClient, chatId);
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