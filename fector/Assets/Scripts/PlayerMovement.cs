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

    public static PlayerMovement instance;

    public float speed = 6.0f;
    public float sprint = 2.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float turnSensitivity = 4;
    public Transform head;
    bool paused = false;
    private Vector3 moveDirection = Vector3.zero;
    public Vector3 curEuler = Vector3.zero;
    public float rotY = 0f;
    public Vector3 headOffset;
    public float mouseX;
    public float mouseY;
    public bool keySprint;
    public bool keyJump;
    public float horizontal;
    public float vertical;

    void Start()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void GetInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        keyJump = Input.GetButton("Jump");
        keySprint = Input.GetButton("Sprint");

        if (characterController.isGrounded)
        {
            Transform t = head.transform;
            t.rotation = Quaternion.Euler(0, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
            moveDirection = t.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            moveDirection *= speed;
            if (keySprint)
            {
                moveDirection *= sprint;
            }

            if (keyJump)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {

    }

    public void Pause(bool pause)
    {
        paused = pause;
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        bool buildMode = Input.GetButtonDown("BuildMode");
        if (buildMode)
        {
            if (PlaceManager.instance.enabled)
            {
                PlaceManager.instance.DeactivateBuildMode();
            }
            else
            {
                PlaceManager.instance.ActivateBuildMode();
            }
        }

        head.transform.position = transform.position + headOffset;

        if (paused)
        {
            if (characterController.isGrounded)
            {
                Transform t = head.transform;
                t.rotation = Quaternion.Euler(0, t.rotation.eulerAngles.y, t.rotation.eulerAngles.z);
                moveDirection = t.TransformDirection(new Vector3(0, 0.0f, 0));
                moveDirection *= speed;
                if (keySprint)
                {
                    moveDirection *= sprint;
                }

                if (keyJump)
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);

            head.transform.localRotation = Quaternion.Euler(curEuler.x, rotY, 0);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceManager.instance.enabled)
            {
                PlaceManager.instance.Place();
            }
        }

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        GetInputs();

        float XturnAmount = mouseY * Time.deltaTime * turnSensitivity;
        curEuler = Vector3.right * Mathf.Clamp(curEuler.x - XturnAmount, -90f, 90f);


        float YturnAmount = mouseX * Time.deltaTime * turnSensitivity;
        rotY += YturnAmount;
        head.transform.localRotation = Quaternion.Euler(curEuler.x, rotY, 0);
    }
}
