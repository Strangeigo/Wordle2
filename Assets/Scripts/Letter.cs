using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public char Char;
    [SerializeField] private TMP_Text charTxt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayLetter(char pLetter)
    {
        charTxt.text = pLetter.ToString();
    }
    public void SetColor(Material pMaterial)
    {
        GetComponent<MeshRenderer>().material = pMaterial;
    }
}
