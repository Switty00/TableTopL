using UnityEngine;
using TMPro;

public class MinionsUI : MonoBehaviour
{
    public TMP_Text[] minionTexts; // One slot per player

    public void UpdateMinions(int playerIndex, int amount)
    {
        if (playerIndex < 0 || playerIndex >= minionTexts.Length)
            return;

        minionTexts[playerIndex].text = $"Minions: {amount}";
    }
}
