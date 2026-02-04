using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardMover[] players;
    public int currentPlayerIndex = 0;
    public TurnUI_TMP turnUI;
    public DiceUI_TMP diceUI;
    public MinionsUI minionsUI;
    public SideFollowCamera cameraFollow;
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
        CurrentPlayerMover.onLandedRiskSquare = () =>
        {
            HandleRiskSquare();
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
    public void GiveMinionsToCurrentPlayer(int amount)
    {
        CurrentPlayerData.AddMinions(amount);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
    }
    public bool ChargeCurrentPlayer(int amount)
    {
        bool success = CurrentPlayerData.SpendMinions(amount);
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
        return success;
    }
    public void HandleRiskSquare()
    {
        int roll = Random.Range(1, 7);
        diceUI.ShowRoll(roll);

        string reward = "";

        switch (roll)
        {
            case 1: reward = "Cowboy Block"; CurrentPlayerData.AddItem("Cowboy Block"); break;
            case 2: reward = "Gunpowder Power"; CurrentPlayerData.AddItem("Gunpowder Power"); break;
            case 3: reward = "Cowboy Boots"; CurrentPlayerData.AddItem("Cowboy Boots"); break;
            case 4: reward = "Thief"; CurrentPlayerData.AddMinions(-200); break;
            case 5: reward = "Mega Minion"; CurrentPlayerData.AddMinions(200); break;
            case 6: reward = "Extra Turn"; CurrentPlayerData.extraTurn = true; break;
        }

        StartCoroutine(ShowRiskMessages(roll, reward));
    }
    private IEnumerator ShowRiskMessages(int roll, string reward)
        {
        turnUI.ShowMessage("Risk Square! Roll the dice!");
        yield return new WaitForSeconds(1.5f);

        turnUI.ShowMessage($"You rolled a {roll}! You got: {reward}");
        yield return new WaitForSeconds(2f);

        turnUI.HideMessage();
        }   
}
