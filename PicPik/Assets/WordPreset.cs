using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordPreset : MonoBehaviour
{
    public string listName;
    [TextArea(5, 50)]
    public string wordList;

    private TextOverlay overlay;

    private void Start()
    {
        overlay = FindObjectOfType<TextOverlay>();
        
        GetComponentInChildren<TMP_Text>().text = listName;
        name = listName;
        
        GetComponent<Button>().onClick.AddListener(StartWords);
    }

    private void StartWords()
    {
        overlay.StartPreset(wordList);
    }
}
