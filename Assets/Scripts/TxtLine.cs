using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxtLine : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    [Header("文本文件")]
    public TextAsset textFile;
    public int index;

    List<string> textList = new List<string>();

    void Awake()
    {
        GetTextFormFile(textFile, textLabel);
        // int = 0;
        // OnEnable();
    }

    private void OnEnable() {
        textLabel.text = textList[index];
        index ++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && index >= textList.Count)
        {
            gameObject.SetActive(false);
            // index = 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // index++;
            // if (index >= textFile.text.Split('\n').Length)
            // {
            //     index = 0;
            // }
            // GetTextFormFile(textFile);
            textLabel.text = textList[index];
            index++;
        }
    }
    
    void GetTextFormFile(TextAsset file, Text textLabel)
    {   
        textList.Clear();
        index = 0;
        if (file){
        // string[] txts = file.text.Split('\n');
        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
        }
        else
        {
            textList.Add(textLabel.ToString());
        }
    }
}
