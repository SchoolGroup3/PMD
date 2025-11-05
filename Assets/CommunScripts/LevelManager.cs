using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    // ✅ AÑADE estas variables
    private int totalCollectibles = 0;
    private int collectedCount = 0;
    private bool allCollectiblesCollected = false;
    
    void Awake()
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
    

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reiniciar contadores en nuevo nivel
        totalCollectibles = 0;
        collectedCount = 0;
        allCollectiblesCollected = false;
        CountCollectibles();
    }
    

    void Start()
    {
        CountCollectibles();
    }
    
    void CountCollectibles()
    {
        // Contar todos los objetos Coleccionable en la escena
        Coleccionable[] collectibles = FindObjectsOfType<Coleccionable>();
        totalCollectibles = collectibles.Length;
        Debug.Log($"Coleccionables en nivel: {totalCollectibles}");
    }
    

    public void CollectItem()
    {
        collectedCount++;
        Debug.Log($"Coleccionados: {collectedCount}/{totalCollectibles}");
        
        // Verificar si se recolectaron todos
        if (collectedCount >= totalCollectibles && totalCollectibles > 0)
        {
            allCollectiblesCollected = true;
            Debug.Log("¡Todos los coleccionables recolectados! Meta activada.");
        }
    }
    

    public bool CanPassToNextLevel()
    {
        return allCollectiblesCollected || totalCollectibles == 0;
    }
    
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}