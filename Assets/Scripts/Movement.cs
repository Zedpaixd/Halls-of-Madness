using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Min(0.1f)] public float mouseSensitivity, baseMoveSpeed, jumpHeight, gravity, stepDistance;
    [Min(0f)] public float groundCheckStartHeight, groundCheckDistance, baseAirAcceleration, 
        bobUpAmplitude, bobSideAmplitude, landTime, landingShakePosAmp;
    [Min(1f)] public float sprintMultiplier;
    Transform cam;
    CharacterController cc;
    PlayerSoundsController soundController;
    Vector2 mouseDelta, inputDir, mousePos, mousePosLastFrame;
    Vector3 velocity, moveDir, camOrigPos;
    float xRot, yRot, moveSpeed, jumpSpeed, airAcceleration, cumulativeDistance, stepPhase, landTimer;
    public bool onGround;
    bool canJump, jumped, jumping, moving, landing, sprintPressed, sprinting;
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
        camOrigPos = cam.localPosition;
        cumulativeDistance = stepDistance / 2f;
    }

    void Update()
    {
        Forces();
        MouseDelta();
        Initial();
        Rotate();
        Directional();
        Step();
        LandingAnimation();
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
        //Move
        if (onGround)
        {
            sprinting = sprintPressed;
        }
        moveSpeed = baseMoveSpeed * (sprinting ? sprintMultiplier : 1f);
        
        //Jump
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
        airAcceleration = baseAirAcceleration * (sprinting ? sprintMultiplier/2f : 1f);

    }

    void MouseDelta()
    {
        mouseDelta = PauseGame.paused ? Vector2.zero : new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
   
    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputDir = ctx.ReadValue<Vector2>();
        moving = inputDir.magnitude > 0.1f;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        sprintPressed = ctx.performed;
    }
   
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && canJump) || PauseGame.paused) { return; }
        velocity.y = jumpSpeed;
        soundController.PlayStartJump();
        jumped = true;

        cumulativeDistance = 0f;
        stepPhase = stepDistance/2f;
        cam.localPosition = camOrigPos;
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
            var moveMagTime = move2.magnitude * Time.deltaTime;
            cumulativeDistance += moveMagTime;
            stepPhase += moveMagTime;
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
        if (!landing && onGround)
        {
            if (moving)
            {
                var camTargetPos = camOrigPos + Vector3.up * bobUpAmplitude * 
                    (Mathf.Abs(Mathf.Cos(Mathf.PI * cumulativeDistance / stepDistance)) - 1f)
                    + Vector3.right * bobSideAmplitude * Mathf.Cos(Mathf.PI * cumulativeDistance / stepDistance);
                cam.localPosition = Vector3.Lerp(cam.localPosition, camTargetPos, 0.1f);
            }
            else
            {
                stepPhase = stepDistance/2f;
                cumulativeDistance = 0f;
                //print("stopped moving");
                cam.localPosition = Vector3.Lerp(cam.localPosition, camOrigPos, 0.02f);
            }
        }
        
        //Step sound
        if (stepPhase >= stepDistance) 
        {
            stepPhase = 0f;
            soundController.PlayStep();
        }
    }

    void LandJump()
    {
        soundController.PlayLandJump();
        landing = true;
        landTimer = landTime;
    }

    void LandingAnimation()
    {
        if (!landing) { return; }
        landTimer -= Time.deltaTime;
        if (landTimer <= 0f)
        {
            landing = false;
            return;
        }
        cam.localPosition = camOrigPos + Vector3.down * landingShakePosAmp * Mathf.Sin(Mathf.PI * landTimer / landTime);
        
    }

    void MoveCC()
    {
        cc.Move(velocity * Time.deltaTime);    
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
