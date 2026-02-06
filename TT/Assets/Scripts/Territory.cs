using UnityEngine;

public class Territory : MonoBehaviour
{
    public int territoryIndex;
    public int cost = 150;
    public int rent = 30;

    public PlayerData owner = null;

    public bool IsOwned => owner != null;
}
