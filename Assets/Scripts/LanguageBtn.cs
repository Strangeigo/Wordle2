using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageBtn : MonoBehaviour
{
    [SerializeField] private string Language = "ENG";
    private Button currentBtn;
    // Start is called before the first frame update
    void Start()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(ChangeLanguage);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChangeLanguage()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.selectedLanguage = Language;
        }
    }
    
}
