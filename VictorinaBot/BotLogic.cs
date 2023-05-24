namespace VictorinaBot
{ 
    public class BotLogic
    {
        private static readonly Data Data = new ();
        private static int _questionIndex = -1;
        public static string CheckAnswer(string message) => 
            message.Equals(Data.answersHistory[_questionIndex]) ? "Ответ правильный" : "Ответ неправильный";
        

        public static string SendQuestion() =>
            "Вопрос " + (_questionIndex + 1).ToString() + ": " + Data.questionsHistory[_questionIndex];

        public (string, int) Parser(string message)
        {
            
            if (_questionIndex >= 0)
            {
                var text = CheckAnswer(message);
                _questionIndex++;
                if (_questionIndex >= Data.questionsHistory.Count)
                {
                    _questionIndex = -1;
                    text += "\nВикторина пройдена, выберите следующую тему";

                    return (text, 1);
                }

                text += "\n" + SendQuestion();
                    
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
                _questionIndex = 0;
                return (SendQuestion(), -1);
            }
            
            return ("Нераспознанная команда", -1);
            
        }
    }
}