using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float timeSpeed = 1;

    public bool movement = false;
    public bool playSound = false;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    AudioSource audioSource;
    public AudioClip footsteps;
    Vector3 velocity;
    bool isGrounded;

    public bool action = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float time = (move.x != 0 || move.z != 0) ? 1f : .05f;
        float lerpTime = (move.x != 0 || move.z != 0) ? .05f : .5f;

        if (move.x != 0 || move.z != 0)
        {
            movement = true;
        }
        else
        {
            movement = false;
        }

        if(movement && !playSound)
        {
            audioSource.Play();
            playSound = true;
        }

        if(!movement && playSound)
        {
            audioSource.Stop();
            playSound = false;
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(!action)
        Time.timeScale = Mathf.Lerp(Time.timeScale, time, lerpTime);
    }
}
