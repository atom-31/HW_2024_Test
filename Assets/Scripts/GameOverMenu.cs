using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI;
    public AudioSource gameOverAudioSource;

    private void Awake()
    {
        gameOverAudioSource = GetComponent<AudioSource>();  
    }

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

            if (gameOverAudioSource != null)
            {
                gameOverAudioSource.Play();
            }
            else
            {
                Debug.Log("Eror Playing Audio!");
            }
        }
        else
        {
            Debug.LogError("GameOverUI is null");
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
