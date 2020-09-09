using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChanger : MonoBehaviour
{
    public Map map;
    public int layer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            setLayer(other.gameObject, layer);
        }
    }

    public void setLayer(GameObject gameObject, int layerIndex)
    {
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        //render.sortingLayerName = "Entity";
        render.sortingOrder = layerIndex;
        GameObject.Find("Canvas").GetComponent<CanvasManager>().changeColliderOnOff(map, layer);
    }
}
