using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameOverMenu gameOverMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        gameOverMenu = FindObjectOfType<GameOverMenu>();

        if (gameOverMenu == null)
        {
            Debug.LogError("GameOverMenu not found");
        }
    }

    public void GameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.GameOver();
        }
        else
        {
            Debug.LogError("GameOverMenu is null");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
