using UnityEngine;
using TMPro;

public class DiceUI_TMP : MonoBehaviour
{
    public TMP_Text diceText;

    public void ShowRoll(int roll)
    {
        diceText.text = $"Rolled: <b>{roll}</b>";
    }
}
