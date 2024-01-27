using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ImageSlideshow : MonoBehaviour
{
    public RawImage imageDisplayParent; // Parent RawImage to fit images into
    public RawImage imageDisplayChild; // Child RawImage for displaying images
    public Button selectFolderButton; // Button to trigger folder selection
    public Toggle subfolderToggle; // Toggle for including subfolders
    public TMP_InputField intervalInputField; // InputField for adjusting interval
    
    public GameObject UI;

    private List<Texture2D> images = new List<Texture2D>();
    private Coroutine slideshowCoroutine;
    private float interval = 2.0f;
    private bool isPaused = false;

    void Start()
    {
        selectFolderButton.onClick.AddListener(OpenFolderSelection);
        intervalInputField.onEndEdit.AddListener(UpdateInterval);
        intervalInputField.text = "0,5";
        ContinueSlideshow(); // Start the slideshow initially
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            UI.SetActive(!UI.activeSelf);
        }
    }

    void UpdateInterval(string value)
    {
        if (float.TryParse(value, out float result))
        {
            interval = Mathf.Max(result, 0.01f); // Clamp interval to minimum 0.01f
        }
        else
        {
            intervalInputField.text = interval.ToString(); // Revert to the previous valid value
        }
    }

    void OpenFolderSelection()
    {
        string path = UnityEditor.EditorUtility.OpenFolderPanel("Select Folder", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            images.Clear();
            LoadImagesFromFolder(path, subfolderToggle.isOn);
        }
    }

    void LoadImagesFromFolder(string folderPath, bool includeSubfolders)
    {
        string[] allowedExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
        SearchOption searchOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        foreach (string file in Directory.GetFiles(folderPath, "*.*", searchOption))
        {
            if (IsFileExtensionAllowed(file, allowedExtensions))
            {
                byte[] fileData = File.ReadAllBytes(file);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
                images.Add(texture);
            }
        }

        if (slideshowCoroutine == null)
        {
            ContinueSlideshow();
        }
    }

    bool IsFileExtensionAllowed(string filePath, string[] extensions)
    {
        string fileExtension = Path.GetExtension(filePath);
        foreach (string ext in extensions)
        {
            if (fileExtension.ToLower().EndsWith(ext))
                return true;
        }
        return false;
    }

    IEnumerator PlaySlideshow()
    {
        int index = 0;
        while (true)
        {
            if (!isPaused && images.Count > 0)
            {
                imageDisplayChild.texture = images[index];
                ScaleImageToFitParent();

                index = (index + 1) % images.Count;
            }
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    void StartSlideshow()
    {
        if (slideshowCoroutine == null)
        {
            slideshowCoroutine = StartCoroutine(PlaySlideshow());
        }
    }
    
    void ContinueSlideshow()
    {
        isPaused = false;
        StartSlideshow();
    }

    void ScaleImageToFitParent()
    {
        if (imageDisplayChild.texture != null)
        {
            float parentAspect = (float)imageDisplayParent.rectTransform.rect.width / imageDisplayParent.rectTransform.rect.height;
            float imageAspect = (float)imageDisplayChild.texture.width / imageDisplayChild.texture.height;

            if (imageAspect > parentAspect)
            {
                imageDisplayChild.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imageDisplayParent.rectTransform.rect.width / imageAspect);
                imageDisplayChild.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageDisplayParent.rectTransform.rect.width);
            }
            else
            {
                imageDisplayChild.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageDisplayParent.rectTransform.rect.height * imageAspect);
                imageDisplayChild.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imageDisplayParent.rectTransform.rect.height);
            }
        }
    }
}
