using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour
{
    public Transform playerTransform;
    private float followSpeed = 1f;
    private float offsetX = 0f;
    private float offsetY = 0f;
    private float offsetZ = -12f;

    private Vector3 cameraPosition;

    private Map player_location;
    private Vector3 player_position;
    // Start is called before the first frame update
    void Start()
    {
        //playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (BattleManager.instance.isBattle)
        {
            FixCameraPositionBattle();
            return;
        }

        FixCameraPositionX();
        FixCameraPositionY();
        cameraPosition.z = playerTransform.position.z + offsetZ;

        transform.position = 
            Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime * 2);
    }

    private void FixCameraPositionBattle()
    {
        transform.position = new Vector3(180, 1399, -10);
    }

    private void FixCameraPositionX()
    {
        player_location = PlayerManager.instance.location;
        player_position = PlayerManager.instance.transform.position;

        // Village
        if (player_location.Equals(Map.VILLAGE) && player_position.x > 17.5f)
        {
            cameraPosition.x = 17.5f;
            return;
        }
        else if (player_location.Equals(Map.VILLAGE) && player_position.x < -17.5f)
        {
            cameraPosition.x = -17.5f;
            return;
        }
        else if (player_location.Equals(Map.SCHOOL) && player_position.x > 5.8f)
        {
            cameraPosition.x = 5.8f;
            return;
        }
        else if (player_location.Equals(Map.SCHOOL) && player_position.x < -5.8f)
        {
            cameraPosition.x = -5.8f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon1))
        {
            cameraPosition.x = -300f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon2))
        {
            cameraPosition.x = -200f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon3))
        {
            cameraPosition.x = -100f;
            return;
        }

        cameraPosition.x = playerTransform.position.x + offsetX;
    }

    private void FixCameraPositionY()
    {
        player_location = PlayerManager.instance.location;
        player_position = PlayerManager.instance.transform.position;

        // Village
        if (player_location.Equals(Map.VILLAGE) && player_position.y > 12f)
        {
            cameraPosition.y = 12f;
            return;
        } 
        else if (player_location.Equals(Map.VILLAGE) && player_position.y < -12f)
        {
            cameraPosition.y = -12f;
            return;
        }
        else if (player_location.Equals(Map.SCHOOL) && player_position.y > 46.5f)
        {
            cameraPosition.y = 46.5f;
            return;
        }
        else if (player_location.Equals(Map.SCHOOL) && player_position.y < 23.6f)
        {
            cameraPosition.y = 23.6f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon1))
        {
            cameraPosition.y = 199f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon2))
        {
            cameraPosition.y = 199f;
            return;
        }
        else if (player_location.Equals(Map.Dungeon3))
        {
            cameraPosition.y = 199f;
            return;
        }

        cameraPosition.y = playerTransform.position.y + offsetY;
    }
}
