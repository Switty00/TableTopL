using System.Collections;
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
    public PlayerData CurrentPlayerData => players[currentPlayerIndex].GetComponentInChildren<PlayerData>();
    public bool skipNextPlayer = false;
    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);
        for (int i = 0; i < players.Length; i++)
        {
            bool active = i < playerCount;
            players[i].gameObject.SetActive(active);
            if (active)
            {
                var pd = players[i].GetComponentInChildren<PlayerData>();
                pd.playerIndex = i;
                minionsUI.UpdateMinions(i, pd.minions);
            }
        }
        currentPlayerIndex = 0;
        SetCurrentPlayerFlags();
        cameraFollow.target = CurrentPlayerMover.transform;
        turnUI.UpdateTurn(currentPlayerIndex);
    }
    private IEnumerator ShowBanditMessage()
    {
        turnUI.BanditMessage("Bandits stole 150 Minions");
        yield return new WaitForSeconds(3f);
        turnUI.BanditHideMessage();
    }
    public void MoveCurrentPlayer(int spaces)
    {
        if (CurrentPlayerData.doubleNextRoll)
        {
            spaces *= 2;
            CurrentPlayerData.doubleNextRoll = false;
            Debug.Log("Gunpowder Power activated! Roll doubled to " + spaces);
        }
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
            StartCoroutine(ShowBanditMessage());
            Debug.Log("Bandits stole 150 minions from player " + currentPlayerIndex);
            CurrentPlayerData.AddMinions(-150);
            minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
        };
        CurrentPlayerMover.onLandedRiskSquare = () =>
        {
            HandleRiskSquare();
        };
        CurrentPlayerMover.onLandedTerritory = (territory) =>
        {
            HandleTerritory(territory);
        };
    }
    void SetCurrentPlayerFlags()
    {
        for (int i = 0; i < players.Length; i++)
            players[i].isCurrentPlayer = (i == currentPlayerIndex);
    }

    private IEnumerator ApplyCowboyBoots()
    {
        BoardMover bootsPlayer = CurrentPlayerMover;
        var oldCallback = bootsPlayer.onMoveComplete;
        bootsPlayer.onMoveComplete = null;

        yield return new WaitForSeconds(1.5f);

        Debug.Log("Cowboy Boots activated! Moving player forward 2 spaces.");
        bootsPlayer.MoveSpaces(2, true);
        bootsPlayer.onMoveComplete = oldCallback;
    }

    public void NextTurn()
    {
        if (CurrentPlayerData.extraTurn)
        {
            Debug.Log("Player " + currentPlayerIndex + " gets an EXTRA TURN!");
            CurrentPlayerData.extraTurn = false;

            SetCurrentPlayerFlags();
            turnUI.UpdateTurn(currentPlayerIndex);
            CheckElimination(currentPlayerIndex);
            return;
        }

        int playerCount = PlayerPrefs.GetInt("PlayerCount", 2);
        int nextPlayer = currentPlayerIndex;

        do
        {
            nextPlayer = (nextPlayer + 1) % playerCount;

            if (skipNextPlayer)
            {
                Debug.Log("Cowboy Block activated! Skipping player " + nextPlayer);
                skipNextPlayer = false;
                nextPlayer = (nextPlayer + 1) % playerCount;
            }

        } while (!players[nextPlayer].gameObject.activeSelf);

        currentPlayerIndex = nextPlayer;

        SetCurrentPlayerFlags();
        turnUI.UpdateTurn(currentPlayerIndex);
        CheckElimination(currentPlayerIndex);
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
            case 1:
                reward = "Cowboy Block";
                skipNextPlayer = true;
                break;
            case 2:
                reward = "Gunpowder Power";
                CurrentPlayerData.doubleNextRoll = true;
                break;
            case 3:
                reward = "Cowboy Boots";
                StartCoroutine(ApplyCowboyBoots());
                break;
            case 4:
                reward = "Thief";
                CurrentPlayerData.AddMinions(-200);
                break;
            case 5:
                reward = "Mega Minion";
                CurrentPlayerData.AddMinions(200);
                break;
            case 6:
                reward = "Extra Turn";
                CurrentPlayerData.extraTurn = true;
                break;
        }
        minionsUI.UpdateMinions(currentPlayerIndex, CurrentPlayerData.minions);
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
    public void HandleTerritory(Territory territory)
    {
        int buyerIndex = currentPlayerIndex;
        PlayerData player = CurrentPlayerData;
        if (!territory.IsOwned)
        {
            turnUI.ShowBuyPrompt(
                $"Buy this territory for {territory.cost} minions?",
                () =>
                {
                    if (player.SpendMinions(territory.cost))
                    {
                        territory.owner = player;
                        minionsUI.UpdateMinions(buyerIndex, player.minions);
                        turnUI.ShowTimedMessage("You bought the territory!", 2f);
                    }
                    else
                    {
                        turnUI.ShowTimedMessage("Not enough minions!", 2f);
                    }
                }
            );
            return;
        }
        if (territory.owner == player)
        {
            turnUI.ShowTimedMessage("You already own this territory.", 2f);
            return;
        }
        PlayerData owner = territory.owner;
        if (player.SpendMinions(territory.rent))
        {
            owner.AddMinions(territory.rent);
            turnUI.ShowTimedMessage($"Paid {territory.rent} minions in rent.", 2f);
        }
        else
        {
            turnUI.ShowTimedMessage("You cannot afford the rent!", 2f);
        }
        minionsUI.UpdateMinions(currentPlayerIndex, player.minions);
        minionsUI.UpdateMinions(owner.playerIndex, owner.minions);
    }

    public void CheckElimination(int index)
    {
        var pd = players[index].GetComponentInChildren<PlayerData>();
        if (pd.minions < 0)
        {
            EliminatePlayer(index);
        }
    }
    public void EliminatePlayer(int index)
    {
        Debug.Log($"Player {index + 1} has been eliminated!");

        players[index].gameObject.SetActive(false);
        var pd = players[index].GetComponentInChildren<PlayerData>();
        pd.minions = 0;
        minionsUI.UpdateMinions(index, pd.minions);

        if (currentPlayerIndex == index)
            NextTurn();
    }
}
