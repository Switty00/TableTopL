using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public GameManager gameManager;

    public void RollDice()
    {
        int roll = Random.Range(1, 7);
        gameManager.MoveCurrentPlayer(roll);
    }
}
