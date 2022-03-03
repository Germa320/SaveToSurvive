using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public bool chase = true;
    public bool death = false;
    public bool weaponEquipped = true;
    public bool hitByPlayer = false;
    public bool findWeapon = false;
    bool readyToPunch = false;
    public GameObject gun;
    public GameObject enemyModel;
    public Transform gunHandler;
    public LayerMask gunLayer;
    public float fieldOfView = 90;
    public float distance = 5;
    Vector3 gunPosition;
    Quaternion gunRotation;
    NavMeshAgent enemyAgent;
    public Animator enemyAnimator;
    public GameObject gunToFind;

    public string state;

    public List<GameObject> enemyComponents = new List<GameObject>();

    AudioSource audioSource;
    public AudioClip deathClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gunPosition = gun.transform.localPosition;
        gunRotation = gun.transform.localRotation;

        state = "pistolwalk";
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    public void DropEnemyWeapon()
    {
        gun.GetComponent<Gun>().EnemyDropWeapon(death);
    }

    // Update is called once per frame
    void Update()
    {
        if (chase && !death && weaponEquipped) // start chasing
        {
            enemyAgent.SetDestination(player.transform.position);
            state = "pistolwalk";
        }

        if(!weaponEquipped && !death && !readyToPunch && !hitByPlayer && !findWeapon) // start running without a gun
        {
            enemyAgent.SetDestination(player.transform.position);
            state = "running";
        }

        if(Vector3.Distance(player.transform.position, transform.position) <= 5f && !death && !readyToPunch && !hitByPlayer && state != "pistolwalk") // start fighting
        {
            state = "fighting";      
        }

        if(Vector3.Distance(player.transform.position, transform.position) >= 5f && readyToPunch && !death && !hitByPlayer && !weaponEquipped) // stop fighting, start running
        {
            state = "running";
        }

        if (death)
        {
            state = "killed";
        }

        if (hitByPlayer && !death)
        {
            state = "hitByPlayer";
        }

        if(state == "running" && CheckIfGunNearby() != null && Vector3.Distance(player.transform.position, transform.position) >= 5f && !death && !readyToPunch && !hitByPlayer && !findWeapon && !weaponEquipped)
        {
            gunToFind = CheckIfGunNearby();
            state = "findGun";
            findWeapon = true;
        }

        ExecuteState();
    }

    private GameObject CheckIfGunNearby()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Collider[] hits = Physics.OverlapSphere(transform.position, distance, gunLayer);

        Collider bestHit = null;
        float bestHitDistance = float.MaxValue;
        for (int i = 0; i < hits.Length; i++)
        {
            Vector3 directionToGun = hits[i].transform.position - transform.position;

            if (Vector3.Angle(transform.forward, directionToGun) < fieldOfView)
            {

                if ((bestHit == null || directionToGun.sqrMagnitude < bestHitDistance) && hits[i].transform.tag == "Gun")
                {
                    bestHit = hits[i];
                    bestHitDistance = directionToGun.sqrMagnitude;
                }
            }
        }

        if (bestHit != null && !bestHit.transform.GetComponent<Gun>().playerEquipped && bestHit.transform.GetComponent<Gun>().bulletsCount > 0)
        {
            return bestHit.gameObject;
        }    

        return null;
    }

    public void ExecuteState()
    {
        switch (state)
        {
            case "pistolwalk":
                enemyModel.transform.localEulerAngles = new Vector3(0, 25, 0);
                enemyAgent.isStopped = false;
                enemyAnimator.SetBool("fighting", false);
                enemyAnimator.SetBool("isRunning", false);
                break;

            case "running":
                enemyModel.transform.localEulerAngles = new Vector3(0, 0, 0);
                enemyAgent.isStopped = false;
                enemyAnimator.SetBool("fighting", false);
                enemyAnimator.SetBool("isRunning", true);
                readyToPunch = false;
                break;

            case "fighting":
                enemyModel.transform.localEulerAngles = new Vector3(0, -30, 0);
                enemyAgent.isStopped = true;
                enemyAnimator.SetBool("fighting", true);
                readyToPunch = true;

                break;

            case "killed":

                break;

            case "hitByPlayer":

                break;
            case "findGun":
                
                if(Mathf.Abs(Vector3.Distance(transform.position, gunToFind.transform.position)) < 5f)
                {
                    Debug.Log(gunToFind);
                    enemyModel.transform.localEulerAngles = new Vector3(0, 25, 0);
                    gun = gunToFind;
                    gun.GetComponent<Rigidbody>().isKinematic = true;
                    gun.GetComponent<Rigidbody>().useGravity = false;
                    gun.transform.GetComponent<Gun>().enemyController = this;
                    gun.transform.GetComponent<Gun>().enemyEquipped = true;
                    weaponEquipped = true;
                    gun.transform.parent = gunHandler;
                    gun.transform.localPosition = gunPosition;
                    gun.transform.localRotation = gunRotation;
                    
                    enemyAgent.SetDestination(player.transform.position);
                    chase = true;
                    findWeapon = false;
                    gun.GetComponent<Gun>().PlayPickGunSound();
                    state = "pistolWalk";
                    gunToFind = null;
                }
                else
                {
                    chase = false;
                    enemyAgent.SetDestination(gunToFind.transform.position);
                }
                
                break;
            case null:

                break;
        }
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathClip);
    }
}
