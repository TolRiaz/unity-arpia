using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlParticle())
        {
            Destroy(gameObject);
        }
    }

    public bool controlParticle()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("ClickParticle") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f;
    }
}
