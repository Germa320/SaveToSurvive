using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorDisable : MonoBehaviour
{
    public GameObject enemy;

    public void DisableCharacter()
    {
        enemy.GetComponent<EnemyController>().PlayDeathSound();
        enemy.GetComponent<EnemyController>().death = true;
        enemy.GetComponent<EnemyController>().chase = false;
        enemy.GetComponent<EnemyController>().DropEnemyWeapon();
        //Destroy(enemy.GetComponent<NavMeshAgent>());
        enemy.GetComponent<NavMeshAgent>().enabled = false;

        foreach (GameObject obj in enemy.GetComponent<EnemyController>().enemyComponents)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().useGravity = true;
        }

        Destroy(enemy.GetComponent<Animator>());
    }

    void AffectedByHit()
    {
        StartCoroutine(WaitForAnimationToEnd());
        enemy.GetComponent<EnemyController>().hitByPlayer = true;
        enemy.GetComponent<EnemyController>().weaponEquipped = false;
        enemy.GetComponent<EnemyController>().DropEnemyWeapon();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "GivesHitObject" && !enemy.GetComponent<EnemyController>().death)
        {
            collision.gameObject.tag = "Gun";
            collision.transform.GetComponent<Gun>().PlayGunPunchSound();
            AffectedByHit();
        }
    }

    IEnumerator WaitForAnimationToEnd()
    {
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        enemy.GetComponent<Animator>().SetTrigger("pistolHit");

        yield return new WaitForSeconds(1f);

        enemy.GetComponent<EnemyController>().hitByPlayer = false;
        enemy.GetComponent<NavMeshAgent>().isStopped = false;
    }

}
