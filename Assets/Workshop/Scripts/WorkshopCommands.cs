using UnityEngine;
using Yarn.Unity;

// Commands and functions the .yarn files can call.
public static class WorkshopCommands
{
    // In Yarn: <<give_coins 3>>
    [YarnCommand("give_coins")]
    public static void GiveCoins(int amount)
    {
        if (ScoreCounter.Instance != null) ScoreCounter.Instance.Add(amount);
    }

    // In Yarn: <<take_coins 5>>
    [YarnCommand("take_coins")]
    public static void TakeCoins(int amount)
    {
        if (ScoreCounter.Instance != null) ScoreCounter.Instance.Add(-amount);
    }

    // In Yarn: <<open_gate>>
    [YarnCommand("open_gate")]
    public static void OpenGate()
    {
        foreach (var gate in Object.FindObjectsByType<Gate>(FindObjectsSortMode.None))
            gate.Open();
    }

    // In Yarn: {coins()}
    [YarnFunction("coins")]
    public static int Coins()
    {
        return ScoreCounter.Instance != null ? ScoreCounter.Instance.score : 0;
    }
}
