using UnityEngine;
using UnityEngine.SceneManagement;


public class Opcionesscript : MonoBehaviour
{
    public GameObject MenuOpciones;
    public GameObject Mainmenu;

    public void OpenOptionPanel(){
        Mainmenu.SetActive(false);
        MenuOpciones.SetActive(true);
    }
    public void OpenMainMenuPanel(){
        Mainmenu.SetActive(true);
        MenuOpciones.SetActive(false);
    }
    public void QuitGame(){
        Application.Quit();
    }

    public void PlayGame(){
        SceneManager.LoadScene("Nivel 1");
    }
}
