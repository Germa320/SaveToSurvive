using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0f;
    float velocityX = 0f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    public bool hasGun;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hasGun = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");

        //increase forward
        if (forwardPressed && velocityZ < 0.5f && !backPressed)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //decrease left
        if (leftPressed && velocityX > -0.5f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //increase right
        if (rightPressed && velocityX < 0.5f)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //decrease velocityZ (forth/back)
        if (!forwardPressed && velocityZ > 0.0f && !backPressed)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //increase back
        if (backPressed && velocityZ > -0.5f && !forwardPressed)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocityZ < 0.0f && !backPressed)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0;
        }

        animator.SetFloat("VelocityZ", velocityZ);
        animator.SetFloat("VelocityX", velocityX);
    }
}
