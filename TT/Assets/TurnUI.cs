using UnityEngine;
using TMPro;

public class TurnUI_TMP : MonoBehaviour
{
    public TMP_Text turnText;

    public void UpdateTurn(int playerIndex)
    {
        turnText.text = $"Player <b>{playerIndex + 1}</b>'s Turn";
    }
}
