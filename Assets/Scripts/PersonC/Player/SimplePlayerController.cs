using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    public Transform cameraTransform;

    private CharacterController controller;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera cam = Camera.main;

            if (cam != null)
                cameraTransform = cam.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("[C] Free-look controller started");
    }

    void Update()
    {
        MovePlayer();
        FreeLook();
    }

    void MovePlayer()
{
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move =
        cameraTransform.forward * z +
        cameraTransform.right * x;

    move.y = 0f;
    move.Normalize();

    if (move.magnitude > 0.1f)
    {
        transform.rotation = Quaternion.LookRotation(move);
    }

    controller.Move(move * moveSpeed * Time.deltaTime);
}

    void FreeLook()
    {
        if (cameraTransform == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -80f, 80f);

        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}