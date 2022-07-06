using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Data
{
    public List<int> userStructureData = new List<int>();
    public List<Transform> structureTransform = new List<Transform>();
    public void LoadData()
    {
        Debug.Log("Start!");
        for (int i = 0; i < userStructureData.Count; i++)
        {
            Debug.Log(userStructureData[i]);
            Debug.Log(structureTransform[i]);
        }

    }

}
public class SaveTest : MonoBehaviour
{

    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        //Data data = new Data();
        //data.userStructureData.Add(1);
        //data.structureTransform.Add(transform);

        //string str = JsonUtility.ToJson(data);

        //File.WriteAllText(Application.dataPath + "/TestJson.json", str);

        try
        {
            string str2 = File.ReadAllText(Application.dataPath + "/TestJson.json");
            Data data3 = JsonUtility.FromJson<Data>(str2);
            data3.LoadData();
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Error");
        }
    }
        
}
