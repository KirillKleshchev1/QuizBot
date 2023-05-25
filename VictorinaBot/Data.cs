namespace VictorinaBot;

public class Data
{
    private readonly List<string> _questionsHistory = new() {
        "В каком году распался СССР ?", 
        "Когда отменили крепостное право ?", 
        "Когда умер Петр первый ?" };

    private readonly List<string> _answersHistory = new() { "1991", "1861", "1725" };

    public string GetQuestion(int questionIndex) => _questionsHistory[questionIndex];
    
    public bool CheckAnswer(int questionIndex, string answer) => _answersHistory[questionIndex].Equals(answer);

    private bool IsEndOfSequence(int questionIndex) => _questionsHistory.Count - 1 == questionIndex;

    public int GetNextQuestion(int questionIndex) => IsEndOfSequence(questionIndex) ? -1 : questionIndex + 1;

}