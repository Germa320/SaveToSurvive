using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBehaviour : MonoBehaviour
{
    float speed = 7f;
    public string direction;
    float lifeDuration = 25f;

    // Update is called once per frame
    void Update()
    {
        lifeDuration -= Time.deltaTime;
        Vector3 position = transform.position;

        if(direction == "left")
        {
            position.x -= Time.deltaTime * speed;
        }
        else if(direction == "right")
        {
            position.x += Time.deltaTime * speed;
        }

        transform.position = position;

        if(lifeDuration<= 0)
        {
            Destroy(gameObject);
        }
    }
}
