using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextOverlay : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_Text textOverlay;

    private string[] arr;
    private string wordSeperator = ",";
    private string storySeperator = "\n";

    public void StartWords()
    {
        StopAllCoroutines();
        textOverlay.text = "";
        arr = input.text.Split(wordSeperator);
        StartCoroutine(ShowWord());
    }

    public void StartPreset(string words)
    {
        StopAllCoroutines();
        textOverlay.text = "";
        arr = words.Split(wordSeperator);
        StartCoroutine(ShowWord());
    }

    private IEnumerator ShowWord()
    {
        int last = -1;
        while (true)
        {
            int random = RandomExcept(0, arr.Length, last);
            last = random;
            textOverlay.text = arr[random];
            yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
            textOverlay.text = "";
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
    }

    public void StartStory()
    {
        StopAllCoroutines();
        textOverlay.text = "";
        arr = input.text.Split(storySeperator);
        StartCoroutine(ShowStory());
    }
    
    private IEnumerator ShowStory()
    {
        int index = 0;
        while (index < arr.Length)
        {
            textOverlay.text = arr[index];
            yield return new WaitForSeconds(3);
            textOverlay.text = "";
            yield return new WaitForSeconds(1);
            index++;
        }
    }

    public void StopAll()
    {
        StopAllCoroutines();
        textOverlay.text = "";
    }


    public int RandomExcept(int min, int max, int except)
    {
        int randomNr = except;

        while (randomNr == except)
        {
            randomNr = Random.Range(min, max);
        }

        return randomNr;
    }
}