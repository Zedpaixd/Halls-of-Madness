using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Min(0.1f)] public float mouseSensitivity, moveSpeed, jumpHeight, gravity, stepDistance;
    [Min(0f)] public float groundCheckStartHeight, groundCheckDistance, airAcceleration, bobUpAmplitude, bobSideAmplitude;
    Transform cam;
    CharacterController cc;
    PlayerSoundsController soundController;
    AudioSource audioSource;
    Vector2 mouseDelta, inputDir, mousePos, mousePosLastFrame;
    Vector3 velocity, moveDir, camOrigPos;
    float xRot, yRot, jumpSpeed, cumulativeDistance, cumulativeDistance2;
    public bool onGround;
    bool canJump, jumped = false, jumping = false, moving = false, rightStep;
    int invertControls = 1;
    [SerializeField] LayerMask groundCheckLayerMask;
    float raycastOriginHeightMinus;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(0);
        cc = GetComponent<CharacterController>();
        raycastOriginHeightMinus = transform.localScale.y - groundCheckStartHeight;
        soundController = GetComponent<PlayerSoundsController>();
        audioSource = GetComponent<AudioSource>();
        cumulativeDistance = stepDistance - 0.5f;
        camOrigPos = cam.localPosition;
    }

    void Update()
    {
        Forces();
        MouseDelta();
        Initial();
        Rotate();
        Directional();
        Step();
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
        if (jumped && !onGround)
        {
            jumped = false;
            jumping = true;
        }
        else if (jumping && onGround)
        {
            jumping = false;
            LandJump();
        }
    }
   
    void MouseDelta()
    {
        mouseDelta = PauseGame.paused ? Vector2.zero : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
   
    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();
        moving = ctx.performed;
    }
   
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && canJump) || PauseGame.paused) { return; }
        velocity.y = jumpSpeed;
        soundController.PlayStartJump();
        jumped = true;
    }

    void Rotate()
    {
        yRot += mouseDelta.x * mouseSensitivity * invertControls;
        yRot %= 360f;
        xRot += mouseDelta.y * mouseSensitivity * invertControls;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.eulerAngles = new Vector3(0f, yRot, 0f);
        cam.localEulerAngles = new Vector3(-xRot, 0f, 0f);
    }

    void Directional()
    {
        moveDir = Vector3.Cross(transform.right, Vector3.up) * inputDir.y + transform.right * inputDir.x;
        var move2 = new Vector2(moveDir.x, moveDir.z) * moveSpeed * invertControls;
        if (onGround)
        {
            cumulativeDistance += move2.magnitude * Time.deltaTime;
            velocity.x = move2.x;
            velocity.z = move2.y;
        }
        else
        {
            var velocity2 = new Vector2(velocity.x, velocity.z);
            if (inputDir.magnitude < 0.1f)
            {
                velocity2 -= velocity2 * Time.deltaTime * airAcceleration/1.5f;
                velocity.x = velocity2.x;
                velocity.z = velocity2.y;
            }
            else
            {
                velocity2 += move2 * Time.deltaTime * airAcceleration;
                velocity2 = Vector2.ClampMagnitude(velocity2, moveSpeed);
                velocity.x = velocity2.x;
                velocity.z = velocity2.y;
            }
            
        }
    }

    void Step()
    {
        //Head bobbing camera effect
        cam.localPosition = camOrigPos + Vector3.up * bobUpAmplitude * (Mathf.Sin(Mathf.PI * cumulativeDistance / stepDistance) - 1f)
            + Vector3.right * bobSideAmplitude * Mathf.Cos(Mathf.PI*2f*(cumulativeDistance2 / (stepDistance * 2f)));

        //Step sound
        if (cumulativeDistance >= stepDistance) 
        {
            cumulativeDistance = 0f;
            soundController.ChangeToStep();
            audioSource.Play();
            rightStep = !rightStep;
        }
        cumulativeDistance2 = cumulativeDistance + (rightStep ? 0 : stepDistance);
        print(cumulativeDistance);
        print(cumulativeDistance2);
    }

    void LandJump()
    {
        soundController.PlayLandJump();
    }

    void MoveCC()
    {
        cc.Move(velocity * Time.deltaTime);    
    }

    void StepAudio()
    {
        if (velocity.x != 0 && velocity.z != 0)
        {
            if (onGround)
            {
                if (!soundController.isWalking)
                {
                    if (soundController.CanChangeToStep())
                    {
                        soundController.ChangeToStep();
                        audioSource.Play();
                        soundController.isWalking = true;
                    }
                }
            }
            else
            {
                soundController.isWalking = false;
            }
        }
        else
        {
            soundController.isWalking = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Pickupable" || collision.transform.tag == "SpPickupable")
        {
            canJump = true;
            onGround = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Pickupable" || collision.transform.tag == "SpPickupable")
        {
        }
        if (collision.transform.tag == "Pickupable")
        {
            canJump = false;
            onGround = false;
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
