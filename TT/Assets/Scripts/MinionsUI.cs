using UnityEngine;
using TMPro;

public class MinionsUI_TMP : MonoBehaviour
{
    public TMP_Text minionsText;

    public void UpdateMinions(int amount)
    {
        minionsText.text = $"Minions:<b>{amount}</b>";
    }
}
