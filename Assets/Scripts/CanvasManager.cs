using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject loading_scene;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        loading_scene.SetActive(true);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (loading_scene.activeSelf && timer > 3f)
        {
            loading_scene.SetActive(false);
        }
    }
}
