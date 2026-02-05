using UnityEngine;
using TMPro;

public class TurnUI_TMP : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text messageText;
    public GameObject messagePanel;
    public TMP_Text banditText;
    public GameObject banditPanel;

    public void Start()
    {
        messagePanel.SetActive(false);
        banditPanel.SetActive(false);
    }

    public void UpdateTurn(int playerIndex)
    {
        turnText.text = $"Player <b>{playerIndex + 1}</b>'s Turn";
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }
    public void HideMessage()
    {
        Debug.Log("Hiding panel");
        messagePanel.SetActive(false);
    }
    public void BanditMessage(string message)
    {
        banditText.text = message;
        banditPanel.SetActive(true);
    }
    public void BanditHideMessage()
    {
        Debug.Log("Hiding Panel");
        banditPanel.SetActive(false);
    }
}
