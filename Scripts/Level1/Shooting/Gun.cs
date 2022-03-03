using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 150f;
    public float force = 100f;

    public GameObject muzzleFlash;
    public GameObject player;
    public GameObject gunParent;
    public Animator playerAnimator;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public Camera fpsCam;
    public EnemyController enemyController;

    AudioSource audioSource;
    PlayerMovement playerMovement;
    Rigidbody weaponRigid;
    MenuHandler menuHandler;
    public AudioClip gunShot;
    public AudioClip gunFloorHit;
    public AudioClip gunHumanHit;
    public AudioClip pickUpGun;
    bool alreadyPlayedSound = false;

    public bool playerEquipped = true;
    public bool enemyEquipped = true;

    float intervalBetweenEnemyShots = 3f;

    Vector3 weaponPosition = new Vector3(-0.016245611f, -0.0816428512f, -0.0253232289f);
    Quaternion weaponRotation = Quaternion.Euler(new Vector3(11.2383699f, 6.20930576f, 357.691284f));

    public int bulletsCount = 2;

    private void Start()
    {
        menuHandler = FindObjectOfType<MenuHandler>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        weaponRigid = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        fpsCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerEquipped)
        {
            if (Input.GetMouseButtonDown(0) && bulletsCount > 0 && !menuHandler.pause)
            {
                Shoot();
                bulletsCount--;
            }

            if (Input.GetKeyDown(KeyCode.R) && !menuHandler.pause)
            {
                DropWeapon();
            }

        }
        else if (enemyEquipped)
        {
            intervalBetweenEnemyShots -= Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, enemyController.gameObject.transform.forward, out hit, 20))
            {
                if (hit.transform.tag == "Player" && intervalBetweenEnemyShots <= 0 && bulletsCount > 0)
                {
                    GameObject bulletObject = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                    bulletObject.transform.forward = enemyController.gameObject.transform.forward;
                    bulletObject.GetComponent<Bullet>().start = true;
                    muzzleFlash.GetComponent<ParticleSystem>().Play();
                    intervalBetweenEnemyShots = 3f;
                    bulletsCount--;
                    audioSource.PlayOneShot(gunShot);
                }
            }

            if(bulletsCount <= 0)
            {
                EnemyDropWeaponDown();
            }
        }
    }

    IEnumerator Action()
    {
        playerMovement.action = true;
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(.08f);
        playerMovement.action = false;
    }

    void EnemyShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemyController.gameObject.transform.position, enemyController.gameObject.transform.forward, out hit, 20))
        {
            Debug.Log(hit);
            if(hit.transform.tag == "Player")
            {
                GameObject bulletObject = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
                bulletObject.transform.forward = enemyController.gameObject.transform.forward;
                bulletObject.GetComponent<Bullet>().start = true;
                muzzleFlash.GetComponent<ParticleSystem>().Play();
                intervalBetweenEnemyShots = 3f;
                bulletsCount--;
            }
        }
        
    }

    public void EnemyDropWeapon(bool death)
    {
        GetComponent<Collider>().enabled = true;
        enemyEquipped = false;
        //enemyController.weaponEquipped = false;

        weaponRigid.isKinematic = false;
        weaponRigid.useGravity = true;

        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => weaponRigid.AddForce(transform.forward * 4, ForceMode.Impulse)).SetUpdate(true).SetEase(Ease.Flash);
        s.AppendCallback(() => weaponRigid.AddTorque((transform.forward + transform.right) * 10, ForceMode.Impulse)).SetUpdate(true).SetEase(Ease.Flash); 
    }

    void EnemyDropWeaponDown()
    {
        enemyEquipped = false;
        enemyController.weaponEquipped = false;

        weaponRigid.isKinematic = false;
        weaponRigid.useGravity = true;

        transform.parent = null;
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            playerAnimator.SetTrigger("shoot");
            if (!playerMovement.action)
                StartCoroutine(Action());
            
            GameObject bulletObject = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            bulletObject.transform.forward = fpsCam.transform.forward;
            bulletObject.GetComponent<Bullet>().start = true;
            muzzleFlash.GetComponent<ParticleSystem>().Play();
            audioSource.PlayOneShot(gunShot);
        }
    }

    void DropWeapon()
    {
        playerEquipped = false;

        if (!playerMovement.action)
            StartCoroutine(Action());

        transform.tag = "GivesHitObject";
        weaponRigid.isKinematic = false;
        weaponRigid.useGravity = true;

        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => transform.parent = null);
        s.AppendCallback(() => weaponRigid.AddForce(Camera.main.transform.forward * 20, ForceMode.Impulse)).SetUpdate(true).SetEase(Ease.Flash);
        s.AppendCallback(() => weaponRigid.AddTorque((transform.forward + transform.right) * 20, ForceMode.Impulse)).SetUpdate(true).SetEase(Ease.Flash);

        StartCoroutine(rotateToDestination());
    }

    public void PickUpWeapon()
    {
        PlayPickGunSound();
        StartCoroutine(rotateToDestination2());
        playerEquipped = true;

        transform.parent = gunParent.transform;

        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;

        transform.DOLocalMove(weaponPosition, .25f).SetEase(Ease.Flash).SetUpdate(true);
        transform.DOLocalRotate(new Vector3(11.2383699f, 6.20930576f, 357.691284f), .25f).SetUpdate(true);
    }

    IEnumerator rotateToDestination()
    {
        //while(Vector3.Distance(NewPlayerController.instance.middleArmMover.transform.rotation.eulerAngles, NewPlayerController.instance.middleArmRotationUnequipped) > 1)
        //{
        //    NewPlayerController.instance.middleArmMover.transform.rotation = Quaternion.Slerp(NewPlayerController.instance.middleArmMover.transform.rotation, Quaternion.Euler(NewPlayerController.instance.middleArmRotationUnequipped), Time.deltaTime);
        //    yield return null;
        //}

        NewPlayerController.instance.middleArmMover.transform.rotation = Quaternion.Euler(NewPlayerController.instance.middleArmRotationUnequipped);
        yield return null;
    }

    IEnumerator rotateToDestination2()
    {
        //Quaternion targetRotation = NewPlayerController.instance.middleArmRotationEquipped;

        //while (Vector3.Distance(NewPlayerController.instance.middleArmMover.transform.rotation.eulerAngles, NewPlayerController.instance.middleArmRotationEquipped) >= -1)
        //{
        //    NewPlayerController.instance.middleArmMover.transform.rotation = Quaternion.Lerp(NewPlayerController.instance.middleArmMover.transform.rotation, Quaternion.Euler(NewPlayerController.instance.middleArmRotationEquipped), Time.deltaTime);
        //    yield return null;
        //}

        NewPlayerController.instance.middleArmMover.transform.rotation = Quaternion.Euler(NewPlayerController.instance.middleArmRotationEquipped);
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(audioSource.isPlaying == false && alreadyPlayedSound)
        {
            audioSource.clip = gunShot;
            alreadyPlayedSound = false;
        }
        if (collision.gameObject.tag == "Ground")
        {
            transform.tag = "Gun";
            if (!alreadyPlayedSound)
            {
                audioSource.clip = gunFloorHit;
                audioSource.PlayOneShot(gunFloorHit);
                //audioSource.Play();
                alreadyPlayedSound = true;
            }
            
        }
    }

    public void PlayGunPunchSound()
    {
        audioSource.PlayOneShot(gunHumanHit);
    }

    public void PlayPickGunSound()
    {
        audioSource.PlayOneShot(pickUpGun);
    }

    }
