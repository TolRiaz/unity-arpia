using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public Sprite[] buttonSprite;
    public Map movePosition;
    public Vector2 teleportPostion;
    public int changeMusic;

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
        GetComponent<SpriteRenderer>().sprite = buttonSprite[1];
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sprite = buttonSprite[0];
        PlayerManager.instance.location = movePosition;
        PlayerManager.instance.transform.position = teleportPostion;
        GameObject.Find("Main Camera").GetComponent<Transform>().position = teleportPostion;
        SoundManager.instance.stopAllSounds();
        SoundManager.instance.playMusic(changeMusic);
        MouseMovement.instance.stopMovement();
    }
}
