using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public void SelectPlayers(int count)
    {
        PlayerPrefs.SetInt("PlayerCount", count);
        SceneManager.LoadScene("GameScene");
    }
}
