namespace VictorinaBot;

public class Data
{
    private List<string> questionsHistory = new() {
        "В каком году распался СССР ?", 
        "Когда отменили крепостное право ?", 
        "Когда умер Петр первый ?" };

    private List<string> answersHistory = new() { "1991", "1861", "1725" };

    private List<(long, int, int)> statistics = new() {(12, 3, 1), (22, 3, 5), (32, 10, 6)}; 
    

    public int Find(long chatId)
    {
        var left = 0;
        var right = statistics.Count;
        var middle = right / 2;
        while (true)
        {
            if (statistics[middle].Item1 == chatId) 
                return middle;
            
            if (middle == left)
                return -1;
            
            if (chatId < statistics[middle].Item1)
                right = middle;
            
            else 
                left = middle;
            

            middle = (left + right) / 2;
        }
    }

    public (int, int) GetStatistics(long chatId)
    {
        var tmp = Find(chatId);
        return tmp >= 0 ? (statistics[tmp].Item2, statistics[tmp].Item3) : (0, 0);
    } 

    public void ChangeStatistics(long chatId, int all, int right)
    {
        var index = Find(chatId);
        if (index >= 0)
        {
            var tmp = statistics[index];
            statistics[index] = (tmp.Item1, tmp.Item2 + all, tmp.Item3 + right);
        }

        else
        {
            statistics.Add((0, 0, 0));
            var i = statistics.Count - 2;
            for (; i >= 0 && statistics[i].Item1 > chatId; i--)
            {
                statistics[i + 1] = statistics[i];
            }

            statistics[i + 1] = (chatId, all, right);
        }
    }

    public string GetQuestion(int questionIndex) => questionsHistory[questionIndex];
    
    public bool CheckAnswer(int questionIndex, string answer) => answersHistory[questionIndex].Equals(answer);

    private bool IsEndOfSequence(int questionIndex) => questionsHistory.Count - 1 == questionIndex;

    public int GetNextQuestion(int questionIndex) => IsEndOfSequence(questionIndex) ? -1 : questionIndex + 1;

}