using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 target;
    public Vector2 playerPosition;
    public static MouseMovement instance;

    public bool isMoving;

    public GameObject clickParticle;
    public float particleTimer;

    void Start()
    {
        target = transform.position;
        instance = this;
        isMoving = true;
        particleTimer = 0;
    }

    void FixedUpdate()
    {
        // EventSystem.current.IsPointerOverGameObject() UI 위에있을때 감지

        // Debug.Log(Input.mousePosition);

        if (GameManager.instance.isBattle)
        {
            return;
        }

        if (!isMoving || GameManager.instance.isAction)
        {
            target = GetComponent<RectTransform>().position;
            ObjectController.instance.SetAnimator(ViewDirection.NONE);
            return;
        }

        if (particleTimer > 0)
        {
            particleTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            playerPosition = GetComponent<RectTransform>().position;

            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            if (particleTimer <= 0)
            {
                GameObject go = Instantiate(clickParticle);
                go.transform.position = new Vector2(target.x, target.y + 1f);
                particleTimer = 0.5f;
            }

            if (playerPosition.x < target.x)
            {
                PlayerManager.instance.GetComponent<SpriteRenderer>().flipX = true;
                ObjectController.instance.SetAnimator(ViewDirection.LEFT);

/*                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingLeft", true);
                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingDown", false);
                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingUp", false);*/
            }
            else
            {
                PlayerManager.instance.GetComponent<SpriteRenderer>().flipX = false;
                ObjectController.instance.SetAnimator(ViewDirection.RIGHT);

/*                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingLeft", true);
                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingDown", false);
                PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingUp", false);*/
            }

            if (Mathf.Pow(target.x - playerPosition.x, 2) < Mathf.Pow(target.y - playerPosition.y, 2))
            {
                if (playerPosition.y - target.y < 0)
                {
                    ObjectController.instance.SetAnimator(ViewDirection.UP);
/*                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingLeft", false);
                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingDown", false);
                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingUp", true);*/
                }
                else
                {
                    ObjectController.instance.SetAnimator(ViewDirection.DOWN);
/*                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingLeft", false);
                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingDown", true);
                    PlayerManager.instance.GetComponent<ObjectController>().animator.SetBool("isWalkingUp", false);*/
                }
            }
        }

        if (Mathf.Abs(GetComponent<RectTransform>().position.x - target.x) < 0.01f && Mathf.Abs(GetComponent<RectTransform>().position.y - target.y) < 0.01f)
        {
            ObjectController.instance.SetAnimator(ViewDirection.NONE);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void stopMovement()
    {
        target = GetComponent<RectTransform>().position;
    }
}
