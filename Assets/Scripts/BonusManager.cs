using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    // List of active bonuses
    private List<Action> activeBonuses = new List<Action>();
    private List<char> vowelList = new List<char>();
    private List<char> consonantList = new List<char>();
    private string currentWord;

    private void Awake()
    {
        PopulateVowelList();
    }

    private void Start()
    {
        GetCurrentWord();

        //AddBonus(() => Bonus_GetVowels(5));

        //ApplyBonuses();

    }
    private void GetCurrentWord()
    {
        if (DataManager.Instance != null)
            currentWord = DataManager.Instance.wordToGuess;
    }
    private void PopulateVowelList()
    {
        vowelList = new List<char> { 'A', 'E', 'I', 'O', 'U', 'Y' };
        consonantList = new List<char> { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z' } ;
    }
    // Call this at the start of a new game
    public void ApplyBonuses()
    {
        foreach (var bonus in activeBonuses)
        {
            bonus.Invoke();
        }
    }

    // Call this when the player wins
    public void AddBonus(Action bonusEffect)
    {
        activeBonuses.Add(bonusEffect);
    }

    // Example of a bonus
    public void Bonus_GetVowels(int pAmount)
    {
        string pickedVowels = "";

        // Filter only vowels that are actually in the current word
        List<char> availableVowels = new List<char>();
        foreach (char v in vowelList)
        {
            if (currentWord.ToUpper().Contains(v.ToString()))
                availableVowels.Add(v);
        }

        int removedCount = 0;
        while (removedCount < pAmount && availableVowels.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, availableVowels.Count);
            char pickedChar = availableVowels[index];

            vowelList.Remove(pickedChar);
            availableVowels.RemoveAt(index);

            pickedVowels += pickedChar;
            removedCount++;
            Debug.Log(pickedChar);
        }

        gameManager.SetOrangeLetters(pickedVowels);
    }

}
