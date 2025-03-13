public static class ScoreManager
{
    public static int Tank1Score { get; private set; }
    public static int Tank2Score { get; private set; }

    public static void IncrementTank1Score()
    {
        Tank1Score++;
    }

    public static void IncrementTank2Score()
    {
        Tank2Score++;
    }

    public static void ResetScores()
    {
        Tank1Score = 0;
        Tank2Score = 0;
    }
}