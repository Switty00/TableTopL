using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardMover[] players;
    public int currentPlayerIndex = 0;

    public TurnUI_TMP turnUI;
    public DiceUI_TMP diceUI;
    public MinionsUI minionsUI;
    public CameraFollow cameraFollow;

    public BoardMover CurrentPlayerMover => players[currentPlayerIndex];
    public PlayerData CurrentPlayerData => players[currentPlayerIndex].GetComponent<PlayerData>();

    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);

        for (int i = 0; i < players.Length; i++)
            players[i].gameObject.SetActive(i < playerCount);
        for (int i = 0; i < minionsUI.minionTexts.Length; i++)
        {
            minionsUI.minionTexts[i].gameObject.SetActive(i < playerCount);
        }


        currentPlayerIndex = 0;

        SetCurrentPlayerFlags();
        cameraFollow.target = CurrentPlayerMover.transform;
        turnUI.UpdateTurn(currentPlayerIndex);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(i < playerCount);
            minionsUI.UpdateMinions(i, players[i].GetComponent<PlayerData>().minions);
        }

    }

    public void MoveCurrentPlayer(int spaces)
    {
        diceUI.ShowRoll(spaces);

        foreach (BoardMover mover in players)
            mover.onMoveComplete = null;

        cameraFollow.target = CurrentPlayerMover.transform;

        CurrentPlayerMover.onMoveComplete = NextTurn;
        CurrentPlayerMover.MoveSpaces(spaces);

        CurrentPlayerMover.onPassedCorner = () =>
        {
            CurrentPlayerData.AddMinions(50);
            minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
        };
        CurrentPlayerMover.onLandedBanditSquare = () =>
        {
            Debug.Log("Bandits stole 150 minions from player " + currentPlayerIndex);

            CurrentPlayerData.AddMinions(-150);
            minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
        };


    }


    void SetCurrentPlayerFlags()
    {
        for (int i = 0; i < players.Length; i++)
            players[i].isCurrentPlayer = (i == currentPlayerIndex);
    }



    public void NextTurn()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);
        currentPlayerIndex = (currentPlayerIndex + 1) % playerCount;

        SetCurrentPlayerFlags();
        turnUI.UpdateTurn(currentPlayerIndex);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
    }




    // Example: give Minions
    public void GiveMinionsToCurrentPlayer(int amount)
    {
        CurrentPlayerData.AddMinions(amount);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
    }

    // Example: charge Minions
    public bool ChargeCurrentPlayer(int amount)
    {
        bool success = CurrentPlayerData.SpendMinions(amount);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
        return success;
    }
}
