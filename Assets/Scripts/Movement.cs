using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Min(0.1f)] public float mouseSensitivity, moveSpeed, jumpHeight, gravity;
    [Min(0f)] public float groundCheckStartHeight, groundCheckDistance;
    Transform cam;
    CharacterController cc;
    Vector2 mouseDelta, inputDir, mousePos, mousePosLastFrame;
    Vector3 velocity, moveDir;
    float xRot, yRot, jumpSpeed;
    [SerializeField] bool onGround;
    bool canJump;
    int invertControls = 1;
    [SerializeField] LayerMask groundCheckLayerMask;
    float raycastOriginHeightMinus;
    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(0);
        cc = GetComponent<CharacterController>();
        raycastOriginHeightMinus = transform.localScale.y - groundCheckStartHeight;
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

    void FixedUpdate()
    {
        onGround = Physics.Raycast(transform.position + Vector3.down * raycastOriginHeightMinus, Vector3.down, groundCheckDistance, groundCheckLayerMask);
    }
    void Forces()
    {
        if (!onGround)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else if (velocity.y < -0.01f)
        {
            velocity.y = 0f;
        }
    }

    void Initial()
    {
        jumpSpeed = Mathf.Sqrt(jumpHeight * gravity * 2f);
        canJump = onGround;
    }
   
    void MouseDelta()
    {
        mouseDelta = PauseGame.paused ? Vector2.zero : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
   
    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();
    }
   
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && canJump) || PauseGame.paused) { return; }
        velocity.y = jumpSpeed;
    }
  
    void Rotate()
    {
        yRot += mouseDelta.x * mouseSensitivity * invertControls;
        yRot %= 360f;
        xRot += mouseDelta.y * mouseSensitivity * invertControls;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.eulerAngles = new Vector3(0f, yRot, 0f);
        cam.transform.localEulerAngles = new Vector3(-xRot, 0f, 0f);
    }

    void Directional()
    {
        moveDir = Vector3.Cross(transform.right, Vector3.up) * inputDir.y + transform.right * inputDir.x;
        velocity.x = moveDir.x * moveSpeed * invertControls;
        velocity.z = moveDir.z * moveSpeed * invertControls;
    }
  
    void MoveCC()
    {
        cc.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Pickupable")
        {
            canJump = true;
            onGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("InvertControls")){
            invertControls = -1;
        }
        if (other.gameObject.CompareTag("ResetControls"))
        {
            invertControls = 1;
        }
    }
}
