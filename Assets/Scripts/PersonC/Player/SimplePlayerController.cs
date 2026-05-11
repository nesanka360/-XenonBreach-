using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    public float gravity = 20f; // Added downward acceleration force

    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotatePlayer();
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void RotatePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0f, horizontal * turnSpeed * Time.deltaTime, 0f);
    }

    void MovePlayer()
    {
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * vertical;

        // Apply constant gravity check to stick to canyon floor
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Small constant pressure to stay pinned
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Accelerate downward
        }

        moveDirection.y = verticalVelocity;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}