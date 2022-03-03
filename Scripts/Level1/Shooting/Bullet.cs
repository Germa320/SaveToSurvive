using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Bullet : MonoBehaviour
{
    float speed = 50f;
    public float lifeDuration = 2f;
    private float lifeTimer;

    public bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (start)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<AnimatorDisable>().DisableCharacter();
        }

        if (other.tag == "Player")
        {
            other.GetComponent<NewPlayerController>().death = true;
        }
    }

}
