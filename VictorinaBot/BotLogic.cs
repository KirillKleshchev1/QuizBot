namespace VictorinaBot
{ 
    public static class BotLogic
    {
        public static string CheckAnswer(string message, int QuestionIndex)
        {
            if (message.Equals(Data.answersHistory[QuestionIndex]))
            {
                return "Ответ правильный";
            }

            return "Ответ неправильный";
        }

        public static string SendQuestion(int QuestionIndex) =>
            "Вопрос " + (QuestionIndex + 1).ToString() + ": " + Data.questionsHistory[QuestionIndex];

    }
}