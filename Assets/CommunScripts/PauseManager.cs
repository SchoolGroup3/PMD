using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject MenuPausa;
    public GameObject Canvas_HUD; 
    private bool isPaused = false;
    public GameObject gameObject1;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        MenuPausa.SetActive(true);
        Time.timeScale = 0f;         
        isPaused = true;
        Canvas_HUD.SetActive(false);
    }

    public void ResumeGame()
    {
        MenuPausa.SetActive(false); 
        Time.timeScale = 1f; 
        isPaused = false;
        Canvas_HUD.SetActive(true);
    }
    public void volverMenu()
    {
        SceneManager.LoadScene("Menu");
        MenuPausa.SetActive(false);
        //Destroy(MenuPausa);
        Destroy(Canvas_HUD);
        Destroy(gameObject1);
        Time.timeScale = 1f;

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
