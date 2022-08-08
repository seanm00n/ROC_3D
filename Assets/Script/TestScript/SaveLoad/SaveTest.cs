using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROC;


[System.Serializable]
public class Data
{
    public List<int> userStructureData = new List<int>();
    public List<Transform> structureTransform = new List<Transform>();
}

public class SaveTest : MonoBehaviour
{
    private Data data = new();
    
    private void LoadSaveContent(Data loadedContent)
    {
        Debug.Log("LoadSaveContent() called!");
        for (int i = 0; i < loadedContent.userStructureData.Count; i++)
        {
            Debug.Log(loadedContent.userStructureData[i]);
            Debug.Log(loadedContent.structureTransform[i]);
        }
    }
    
    private void Start()
    {
        Debug.Log("Press S for save, L for load");
    }

    private void Update()
    {
        // Save file
        if (Input.GetKeyDown(KeyCode.S))
        {
            try
            {
                SaveManager.Save("TestJson.json", data);
                LoadSaveContent(data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        // Load file
        if (Input.GetKeyDown(KeyCode.L))
        {
            try
            {
                var loadedContent = SaveManager.Load<Data>("TestJson.json");
                LoadSaveContent(loadedContent);

                data = loadedContent;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
