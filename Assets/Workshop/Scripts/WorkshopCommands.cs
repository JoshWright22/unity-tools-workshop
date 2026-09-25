using Yarn.Unity;

// Commands and functions the .yarn file can call.
public static class WorkshopCommands
{
    // In Yarn: <<give_coins 3>>
    [YarnCommand("give_coins")]
    public static void GiveCoins(int amount)
    {
        if (ScoreCounter.Instance != null) ScoreCounter.Instance.Add(amount);
    }

    // In Yarn: {coins()}
    [YarnFunction("coins")]
    public static int Coins()
    {
        return ScoreCounter.Instance != null ? ScoreCounter.Instance.score : 0;
    }
}
