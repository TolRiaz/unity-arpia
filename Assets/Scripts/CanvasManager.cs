using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject loading_scene;
    public float timer;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("동작");
        Debug.Log("x = " + Input.GetTouch(0).position.x + ", y = " + Input.GetTouch(0).position.y);
    }

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
