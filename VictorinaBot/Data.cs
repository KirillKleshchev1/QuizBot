namespace VictorinaBot;

public class Data
{
    private static int QuestionIndex = -1;

    public static List<string> questionsHistory = new List<string>() {
        "В каком году распался СССР ?", 
        "Когда отменили крепостное право ?", 
        "Когда умер Петр первый ?" };

    public static List<string> answersHistory = new List<string>() { "1991", "1861", "1725" };
}