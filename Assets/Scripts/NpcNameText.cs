using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcNameText : MonoBehaviour
{
    public TextMeshPro text;
    public string npcName;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.sortingOrder = 1;
        setNpcName(transform.parent.GetComponent<ObjectData>().objectName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setNpcName(string npcName)
    {
        text.text = npcName;
    }
}
