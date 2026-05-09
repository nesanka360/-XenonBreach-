using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;

    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0f, 2.2f, -4.5f);
    public float cameraFollowSpeed = 10f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

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

    void LateUpdate()
    {
        FollowCamera();
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

    void FollowCamera()
    {
        if (cameraTransform == null) return;

        Vector3 targetPosition =
            transform.position
            + transform.right * cameraOffset.x
            + transform.up * cameraOffset.y
            + transform.forward * cameraOffset.z;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            targetPosition,
            cameraFollowSpeed * Time.deltaTime
        );

        cameraTransform.LookAt(transform.position + Vector3.up * 1.4f);
    }
}