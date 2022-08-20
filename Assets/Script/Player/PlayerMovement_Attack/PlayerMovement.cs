using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("On Tilted Ground")]
    [SerializeField] private float slideSpeedX = 0;
    [SerializeField] private float slideSpeedZ = 0;
    private bool notTiltedGround; // Check Tilted Ground.

    public float airSpeedX; // Player Speed while Player jumping.
    public float airSpeedZ; // Player Speed while Player jumping.
    
    [Space]
    [Header("Player Current State")]
    public bool waitingAnimation;
    public bool doJump;
    private bool sitContinue;

    [Space]
    [Header("Cut out of ground layer")]
    public LayerMask playerLayer;

    [Space]
    [Header("Movement Speed Setting")]
    public float speed = 0.02f;
    [SerializeField] float dashspeed = 0.04f;
    public float slowspeed = 0.01f;

    private float originalSpeed;

    [Space]
    [Header("Stop speed On air")]
    public float stopSpeed;

    [Space]
    [Header("Jump Setting")]
    [SerializeField] private float jumpSpeed = 0.08f;
    [SerializeField] private float smoothStopIntensity = 0.05f;
    [SerializeField] private float levitateMinusTime = 0.06f;
    public float antiInertia = 8;
    [HideInInspector]public float currJumpSpeed;
    public bool isJumping;

    [Space]
    [Header("Gravity Setting")]
    public float gravity = 0.03f;
    public float gravity_early;
    public float gravity_Max = 0.04f;
    public float gravitySpeed = 0.02f;

    [Space]
    [Header("Spin Setting")]
    public float spinSpeed = 0.02f;
    public float dashSpinSpeed = 0;
    private float originalSpinSpeed = 0;
    public float spinTime = 0f;
    public bool isSpin = false;

    private CharacterController playerBody;
    private static readonly int DashID = Animator.StringToHash("Dash");
    private static readonly int CrouchID = Animator.StringToHash("Crouch");

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 hitNormal = hit.normal;
        if (Physics.Raycast(transform.position, -transform.up, (playerBody.height / (playerBody.height * 2)), ~playerLayer))
        {
            if (Vector3.Angle(Vector3.up, hitNormal) <= playerBody.slopeLimit) // Use collider to check grond slope.
            {
                slideSpeedX = 0;
                slideSpeedZ = 0;
                notTiltedGround = true;
            }
        }
        
    }

    private void Awake()
    {
        gravity_early = gravity;
        originalSpeed = speed;
        originalSpinSpeed = spinSpeed;
    }

    private void Start()
    {
        playerBody = GetComponent<CharacterController>();
    }

    public void Update()
    {
        if (!Physics.Raycast(transform.position, -transform.up, (playerBody.height / (playerBody.height * 2)), ~playerLayer)) // Player is float on air.
            notTiltedGround = false;

        if (notTiltedGround)
        {
            slideSpeedX = 0;
            slideSpeedZ = 0;
        }
        else // Set value about slideSpeed.
        {
            if (slideSpeedX == 0 && slideSpeedZ == 0 && isJumping)
            {
                var playerHalfHeight = playerBody.height / 2;
                
                if (Physics.Raycast(transform.position, -transform.forward, playerHalfHeight, ~playerLayer))
                {
                    slideSpeedZ = 5; // Current SlideSpeed Value = 5
                }
                else if (Physics.Raycast(transform.position, transform.forward, playerHalfHeight, ~playerLayer))
                {
                    slideSpeedZ = -5;
                }

                if (Physics.Raycast(transform.position, -transform.right, playerHalfHeight, ~playerLayer))
                {
                    slideSpeedX = 5;
                }
                else if (Physics.Raycast(transform.position, transform.right, playerHalfHeight, ~playerLayer))
                {
                    slideSpeedX = -5;
                }
            }
        }

        // Default move value.

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        // Default move value while player jumping.

        if (airSpeedX is > 0 or < 0)
        {
            airSpeedX -= airSpeedX * Time.unscaledDeltaTime;
        }
        else { airSpeedX = 0; }

        if (airSpeedZ > 0)
        {
            airSpeedZ -= airSpeedZ * Time.unscaledDeltaTime;
        }
        else if (airSpeedZ < 0)
        {
            airSpeedZ += airSpeedZ * Time.unscaledDeltaTime;
        }
        else { airSpeedZ = 0; }

        ///////////////////////////////////////////////////////////////////// Stop speed part
        
        if ((airSpeedX) > 0)
        {
            airSpeedX += horizontalMove / 10 * smoothStopIntensity * Time.unscaledDeltaTime;
            if (airSpeedX < 0) airSpeedX = 0;
        }
        else if ((airSpeedX) < 0)
        {
            airSpeedX += horizontalMove / 10 * smoothStopIntensity * Time.unscaledDeltaTime;
            if (airSpeedX > 0) airSpeedX = 0;
        }

        if ((airSpeedZ) > 0)
        {
            airSpeedZ += verticalMove / 10 * smoothStopIntensity * Time.unscaledDeltaTime;
            if (airSpeedZ < 0) airSpeedZ = 0;
        }
        else if ((airSpeedZ) < 0)
        {
            airSpeedZ += verticalMove / 10 * smoothStopIntensity * Time.unscaledDeltaTime;
            if (airSpeedZ > 0) airSpeedZ = 0;
        }
        
        /////////////////////////////////////////////////////////////////

        MovePlayer(horizontalMove, verticalMove);
        JumpPlayer(horizontalMove, verticalMove);
    }

    public void MovePlayer(float horizontalMove, float verticalMove) // Character Move
    {
        if (spinTime == 0 && spinSpeed != originalSpinSpeed && !isSpin)
        {
            if(isJumping)
            spinSpeed = originalSpinSpeed;
        }
        if (gravity_Max > gravity && !isJumping)
            gravity += gravitySpeed * Time.unscaledDeltaTime;
        
        if (currJumpSpeed > 0)
            currJumpSpeed -= levitateMinusTime * Time.unscaledDeltaTime;
        
        else currJumpSpeed = 0;

        ///////////////////////////////////////////////////////////////////////////
        
        if (Input.GetKeyDown(KeyCode.C)) // Player sit
        {
            sitContinue = !sitContinue;
        }


        if (Input.GetKeyDown(KeyCode.Tab)) // Start Jumping
        {
            Player.instance.animationController.OnSpin();
        }

        // Player Dash.
        if (isJumping)
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) || sitContinue)
                speed = slowspeed;
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = dashspeed;
                if(!isSpin)
                spinSpeed = dashSpinSpeed;
            }
            else
                speed = originalSpeed;
        }

        float moveX = 0, moveZ = 0;

        if (airSpeedX != 0)
            moveX = airSpeedX;
        else if (isJumping || speed != dashspeed)
            moveX = horizontalMove;

        if (airSpeedZ != 0)
            moveZ = airSpeedZ;
        else if (isJumping || speed != dashspeed)
            moveZ = verticalMove;

        ////// Set player animator parameter. ///////////////////
        #region
        if (Player.instance && Player.instance.animationController)
        {
            Player.instance.animationController.animator.SetBool(DashID, speed > originalSpeed && !CameraManager.fpsMode);
            Player.instance.animationController.animator.SetBool(CrouchID, speed < originalSpeed);
            Player.instance.animationController.AnimationWork(new Vector2(horizontalMove, verticalMove));
        }
        #endregion

        /////////////////// Movement when player sliding ///////////////////////////
        #region
        if (slideSpeedZ != 0 && isJumping && !notTiltedGround)
        {
            moveZ /= 2;
            playerBody.Move(transform.forward * (slideSpeedZ * Time.deltaTime));
        }

        if (slideSpeedX != 0 && isJumping && !notTiltedGround)
        {
            moveX /= 2;
            playerBody.Move(transform.right * (slideSpeedX * Time.deltaTime));
        }

        var myVelocity = Vector3.Normalize(transform.right * moveX) * speed;

        if (spinTime != 0)
        {
            myVelocity = new Vector3(0,0,0);
        }

        if (spinTime > 0)
        {
            myVelocity += Vector3.Normalize(transform.forward) * spinSpeed;
        }
        else
            myVelocity += Vector3.Normalize(transform.forward * moveZ) * speed;

        playerBody.Move(new Vector3(myVelocity.x, (-gravity + currJumpSpeed), myVelocity.z) * (200 * Time.unscaledDeltaTime));
        #endregion
    }

    public void JumpPlayer(float horizontalMove, float verticalMove) // Player Jump
    {
        if (spinTime != 0) return;
        // Check Ground Slope;
        var playerHeight = playerBody.height;
        Debug.DrawRay(transform.position, -transform.up * (playerHeight / (playerHeight * 2)), Color.green);
        
        if (notTiltedGround || playerBody.isGrounded) // Make Player can Jump 
        {
            if (!isJumping)
            {
                isJumping = true;
                gravity = gravity_early;
                currJumpSpeed = airSpeedX = airSpeedZ = 0;
            }
        }
        else // Make Player can't Jump
        {
            isJumping = false; 
            
            if (Player.instance && Player.instance.animationController)
            {
                Player.instance.animationController.Air();
                
                if (doJump != true)
                    Player.instance.animationController.waitngJump();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Start Jumping
        {
            if (!notTiltedGround) return;
            
            waitingAnimation = true;
            doJump = true;
                
            if (Player.instance && Player.instance.animationController)
            {
                Player.instance.animationController.Jump(); // Play jump animation.
            }
            var velocity = new Vector3(horizontalMove * speed, jumpSpeed, verticalMove * speed); // Movement when player Jump.
            currJumpSpeed = jumpSpeed;
            playerBody.Move(velocity * Time.unscaledDeltaTime);

            // Make fast turn is impossible.
            if (speed == dashspeed) 
            {
                airSpeedX = horizontalMove * speed / antiInertia;
                    
                airSpeedZ = verticalMove * speed / antiInertia;
            }
            else
            {
                airSpeedX = 0;
                airSpeedZ = 0;
            }
        }
    }
}
