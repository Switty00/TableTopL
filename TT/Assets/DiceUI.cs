using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DiceUI : MonoBehaviour
{
    private GameManager gameManager;

    public TMP_Text rollResultText;
    public TMP_Text cooldownText;
    public Button diceButton;

    public float cooldownTime = 2f;
    private bool onCooldown = false;

    public void SetGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    public void RollDice()
    {
        if (onCooldown) return;

        int roll = Random.Range(1, 7);
        rollResultText.text = "Roll: " + roll;

        gameManager.PlayerRolled(roll);

        StartCoroutine(DiceCooldown());
    }

    public void EnableDice()
    {
        diceButton.interactable = true;
        onCooldown = false;
        cooldownText.text = "";
    }

    private IEnumerator DiceCooldown()
    {
        onCooldown = true;
        diceButton.interactable = false;

        float timer = cooldownTime;

        while (timer > 0)
        {
            cooldownText.text = "Cooldown: " + timer.ToString("0.0");
            timer -= Time.deltaTime;
            yield return null;
        }

        cooldownText.text = "";
    }
}
