using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public Sprite[] button_sprite;
    public Map move_position;
    public Vector2 teleport_postion;
    public int change_music;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().sprite = button_sprite[1];
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = button_sprite[0];
        PlayerManager.instance.location = move_position;
        PlayerManager.instance.transform.position = teleport_postion;
        GameObject.Find("Main Camera").GetComponent<Transform>().position = teleport_postion;
        SoundManager.instance.stopAllSounds();
        SoundManager.instance.playMusic(change_music);
    }
}
