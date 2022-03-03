using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class NewPlayerController : MonoBehaviour
{
    public static NewPlayerController instance;
    public Camera playerCamera;
    public GameObject middleArmMover;
    public Transform gunHolder;

    MouseLook mouseLook;
    PlayerMovement playerMovement;
    public PostProcessVolume volume;
    LensDistortion lensDisortion;
    AutoExposure autoExposure;
    Quaternion fallRotation;

    public bool death = false;
    public GameObject gameOver;

    public Vector3 middleArmRotationEquipped = new Vector3(19.6753063f, -134.65f, -15.048f);
    public Vector3 middleArmRotationUnequipped = new Vector3(38.5070686f, 291.85434f, 46.8850975f);

    AudioSource audioSource;
    public AudioClip playerDeath;
    bool hasPlayed = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameOver.SetActive(false);
        fallRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -90);
        instance = this;
        playerMovement = GetComponent<PlayerMovement>();
        mouseLook = FindObjectOfType<MouseLook>();
        volume.profile.TryGetSettings(out lensDisortion);
        volume.profile.TryGetSettings(out autoExposure);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EquippWeapon();
        }

        if (death)
        {
            audioSource.clip = null;
            PlayDeathSound();
            mouseLook.enabled = false;
            playerMovement.enabled = false;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, fallRotation, 0.005f);

            if(lensDisortion.intensity >= -100)
            {
                lensDisortion.intensity.value -= 0.8f;
            }

            if(autoExposure.maxLuminance >= -9)
            {
                autoExposure.maxLuminance.value -= 0.08f;
            }

            if(lensDisortion.intensity.value <= -100 && autoExposure.maxLuminance.value <= -9)
            {
                gameOver.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
            }
        }
    }

    IEnumerator Action()
    {
        playerMovement.action = true;
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(.03f);
        playerMovement.action = false;
    }

    void EquippWeapon()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10))
        {
            if (hit.transform.tag == "Gun")
            {
                if (!playerMovement.action)
                    StartCoroutine(Action());

                hit.transform.GetComponent<Gun>().gunParent = gunHolder.gameObject;
                hit.transform.GetComponent<Gun>().PickUpWeapon();
            }
            
        }
    }

    public void PlayDeathSound()
    {
        if (!hasPlayed)
        {
            audioSource.PlayOneShot(playerDeath);
            hasPlayed = true;
        }
        
    }
}
