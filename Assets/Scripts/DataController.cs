using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    public TestData testData;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Lv : " + testData.level;
        testData.level++;
        saveTestDataToJson();
        testData.level = 0;
        loadPlayerDataToJson();
    }

    [ContextMenu("To Json Data")]
    void saveTestDataToJson()
    {
        string jsonData = JsonUtility.ToJson(testData, true);
        string path = Path.Combine(Application.persistentDataPath, "testData2.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void loadPlayerDataToJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "testData2.json");
        string jsonData = File.ReadAllText(path);
        testData = JsonUtility.FromJson<TestData>(jsonData);
    }
}

[System.Serializable]
public class TestData
{
    public string name;
    public int age;
    public int level;
    public bool isDead;
    public string[] items;
}