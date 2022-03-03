using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
public class DoorInteraction : MonoBehaviour
{
    public GameObject portal;
    public GameObject portalDoor;
    public GameObject doorHandle;
    PlayerMovementWithoutTimeManagment playerMovement;
    MouseLook mouseLook;
    bool isClose = false;
    public GameObject text;
    public PostProcessVolume volume;
    ChromaticAberration aberration;
    Bloom bloom;
    public GameObject pointLight;
    Vector3 doorTorationTo;
    float bloomValue = 0f;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume.profile.TryGetSettings(out aberration);
        volume.profile.TryGetSettings(out bloom);
        playerMovement = FindObjectOfType<PlayerMovementWithoutTimeManagment>();
        mouseLook = FindObjectOfType<MouseLook>();
        doorTorationTo = portalDoor.transform.rotation.eulerAngles;
        doorTorationTo.y += 90;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3))
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenDoor();
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
        else
        {
            text.SetActive(false);
        }

        if(bloom.intensity.value >= 120f && aberration.intensity.value >= 1f && portalDoor.transform.rotation.eulerAngles.y >= 90f && Indestructable.instance.prevScene == "IntroScene")
        {
            Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Level1Scene");
        }
        else if(bloom.intensity.value >= 120f && aberration.intensity.value >= 1f && portalDoor.transform.rotation.eulerAngles.y >= 90f && Indestructable.instance.prevScene == "Level1Scene")
        {
            Indestructable.instance.prevScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Level2Scene");
        }
        
    }

    public void OpenDoor()
    {
        audioSource.Play();
        mouseLook.enabled = false;
        playerMovement.enabled = false;
        StartCoroutine(RotateDoor());
        StartCoroutine(IncreaseBloom());
        while (aberration.intensity.value <= 1f)
        {
            aberration.intensity.value += Time.deltaTime/5;
        }
    }

    IEnumerator IncreaseBloom()
    {
        while (bloom.intensity.value <= 120f)
        {
            bloomValue += 0.5f;
            bloom.intensity.value = bloomValue;
            yield return null;
        }

    }

    IEnumerator RotateDoor()
    {
        while (Vector3.Distance(portalDoor.transform.rotation.eulerAngles, doorTorationTo) >= 1)
        {
            portalDoor.transform.rotation = Quaternion.Lerp(portalDoor.transform.rotation, Quaternion.Euler(doorTorationTo), Time.deltaTime);
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isClose = true;
        }
        else
        {
            isClose = false;
        }
    }
}
