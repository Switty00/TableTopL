using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int minions = 0;
    public bool extraTurn = false;
    public bool doubleNextRoll = false;
    public List<string> items = new List<string>();
    public int playerIndex;

    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log("Player received item: " + item);
    }

    public void AddMinions(int amount)
    {
        minions += amount;
    }

    public bool SpendMinions(int amount)
    {
        if (minions < amount) return false;
        minions -= amount;
        return true;
    }
}
