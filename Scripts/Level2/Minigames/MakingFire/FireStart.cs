using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireStart : MonoBehaviour
{
    public GameObject rockLeft;
    public GameObject rockRight;
    public ParticleSystem sparks;
    public ParticleSystem fire;
    public GameObject pileOfWood;
    public Transform firstPoint;
    public Transform secondPoint;
    public Transform thirdPoint;
    Inventory inventory;
    MouseLook mouseLook;
    PlayerMovementWithoutTimeManagment playerMovement;

    int counter = 0;
    Quaternion rotation;
    bool gameStarted = false;

    AudioSource audioSource;
    public AudioClip sparksSound;
    public AudioClip rocksHit;

    // Start is called before the first frame update
    void Start()
    {
        rotation = Camera.main.transform.rotation;
        mouseLook = FindObjectOfType<MouseLook>();
        playerMovement = FindObjectOfType<PlayerMovementWithoutTimeManagment>();
        inventory = FindObjectOfType<Inventory>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && inventory.CheckIfContains("Rock", 2) && inventory.CheckIfSelected("Rock") && Vector3.Distance(transform.position, pileOfWood.transform.position) < 4)
        {
            StartTheGame();
        }

        MoveRocks();

        if(counter == 3)
        {
            mouseLook.enabled = true;
            playerMovement.enabled = true;

            rockRight.SetActive(false);
            rockLeft.SetActive(false);

            Camera.main.transform.rotation = rotation;
            fire.gameObject.SetActive(true);
            QuestManager.instance.QuestChange(GetComponent<Quest>().index);
            audioSource.Play();
            counter = 0;
        }
    }

    void StartTheGame()
    {
        mouseLook.enabled = false;
        playerMovement.enabled = false;
        Tween tween0 = Camera.main.transform.DOLookAt(pileOfWood.transform.position, 1);
        rockRight.SetActive(true);
        rockLeft.SetActive(true);
        gameStarted = true;
        rockRight.SetActive(true);
        rockLeft.SetActive(true);
    }

    void MoveRocks()
    {
        if (gameStarted && Input.GetMouseButtonUp(0))
        {
            StartCoroutine(MoveRock());
        }
    }

    IEnumerator MoveRock()
    {
        Tween tween1 = rockRight.transform.DOMove(firstPoint.transform.position, 0.3f);
        audioSource.PlayOneShot(rocksHit);
        audioSource.PlayOneShot(sparksSound);
        yield return tween1.WaitForCompletion();
        
        sparks.Play();
        
        Tween tween2 = rockRight.transform.DOMove(secondPoint.transform.position, 0.3f);
        
        yield return tween2.WaitForCompletion();
        counter++;

        Tween tween3 = rockRight.transform.DOMove(thirdPoint.transform.position, 0.4f);

        yield return tween3.WaitForCompletion();
    }
}
