using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private GameObject GlobalWordPanel;
    [SerializeField] private GameObject AnswersPanel;
    [SerializeField] private Color validColor;
    [SerializeField] private Color orangeColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Material validMat;
    [SerializeField] private Material wrongMat;
    [SerializeField] private Material orangeMat;
    [SerializeField] private BonusManager bonusManager;
    private string[] rows;
    private bool hasWon = false;
    private int letterAmount;
    private GameObject letterObj;
    private List<string> words;
    [SerializeField] GameObject letterKeyPrefab;
    [SerializeField] Transform keyboardPanel; // Put in UI Canvas, anchored bottom
    public Dictionary<char, Letter> letterKeys = new Dictionary<char, Letter>();
    //List
    private HashSet<string> wordsSet;

    //Word
    private string userWord;
    private string wordToGuess;
    private int currentLetterIndex = 0;
    private int answIndex = 0;

    //Word length
    public int currentWordLength;
    //Answers
    private int answersAmount = 6;
    [SerializeField] private GameObject answerPanel;
    private GameObject answerPanelObj;

    //Victory
    [SerializeField] private GameObject VictoryScreen;
    private GameObject VictoryScreenObj;
    //Defeat
    [SerializeField] private GameObject DefeatScreen;
    private GameObject DefeatScreenObj;
    //Player
    [SerializeField] private TMP_Text playerHealth;

    private void Awake()
    {
        if (DataManager.Instance != null)
            currentWordLength = DataManager.Instance.letterAmount;
    }
    private void Start()
    {
        SetGlobalWordPanelLetterAmount();
        SpawnKeys();
        PickRandomWord(currentWordLength);
        playerHealth.text = DataManager.Instance.health.ToString();
        //bonusManager.Bonus_RemoveVowel(2);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("Letter"))
                {
                    Debug.Log("Clicked on: " + clickedObject.name);
                }
            }
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PickRandomWord(currentWordLength);
        }

        //Key Management
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c) && currentLetterIndex < (currentWordLength))
            {
                Debug.Log("You typed: " + char.ToLowerInvariant(c));
                letterObj = Instantiate(letterPrefab);
                letterObj.GetComponent<Letter>().DisplayLetter(char.ToUpperInvariant(c));
                letterObj.transform.parent = AnswersPanel.transform.GetChild(answIndex).transform.GetChild(currentLetterIndex);
                letterObj.transform.localPosition = Vector3.zero;
                letterObj.transform.localScale = new Vector3(20f, 20f, 1f);
                letterObj.transform.GetComponent<TileAnim>().Shake();
                currentLetterIndex++;
                userWord += char.ToUpperInvariant(c);
            }

        }
        //Delete Management
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!string.IsNullOrEmpty(userWord))
            {
                userWord = userWord.Substring(0, userWord.Length - 1);
                currentLetterIndex--;
                Destroy(AnswersPanel.transform.GetChild(answIndex).transform.GetChild(currentLetterIndex).GetChild(0).gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentLetterIndex == (currentWordLength))
        {
            print("VERIFY");
            if (!wordsSet.Contains(userWord.ToLowerInvariant()))
            {
                Debug.Log("Invalid word");
                InvalidWord();
                return;
            }
            VerifyWord(userWord);
            userWord = "";
            if (answIndex <= GlobalWordPanel.transform.childCount - 1 && hasWon == false)
                answIndex++;
            else
            {
                LoseHealth();
            }
            currentLetterIndex = 0;
        }
    }

    private void PickRandomWord(int pLetterAmount)
{
    string folder = DataManager.Instance.selectedLanguage; // "ENG" or "FR"
    string fileName = $"{pLetterAmount}mostused{DataManager.Instance.selectedLanguage}.txt";
    string filePath = Path.Combine(Application.streamingAssetsPath, folder, fileName);
    wordsSet = new HashSet<string>(File.ReadAllLines(filePath));


        if (File.Exists(filePath))
    {
            words = new List<string>(File.ReadAllLines(filePath));
            string randomWord = GetRandomWord();
            Debug.Log("Random word: " + randomWord);
            wordToGuess = randomWord.ToUpperInvariant();
            DataManager.Instance.wordToGuess = wordToGuess;
    }
    else
    {
        Debug.LogError($"Word list file not found at: {filePath}");
    }
}
    private string GetRandomWord()
    {
        if (words == null || words.Count == 0)
            return null;

        int index = Random.Range(0, words.Count);
        return words[index].Trim();
    }

    public void VerifyWord(string pWord)
    {
        print(wordToGuess);

        for (int i = 0; i < wordToGuess.Length; i++)
        {
            char typed = char.ToUpperInvariant(pWord[i]);
            char correct = char.ToUpperInvariant(wordToGuess[i]);

            if (typed == correct)
            {
                AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<Image>().color = validColor;
                letterKeys[typed].transform.GetComponent<Image>().material = validMat;
            }
            else if (wordToGuess.Contains(typed))
            {
                AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<Image>().color = orangeColor;
                letterKeys[typed].transform.GetComponent<Image>().material = orangeMat;
            }
            else
            {
                AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<Image>().color = wrongColor;
                AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<TileAnim>().IsWrong();
                letterKeys[typed].transform.GetComponent<Image>().material = wrongMat;
            }

        }
        if (userWord == wordToGuess)
        {
            StartCoroutine(ShowVictoryScreen());
            hasWon = true;
        }
        //else
            //LoseHealth();
    }

    void SpawnKeys()
    {
        if (DataManager.Instance == true)
        {
            switch (DataManager.Instance.selectedLanguage)
            {
                case "FR":
                    {
                        Debug.Log("FRANCAIS");

                        rows = new string[]
                        {
                            "AZERTYUIOP",
                            "QSDFGHJKLM",
                            "WXCVBN"
                        };
                        break;
                    }
                case "ENG":
                    {
                        rows = new string[]
                        {
                            "QWERTYUIOP",
                            "ASDFGHJKL",
                            "ZXCVBNM"
                        };
                        break;
                    }
                default:
                    break;
            }
        }

        for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            Transform rowParent = keyboardPanel.GetChild(rowIndex);

            for (int i = 0; i < rows[rowIndex].Length; i++)
            {
                char c = rows[rowIndex][i];

                GameObject obj = Instantiate(letterKeyPrefab);
                obj.transform.SetParent(rowParent, false);
                obj.transform.localPosition = Vector3.zero;

                Letter key = obj.GetComponent<Letter>();
                key.DisplayLetter(c);
                key.Char = c;

                letterKeys[c] = key;
            }
        }

    }


    private void SetGlobalWordPanelLetterAmount()
    {
        // Destroy existing letters in AnswersPanel
        for (int i = 0; i < AnswersPanel.transform.childCount; i++)
        {
            Transform answerRow = AnswersPanel.transform.GetChild(i);

            // Iterate backwards to avoid index shifting
            for (int j = answerRow.childCount - 1; j >= 0; j--)
            {
                Destroy(answerRow.GetChild(j).gameObject);
            }
        }

        // Optional: do the same for GlobalWordPanel if needed
        for (int i = 0; i < GlobalWordPanel.transform.childCount; i++)
        {
            Transform globalRow = GlobalWordPanel.transform.GetChild(i);
            for (int j = globalRow.childCount - 1; j >= 0; j--)
            {
                Destroy(globalRow.GetChild(j).gameObject);
            }
        }

        // Creating letters for each AnswersPanel row
        for (int i = 0; i < AnswersPanel.transform.childCount; i++)
        {
            Transform row = AnswersPanel.transform.GetChild(i);
            for (int j = 0; j < currentWordLength; j++)
            {
                GameObject answerPanelObj = Instantiate(answerPanel);
                answerPanelObj.transform.SetParent(row);
                answerPanelObj.transform.localPosition = Vector3.zero;
                answerPanelObj.transform.localScale = Vector3.one;
            }
        }
    }

    private void Victory()
    {
        VictoryScreenObj = Instantiate(VictoryScreen);
    }
    private void Defeat()
    {
        Debug.LogWarning("DEFEAT");
        DefeatScreenObj = Instantiate(DefeatScreen);
    }

    private void LoseHealth()
    {
        if (DataManager.Instance.LoseHealth())
        {
            print("Lose");
            StartCoroutine(ShowDefeatScreen());

        }
        else
            playerHealth.text = DataManager.Instance.health.ToString();
    }   

    private IEnumerator ShowVictoryScreen()
    {
        for (int i = 0; i < AnswersPanel.transform.GetChild(answIndex).transform.childCount; i++)
        {
            print(answIndex);
            AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<TileAnim>().ValidWord();
            yield return new WaitForSeconds(.2f);
        }
        Victory();
    } 
    private IEnumerator ShowDefeatScreen()
    {
        yield return new WaitForSeconds(.3f);
        Defeat();
    }

    private void InvalidWord()
    {
        for (int i = 0; i < AnswersPanel.transform.GetChild(answIndex).transform.childCount; i++)
        {
            print(answIndex);
            AnswersPanel.transform.GetChild(answIndex).transform.GetChild(i).GetComponent<TileAnim>().IsWrong();
        }
    }

    public void SetOrangeLetters(string pWord)
    {

        for (int i = 0; i < pWord.Length; i++)
        {
            char typed = char.ToUpperInvariant(pWord[i]);

            letterKeys[typed].transform.GetComponent<Image>().material = orangeMat;
        }
    }
}
