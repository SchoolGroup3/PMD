using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Opcionesscript : MonoBehaviour
{
    public GameObject MenuOpciones;
    public GameObject Mainmenu;
    public GameObject gameobject;


    public void OpenOptionPanel(){
        Mainmenu.SetActive(false);
        MenuOpciones.SetActive(true);
    }
    public void OpenMainMenuPanel(){
        Mainmenu.SetActive(true);
        MenuOpciones.SetActive(false);
    }
    public void QuitGame(){
        #if UNITY_EDITOR
        // Esto detiene el modo Play en el editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Esto cierra el juego en un build
        Application.Quit();
        #endif
        //Process.GetCurrentProcess().Kill();
    }

    public void PlayGame(){
        SceneManager.LoadScene("Nivel 1");
        Instantiate(gameobject, new Vector3((float)-11.04, (float)4.42, 0), Quaternion.identity);


    }
    
}
