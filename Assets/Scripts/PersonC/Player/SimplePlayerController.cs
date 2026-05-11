using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("[C] Game started");
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

        transform.Rotate(
            0f,
            horizontal * turnSpeed * Time.deltaTime,
            0f
        );
    }

    void MovePlayer()
    {
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * vertical;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}