using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int minions = 500;

    public void AddMinions(int amount)
    {
        minions += amount;
    }
    public bool SpendMinions(int amount)
    {
        if (minions < amount)
            return false;

        minions -= amount;
        return true;
    }
}
