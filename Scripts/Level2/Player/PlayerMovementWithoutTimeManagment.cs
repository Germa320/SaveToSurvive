using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementWithoutTimeManagment : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float timeSpeed = 1;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public bool movement = false;
    public bool playSound = false;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(SceneManager.GetActiveScene().name != "Level0Scene")
        {
            if (move.x != 0 || move.z != 0)
            {
                movement = true;
            }
            else
            {
                movement = false;
            }

            if (movement && !playSound)
            {
                audioSource.Play();
                playSound = true;
            }

            if (!movement && playSound)
            {
                audioSource.Stop();
                playSound = false;
            }
        }
     

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
