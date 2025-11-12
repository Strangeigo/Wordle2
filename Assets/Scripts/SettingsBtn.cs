using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsBtn : MonoBehaviour
{
    private Button currentBtn;

    // Start is called before the first frame update
    void Start()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(GoToSettings);
    }

    private void GoToSettings()
    {
        SceneManager.LoadScene("LettersAmntSelect");

    }
}
