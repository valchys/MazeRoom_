using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    public static VictoryMenu Instance;

    public GameObject menuUI;
    public Button continueButton;
    public Button quitButton;
    private string currentSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        menuUI.SetActive(false);
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void Show()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }
    void ContinueGame()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        string nextSceneName = "level" + (currentSceneIndex + 1);
        SceneManager.LoadScene(nextSceneName);
    }

    void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
