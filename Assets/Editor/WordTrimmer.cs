using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
public class WordListProcessorWindow : EditorWindow
{
    private int wordLength = 5;

    [MenuItem("Tools/Word List Processor")]
    public static void ShowWindow()
    {
        GetWindow<WordListProcessorWindow>("Word List Processor");
    }

    void OnGUI()
    {
        GUILayout.Label("French Word List Processor", EditorStyles.boldLabel);
        wordLength = EditorGUILayout.IntField("Word Length", wordLength);

        if (GUILayout.Button("Process Word List"))
        {
            ProcessWordList(wordLength);
        }
    }
    private void ProcessWordList(int length)
    {
        string inputPath = Application.dataPath + "/StreamingAssets/20K_ENglish.txt";
        string outputPath = Application.dataPath + $"/StreamingAssets/ENG/{length}mostusedENG.txt";

        if (!File.Exists(inputPath))
        {
            Debug.LogError("Input file not found at: " + inputPath);
            return;
        }

        var filteredWords = File.ReadLines(inputPath)
            .Select(w => RemoveDiacritics(w.Trim().ToLowerInvariant()))
            .Where(w => w.Length == length && w.All(char.IsLetter))
            .Distinct()
            .ToList();

        File.WriteAllLines(outputPath, filteredWords);
        Debug.Log($"Filtered {length}-letter words. Total: {filteredWords.Count}");
    }

    private string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
