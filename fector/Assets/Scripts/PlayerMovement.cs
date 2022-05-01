/*
INCLUDES:
	Movement with WSAD and Arrows
    Turning head and body
*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float sprint = 2.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float turnSensitivity = 4;
    public Transform head;
    bool paused = false;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 curEuler = Vector3.zero;
    float rotY = 0f;
    public Vector3 headOffset;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {

        }
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            Transform t = head.transform;
            t.rotation = Quaternion.Euler(0, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
            moveDirection = t.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            moveDirection *= speed;
            if (Input.GetButton("Sprint"))
            {
                moveDirection *= sprint;
            }

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            //if (moveDirection == Vector3.zero)
            //{
            //    animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            //}
            //else if (moveDirection != Vector3.zero && !Input.GetButton("Sprint"))
            //{
            //    animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            //}
            //else if (moveDirection != Vector3.zero && Input.GetButton("Sprint"))
            //{
            //    animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
            //}
        }
        //rotate head on x-axis (Up and down)
        float XturnAmount = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * turnSensitivity;
        curEuler = Vector3.right * Mathf.Clamp(curEuler.x - XturnAmount, -90f, 90f);
        head.transform.position = transform.position + headOffset;
        //transform.rotation = Quaternion.Euler(curEuler);

        //rotate body on y-axis (Sideways)
        float YturnAmount = Input.GetAxisRaw("Mouse X") * Time.deltaTime * turnSensitivity;
        rotY += YturnAmount;
        head.transform.localRotation = Quaternion.Euler(Mathf.Clamp(curEuler.x - XturnAmount, -90f, 90f), rotY, head.transform.rotation.z);
        //transform.localRotation = Quaternion.Euler(transform.rotation.x, rotY, transform.rotation.z);

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
