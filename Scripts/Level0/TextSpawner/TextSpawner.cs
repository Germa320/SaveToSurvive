using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    public GameObject leftWall;
    public GameObject rightWall;

    public GameObject textToSpawn;

    float zDownRange;
    float zUpperRange;

    float yDownRange;
    float yUpperRange;

    float randZPos;
    float randYPos;

    float timeInterval = 0.02f;
    string direction;

    private void Start()
    {
        //Z range
        zDownRange = leftWall.transform.position.z - 75f;
        zUpperRange = leftWall.transform.position.z + 75f;

        //Y range
        yDownRange = leftWall.transform.position.y - 30f;
        yUpperRange = leftWall.transform.position.y + 30f;
    }

    Vector3 GetRandomPosition()
    {
        //takes point on Z axis
        randZPos = Random.Range(zDownRange, zUpperRange);
        //takes point on Y axis
        randYPos = Random.Range(yDownRange, yUpperRange);

        return new Vector3(0f, randYPos, randZPos);
    }

    string GetRandomDirection()
    {
        string dir = "";
        float decision = Random.Range(0f, 2f);

        if(decision <= 1f)
        {
            dir = "right";
        }
        else if(decision > 1f && decision <=2)
        {
            dir = "left";
        }

        return dir;
    }

    // Update is called once per frame
    void Update()
    {
        timeInterval -= Time.deltaTime;

        if(timeInterval <= 0)
        {
            Vector3 newPos = GetRandomPosition();
            direction = GetRandomDirection();

            if (direction == "right")
            {
                newPos.x = leftWall.transform.position.x;
            }
            else if(direction == "left")
            {
                newPos.x = rightWall.transform.position.x;
            }

            GameObject text = SpawnText(newPos);
            text.GetComponent<TextBehaviour>().direction = direction;


            timeInterval = 0.1f;
        }
    }

    GameObject SpawnText(Vector3 textPos)
    {
        GameObject newText = Instantiate(textToSpawn, textPos, textToSpawn.transform.rotation);
        newText.GetComponent<TextBehaviour>().enabled = true;
        return newText;
    }
}
