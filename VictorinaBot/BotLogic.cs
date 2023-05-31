namespace VictorinaBot
{ 
    public class BotLogic
    {
        private readonly Data Data = new ();
        private Dictionary<long, List<int>> globals = new ();

        public static string CheckAnswer(string message, int questionIndex, Data Data, long chatId)
        {
            if (Data.CheckAnswer(questionIndex, message))
            {
                Data.ChangeStatistics(chatId, 1, 1);
                return "Ответ правильный";
            }
            
            Data.ChangeStatistics(chatId, 1, 0);
            return "Ответ неправильный";
        }


        public static string SendQuestion(int questionIndex, Data Data) =>
            "Вопрос " + (questionIndex + 1) + ": " + Data.GetQuestion(questionIndex);

        public static string SendStatistics(long chatId, Data data)
        {
            var statistic = data.GetStatistics(chatId);
            return "Вы ответили правильно " + statistic.Item2 + " раз из " + statistic.Item1 + " вопросов";
        } 

        public (string, string) Parser(string message, long chatId)
        {
            if (!globals.ContainsKey(chatId))
                globals.Add(chatId, new List<int> {-1, -1});
            
            if (globals[chatId][0] >= 0)
            {
                globals[chatId][1] = Data.GetNextQuestion(globals[chatId][0]);
                var text = CheckAnswer(message, globals[chatId][0], Data, chatId);
                if (globals[chatId][1] == -1)
                {
                    globals[chatId][0] = -1;
                    text += "\nВикторина пройдена, выберите следующую тему";

                    return (text, "Themes");
                }
                
                globals[chatId][0] = globals[chatId][1];    
                text += "\n" + SendQuestion(globals[chatId][0], Data);
                return (text, "");
            }
            
            if (message.ToLower() == "/start")
            {
                return ("Добро пожаловать", "Start");
            }

            if (message == "Пройти викторину")
            {
                return ("Выберите тему", "Themes");
            }

            if (message == "История")
            {
                globals[chatId][0] = 0;
                return (SendQuestion(globals[chatId][0], Data), "");
            }

            if (message == "Посмотреть статистику")
            {
                return (SendStatistics(chatId, Data), "");
            }

            return ("Нераспознанная команда", "");
            
        }
     }
}