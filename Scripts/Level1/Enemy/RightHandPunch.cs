using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandPunch : MonoBehaviour
{
    public EnemyController enemy;

    Collider rightHandCollider;

    private void Start()
    {
        rightHandCollider = GetComponent<Collider>();
        rightHandCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && enemy.state == "fighting")
        {
            other.GetComponent<NewPlayerController>().death = true;
        }
    }
}
