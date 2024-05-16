using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;
    public int index;

    public PlayerInput playerInput;

    List<string> textList = new List<string>();
    void Awake()
    {
        GetTextFromFile(textFile);
        
    }

    private void OnEnable()
    {
        textLabel.text = textList[index];
        index++;
    }
    // Update is called once per frame
    void Update()
    {
        if(playerInput.Dialog && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        if(playerInput.Dialog)
        {
            textLabel.text = textList[index];
            index++;
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }
}
