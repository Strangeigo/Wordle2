using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{
    private Button currentBtn;

    // Start is called before the first frame update
    void Start()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(Play);
    }
    private void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
}
