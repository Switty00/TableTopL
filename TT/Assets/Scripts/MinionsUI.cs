using UnityEngine;
using TMPro;

public class MinionsUI : MonoBehaviour
{
    public TMP_Text[] minionTexts;
    public void UpdateMinions(int playerIndex, int amount)
    {
        if (playerIndex < 0 || playerIndex >= minionTexts.Length)
            return;
        minionTexts[playerIndex].text = $"Player {playerIndex + 1}: {amount}";
    }
}

