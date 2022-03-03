using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TreeCuttingManager : MonoBehaviour
{
    PlayerMovementWithoutTimeManagment playerMovement;
    MouseLook mouseLook;
    Inventory inventory;
    public GameObject axe;
    public ParticleSystem particles;

    Vector3 axeRotationBeforeCut;
    Vector3 axeRotationAfterCut;

    public AudioSource audioSource;
    public AudioClip treeChop;

    bool isCutting = false;
    // Start is called before the first frame update
    void Start()
    {
        axe.SetActive(false);
        axeRotationBeforeCut = axe.transform.rotation.eulerAngles;
        axeRotationBeforeCut.z = 10;
        axeRotationAfterCut = axe.transform.rotation.eulerAngles;
        axeRotationAfterCut.z = -60;
        mouseLook = FindObjectOfType<MouseLook>();
        playerMovement = FindObjectOfType<PlayerMovementWithoutTimeManagment>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2) && inventory.CheckIfSelected("Axe"))
            {
                if (hit.transform.tag == "Tree" && !isCutting)
                {
                    axe.SetActive(true);
                    isCutting = true;
                    StartCoroutine(CutTree());
                }
            }
        }
    }

    IEnumerator CutTree()
    {
        axeRotationBeforeCut = axe.transform.rotation.eulerAngles;
        axeRotationBeforeCut.z = 10;
        axeRotationAfterCut = axe.transform.rotation.eulerAngles;
        axeRotationAfterCut.z = -60;
        mouseLook.enabled = false;
        playerMovement.enabled = false;

        Tween tween2 = axe.transform.DORotate(axeRotationAfterCut, 0.3f, RotateMode.Fast);

        yield return tween2.WaitForCompletion();
        particles.gameObject.SetActive(true);
        audioSource.PlayOneShot(treeChop);

        Tween tween3 = axe.transform.DORotate(axeRotationBeforeCut, 1, RotateMode.Fast);

        yield return tween3.WaitForCompletion();
        particles.Stop();

        mouseLook.enabled = true;
        playerMovement.enabled = true;
        isCutting = false;
        axe.SetActive(false);
    }
}
