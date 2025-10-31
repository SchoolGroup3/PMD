using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter Instance;
    
    [SerializeField] private TMP_Text deathText;
    private int totalDeaths = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateHUD();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementDeaths()
    {
        totalDeaths++;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (deathText != null)
        {
            deathText.text = "Deaths: " + totalDeaths;
        }
    }
}