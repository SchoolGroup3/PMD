using UnityEngine;
using UnityEngine.UI;

public class ControladorSFX : MonoBehaviour
{
    public static ControladorSFX instance;

    [Header("Referencias")]
    [SerializeField] private AudioSource audioSourceSFX;   // Para efectos de sonido
    [SerializeField] private AudioSource audioSourceMusica; // Para música de fondo
    [SerializeField] private AudioClip musicaMenu;
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private Button botonMute;

    private float volumen = 1f;
    private bool estaMuteado = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar referencias
        if (audioSourceSFX == null)
            audioSourceSFX = gameObject.AddComponent<AudioSource>();

        if (audioSourceMusica == null)
            audioSourceMusica = gameObject.AddComponent<AudioSource>();

        volumen = audioSourceSFX.volume;
    }

    void Start()
    {
        // Cargar valores guardados
        float volumenGuardado = PlayerPrefs.GetFloat("volumen", 0.5f);
        int muteGuardado = PlayerPrefs.GetInt("mute", 0);

        audioSourceMusica.volume = volumenGuardado;
        estaMuteado = (muteGuardado == 1);
        audioSourceMusica.mute = estaMuteado;

        if (!audioSourceMusica.isPlaying && musicaMenu != null)
        {
            audioSourceMusica.clip = musicaMenu;
            audioSourceMusica.Play();
        }

        ConfigurarUI();
    }

    void ConfigurarUI()
    {
        if (sliderVolumen != null)
        {
            sliderVolumen.onValueChanged.RemoveAllListeners();
            sliderVolumen.value = audioSourceMusica.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (botonMute != null)
        {
            botonMute.onClick.RemoveAllListeners();
            botonMute.onClick.AddListener(ToggleMute);
        }
    }

    public void EjecutarSonido(AudioClip clip)
    {
        if (clip != null && audioSourceSFX != null)
        {
            audioSourceSFX.volume = volumen;
            audioSourceSFX.PlayOneShot(clip);
        }
    }

    void CambiarVolumen(float valor)
    {
        audioSourceMusica.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
        PlayerPrefs.Save();
    }

    public void PararSonido()
    {
        if (audioSourceSFX != null)
            audioSourceSFX.Stop();
    }

    public void CambiarMusica(AudioClip clip, bool reiniciar)
    {
        if (clip == null || audioSourceMusica == null) return;

        if (audioSourceMusica.clip == clip)
        {
            if (reiniciar)
                audioSourceMusica.time = 0f;
            return;
        }

        audioSourceMusica.clip = clip;
        audioSourceMusica.Play();
    }

    void ToggleMute()
    {
        estaMuteado = !estaMuteado;
        audioSourceMusica.mute = estaMuteado;
        PlayerPrefs.SetInt("mute", estaMuteado ? 1 : 0);
        PlayerPrefs.Save();

        if (botonMute != null)
        {

        }
 
    }
}
