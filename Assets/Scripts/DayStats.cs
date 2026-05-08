using UnityEngine;

public static class DayStats
{
    public static int total;
    public static int correct;
    public static int incorrect;
    public static int depression = 50;
    public static int wealth = 50;
    public static int correctAdmitted;
    public static int daySalary;

    private static bool wealthApplied;
    private const int WealthPerCorrectAdmit = 5;
    private const int WealthPenaltyAfterFirstMistake = 5;
    private const int CorrectAdmitHappiness = 5;
    private const int IncorrectAdmitHappiness = 1;
    private const int CorrectRejectHappiness = -2;
    private const int IncorrectRejectHappiness = -6;

    public static void Reset()
    {
        total = 0;
        correct = 0;
        incorrect = 0;
        correctAdmitted = 0;
        daySalary = 0;
        wealthApplied = false;
    }

    public static bool RecordDecision(bool admitted, bool shouldAdmit)
    {
        bool decisionCorrect = admitted == shouldAdmit;

        if (decisionCorrect)
            correct++;
        else
            incorrect++;

        if (decisionCorrect && admitted)
            correctAdmitted++;

        if (admitted)
            depression += decisionCorrect ? CorrectAdmitHappiness : IncorrectAdmitHappiness;
        else
            depression += decisionCorrect ? CorrectRejectHappiness : IncorrectRejectHappiness;

        depression = Mathf.Clamp(depression, 0, 100);
        return decisionCorrect;
    }

    public static void ApplyEndOfDayWealth()
    {
        if (wealthApplied)
            return;

        daySalary = correctAdmitted * WealthPerCorrectAdmit
            - Mathf.Max(0, incorrect - 1) * WealthPenaltyAfterFirstMistake;
        wealth = Mathf.Clamp(wealth + daySalary, 0, 100);
        wealthApplied = true;
    }
}
