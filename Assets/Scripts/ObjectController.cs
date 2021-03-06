﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public JoystickValue value;
    public float joystickSpeed;

    public Animator animator;
    public static ObjectController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        //rigid.velocity = new Vector2(h * horizontalSpeed / doSomething, v * verticalSpeed);

        Vector2 speed = new Vector2(value.joyTouch.x / 100 * joystickSpeed, value.joyTouch.y / 100 * joystickSpeed);
        transform.Translate(speed);

        //SetAnimator(ViewDirection.NONE);
    }

    public void SetAnimator(ViewDirection viewDirection)
    {
        if (value.joyTouch.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (value.joyTouch.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (value.joyTouch.y > 0.6 || viewDirection == ViewDirection.UP)
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", true);
        }
        else if (value.joyTouch.y < -0.6 || viewDirection == ViewDirection.DOWN)
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingDown", true);
            animator.SetBool("isWalkingUp", false);
        }
        else if (value.joyTouch.x != 0 && value.joyTouch.y != 0 || viewDirection == ViewDirection.LEFT || viewDirection == ViewDirection.RIGHT)
        {
            animator.SetBool("isWalkingLeft", true);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", false);
        }
        else
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", false);
        }
    }
}

public enum ViewDirection
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}
