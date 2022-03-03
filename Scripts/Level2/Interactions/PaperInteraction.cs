using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PaperInteraction : MonoBehaviour
{
    public RawImage paperImage;
    public GameObject inventoryPanel;
    public GameObject minimap;
    PlayerMovementWithoutTimeManagment playerMovement;
    MouseLook mouseLook;
    DoFControl depthOfFieldControl;
    public float itemDistance;
    AudioSource audioSource;
    public AudioClip paperSound;

    public Transform missionMapPointer;
    bool paperChecked = false;
    bool checkingPaper = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = FindObjectOfType<PlayerMovementWithoutTimeManagment>();
        mouseLook = FindObjectOfType<MouseLook>();
        depthOfFieldControl = FindObjectOfType<DoFControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !checkingPaper)
        {
            PaperInteract();
        }
        else if (Input.GetKeyDown(KeyCode.E) && checkingPaper)
        {
            StopInteract();
            checkingPaper = false;
        }
    }

    void PaperInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, itemDistance))
        {
            if(hit.transform.tag == "Paper")
            {
                if (!paperChecked)
                {
                    MissionPointer.instance.ChangeTarget(missionMapPointer);
                    paperChecked = true;
                }
                
                audioSource.PlayOneShot(paperSound);
                QuestManager.instance.QuestChange(GetComponent<Quest>().index);
                playerMovement.enabled = false;
                mouseLook.enabled = false;
                depthOfFieldControl.stopAdjusting = true;
                paperImage.enabled = true;
                inventoryPanel.SetActive(false);
                minimap.SetActive(false);
                checkingPaper = true;
            }
        }
    }

    void StopInteract()
    {
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        depthOfFieldControl.stopAdjusting = false;
        paperImage.enabled = false;
        inventoryPanel.SetActive(true);
        minimap.SetActive(true);
    }
}
