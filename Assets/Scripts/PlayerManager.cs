using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Map location;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        location = Map.VILLAGE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
