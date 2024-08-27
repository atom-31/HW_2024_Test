using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI;

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("GameOverUI is null or has been destroyed.");
        }
    }



    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
