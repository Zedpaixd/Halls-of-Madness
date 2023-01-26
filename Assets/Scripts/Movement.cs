using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Min(0.1f)] public float mouseSensitivity, baseMoveSpeed, baseJumpHeight, gravity;
    Transform cam;
    CharacterController cc;
    Vector2 mouseDelta, inputDir, mousePos, mousePosLastFrame;
    Vector3 velocity, moveDir;
    float xRot, yRot, moveSpeed, jumpHeight, jumpSpeed;
    public bool onGround;
    bool canJump;
    void Start() 
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(0);
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        Forces();
        MouseDelta();
        Initial();
        Rotate();
        Directional();
        MoveCC();
    }
    void Forces()
    {
        if (!onGround)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else if (velocity.y < -0.1f)
        {
            velocity.y = 0f;
        }
    }
    void Initial()
    {
        moveSpeed = baseMoveSpeed;
        jumpHeight = baseJumpHeight;
        jumpSpeed = Mathf.Sqrt(jumpHeight * gravity * 2f);
        canJump = onGround;
    }
    //public void OnLook(InputAction.CallbackContext ctx)
    //{
    //    mouseDelta = ctx.ReadValue<Vector2>();
    //}
    void MouseDelta()
    {
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //mouseDelta = mousePos - mousePosLastFrame;
        //mousePosLastFrame = mousePos;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !canJump) { return; }
        velocity.y = jumpSpeed;
    }
    void Rotate()
    {
        yRot += mouseDelta.x * mouseSensitivity;
        yRot %= 360f;
        xRot += mouseDelta.y * mouseSensitivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.eulerAngles = new Vector3(0f, yRot, 0f);
        cam.transform.localEulerAngles = new Vector3(-xRot, 0f, 0f);
    }

    void Directional()
    {
        moveDir = Vector3.Cross(transform.right, Vector3.up) * inputDir.y + transform.right * inputDir.x;

        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;
    }
    void MoveCC()
    {
        cc.Move(velocity * Time.deltaTime);
    }
}
