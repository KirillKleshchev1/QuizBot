namespace VictorinaBot
{ 
    public class BotLogic
    {
        private readonly Data _data = new ();
        private readonly Dictionary<long, List<int>> _globals = new ();

        private static string CheckAnswer(string message, int questionIndex, Data data) => 
            data.CheckAnswer(questionIndex, message) ? "Ответ правильный" : "Ответ неправильный";


        private static string SendQuestion(int questionIndex, Data data) =>
            "Вопрос " + (questionIndex + 1) + ": " + data.GetQuestion(questionIndex);

        public (string, int) Parser(string message, long chatId)
        {
            if (!_globals.ContainsKey(chatId))
                _globals.Add(chatId, new List<int> {-1, -1});
            
            if (_globals[chatId][0] >= 0)
            {
                _globals[chatId][1] = _data.GetNextQuestion(_globals[chatId][0]);
                var text = CheckAnswer(message, _globals[chatId][0], _data);
                if (_globals[chatId][1] == -1)
                {
                    _globals[chatId][0] = -1;
                    text += "\nВикторина пройдена, выберите следующую тему";

                    return (text, 1);
                }
                
                _globals[chatId][0] = _globals[chatId][1];    
                text += "\n" + SendQuestion(_globals[chatId][0], _data);
                return (text, -1);
            }
            
            if (message.ToLower() == "/start")
            {
                return ("Добро пожаловать", 0);
            }

            if (message == "Пройти викторину")
            {
                return ("Выберите тему", 1);
            }

            if (message == "История")
            {
                _globals[chatId][0] = 0;
                return (SendQuestion(_globals[chatId][0], _data), -1);
            }
            
            return ("Нераспознанная команда", -1);
            
        }
    }
}