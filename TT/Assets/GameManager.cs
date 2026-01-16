using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardMover[] players;   // Assign all player pieces in Inspector
    public int currentPlayer = 0;

    public DiceUI diceUI;
    public TMP_Text turnText;

    private void Start()
    {
        diceUI.SetGameManager(this);
        HighlightCurrentPlayer();
    }

    public void PlayerRolled(int roll)
    {
        players[currentPlayer].MoveSpaces(roll);
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Length;
        HighlightCurrentPlayer();
        diceUI.EnableDice();
    }

    private void HighlightCurrentPlayer()
    {
        turnText.text = "Player " + (currentPlayer + 1) + "'s Turn";
    }

}
