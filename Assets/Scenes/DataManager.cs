using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string selectedLanguage = "ENG"; // or "FR"
    // Example data to store
    public string playerName;
    public int score;
    public string wordToGuess;
    public int letterAmount = 5;
    public int health = 5;

    private void Awake()
    {
        // Ensure only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    // Optional: Clear data between sessions
    public void ResetData()
    {
        playerName = "";
        score = 0;
        wordToGuess = "";
        letterAmount = 0;
    }

    public void SetLetterAmount(int pAmount)
    {
        letterAmount = pAmount;
    }

    public bool LoseHealth()
    {
        health--;
        if (health <= 0)
        {
            return true;
        }
        else
            return false;
    }
}