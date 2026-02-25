using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
public class TurnUI_TMP : MonoBehaviour
{
    public TMP_Text turnText;
    public TMP_Text messageText;
    public TMP_Text banditText;
    public TMP_Text buyText;
    public GameObject messagePanel;
    public GameObject banditPanel;
    public GameObject buyPanel;
    public Button buyButton;
    public Button cancelButton;
    void Start()
    {
        buyPanel.SetActive(false);
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
    public void ShowBuyPrompt(string message, System.Action onBuy)
    {
        buyPanel.SetActive(true);
        buyText.text = message;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            buyPanel.SetActive(false);
            onBuy?.Invoke();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            buyPanel.SetActive(false);
        });
    }
    public void ShowTimedMessage(string message, float duration = 2f)
    {
        StartCoroutine(ShowTimedMessageRoutine(message, duration));
    }
    private IEnumerator ShowTimedMessageRoutine(string message, float duration)
    {
        messageText.text = message;
        messagePanel.SetActive(true);

        yield return new WaitForSeconds(1);

        messagePanel.SetActive(false);
    }
}
