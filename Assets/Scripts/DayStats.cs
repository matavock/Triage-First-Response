public static class DayStats
{
    public static int total;      // всего пациентов
    public static int correct;    // верных решений
    public static int incorrect;  // неверных решений

    public static void Reset()
    {
        total = 0;
        correct = 0;
        incorrect = 0;
    }
}