using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordAmountBtn : MonoBehaviour
{
    [SerializeField] private int letterAmount = 5;
    private Button currentBtn;
    // Start is called before the first frame update
    void Start()
    {
        currentBtn = GetComponent<Button>();
        currentBtn.onClick.AddListener(ChangeLetterAmount);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeLetterAmount()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.letterAmount = letterAmount ;
        }
    }
}
