using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject MenuPausa;
    public GameObject Canvas_HUD; // referencia al Canvas del menú de pausa
    private bool isPaused = false;

    void Update()
    {
        // Si el jugador pulsa Escape, alternamos pausa
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
        MenuPausa.SetActive(true); // muestra el menú
        Time.timeScale = 0f;         // detiene el tiempo
        isPaused = true;
        Canvas_HUD.SetActive(false);
    }

    public void ResumeGame()
    {
        MenuPausa.SetActive(false); // oculta el menú
        Time.timeScale = 1f;          // reanuda el tiempo
        isPaused = false;
        Canvas_HUD.SetActive(true);
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
