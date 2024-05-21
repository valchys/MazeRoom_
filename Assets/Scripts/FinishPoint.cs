using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreenManager : MonoBehaviour
{
    public GameObject victoryScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject victoryScreen = GameObject.Find("VictoryScreen"); // Replace "victoryScreen" with the actual name of your victory screen GameObject
            victoryScreen.SetActive(true);
            Time.timeScale = 0f; // Pauses the game
            Cursor.visible = true; // Makes the cursor visible
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameplayScene"); // Replace "GameplayScene" with the actual name of your gameplay scene
        Time.timeScale = 1f; // Resumes the game
        Cursor.visible = false; // Hides the cursor
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}