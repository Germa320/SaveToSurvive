using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPointer : MonoBehaviour
{
    public static MissionPointer instance = null;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    public GameObject player;
    public Transform placeToPointAt;
    Transform missionPointer;

    private void Awake()
    {
        missionPointer = transform;
        targetPosition = placeToPointAt.position;
        currentPosition = player.transform.position;

        if (!instance)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        } 
    }

    void Update()
    {
        if(Vector3.Distance(player.transform.position, placeToPointAt.position) > 10)
        {
            targetPosition = placeToPointAt.position;
            currentPosition = player.transform.position;
            Vector3 dir = (targetPosition - currentPosition).normalized;

            Vector3 destinatedPos = currentPosition + dir * 10;

            destinatedPos.y = missionPointer.position.y;


            while (Vector3.Distance(player.transform.position, destinatedPos) < 10f)
            {
                destinatedPos += dir;
                destinatedPos.y = missionPointer.position.y;
            }

            missionPointer.position = destinatedPos;
        }
    }

    public void ChangeTarget(Transform pointer)
    {
        placeToPointAt = pointer;
    }
}
