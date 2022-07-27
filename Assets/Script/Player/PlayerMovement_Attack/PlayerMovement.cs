using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("On Tilted Ground")]
    [SerializeField] float slideSpeedX = 0;
    [SerializeField] float slideSpeedZ = 0;
    bool notTiltedGround = false; // Check Tilted Ground.

    float airSpeedX; // Player Speed while Player jumping.
    float airSpeedZ; // Player Speed while Player jumping.
    
    [Space]
    [Header("Player Current State")]
    public bool waitingAnimation = false;
    public bool doJump = false;
    bool sitContinue = false;

    [Space]
    [Header("Cut out of ground layer")]
    public LayerMask playerLayer;

    [Space]
    [Header("Movement Speed Setting")]
    public float speed = 0.02f;
    [SerializeField] float dashspeed = 0.04f;
    public float slowspeed = 0.005f;

    float originalSpeed;

    [Space]
    [Header("Stop speed On air")]
    public float stopSpeed;

    [Space]
    [Header("Jump Setting")]
    [SerializeField] float jumpSpeed = 0.08f;
    [SerializeField] float smoothStopIntensity = 0.05f;
    [SerializeField] float levitateMinusTime = 0.06f;
    public float antiInertia = 8;
    float currenJumpSpeed = 0;
    public bool isJumping = false;

    [Space]
    [Header("Gravity Setting")]
    public float gravity = 0.03f;
    public float gravity_early;
    public float gravity_Max = 0.04f;
    public float gravitySpeed = 0.02f;

    CharacterController mybody;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 hitNormal = hit.normal;
        if (Physics.Raycast(transform.position, -transform.up, (mybody.height / (mybody.height * 2)), ~playerLayer))
        {
            if (Vector3.Angle(Vector3.up, hitNormal) <= mybody.slopeLimit)
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

    }

    void Start()
    {
        mybody = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!Physics.Raycast(transform.position, -transform.up, (mybody.height / (mybody.height * 2)), ~playerLayer))
            notTiltedGround = false;

        if (notTiltedGround == true)
        {
            slideSpeedX = 0;
            slideSpeedZ = 0;
        }

        if (notTiltedGround == false) // Set value about slideSpeed.
        {
            if (slideSpeedX == 0 && slideSpeedZ == 0 &&  isJumping == true)
            {
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.forward, (mybody.height / 2), ~playerLayer))
                {
                    slideSpeedZ = 5; // Current SlideSpeed Value = 5
                }
                else if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward, (mybody.height / 2), ~playerLayer))
                {
                    slideSpeedZ = -5;
                }

                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.right, (mybody.height / 2), ~playerLayer))
                {
                    slideSpeedX = 5;
                }
                else if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.right, (mybody.height / 2), ~playerLayer))
                {
                    slideSpeedX = -5;
                }
            }
        }

        // Default move value.

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");


        // Default move value while player jumping.

        if (airSpeedX > 0)
        {
            airSpeedX -= airSpeedX * Time.unscaledDeltaTime;
        }
        else if (airSpeedX < 0)
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

        if ((airSpeedX) > 0) 
        {
            airSpeedX += horizontalMove / 10 * smoothStopIntensity* Time.unscaledDeltaTime;
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
        
        ChMove(horizontalMove, verticalMove);
        ChJump(horizontalMove, verticalMove);
    }

    public void ChMove(float horizontalMove, float verticalMove) // Character Move
    {
        if (gravity_Max > gravity && isJumping == false)
        {
            gravity += gravitySpeed * Time.unscaledDeltaTime;
        }
        if (currenJumpSpeed > 0) currenJumpSpeed -= levitateMinusTime * Time.unscaledDeltaTime; else currenJumpSpeed = 0;

        ///////////////////////////////////////////////////////////////////////////
        
        if (Input.GetKeyDown(KeyCode.C)) // Player sit
        {
            if (sitContinue) { sitContinue = false; } else { sitContinue = true; }
        }

        // Player Dash.
        if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) || sitContinue == true) && isJumping == true) speed = slowspeed;
        else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isJumping == true) speed = dashspeed;
        else if (isJumping == true) speed = originalSpeed;

        float moveX = 0, moveZ = 0;

        if (airSpeedX != 0)
            moveX = (airSpeedX);
        else if (isJumping == true || speed != dashspeed) { moveX = (horizontalMove); }

        if (airSpeedZ != 0)
            moveZ = (airSpeedZ);
        else if (isJumping == true || speed != dashspeed) { moveZ = (verticalMove); }

        ////// Set player animator parameter. ///////////////////

        if (Player.instance && Player.instance.animationController)
        {
            if (speed > originalSpeed && CameraManager.fpsMode == false) Player.instance.animationController.animator.SetBool("Dash", true);
            else Player.instance.animationController.animator.SetBool("Dash", false);

            if (speed < originalSpeed) Player.instance.animationController.animator.SetBool("Crouch", true);
            else Player.instance.animationController.animator.SetBool("Crouch", false);

            Player.instance.animationController.AnimationWork(new Vector2(horizontalMove, verticalMove));
        }

        /////////////////// Movement when player sliding ///////////////////////////
        
        if (slideSpeedZ != 0 && isJumping == true && notTiltedGround == false)
        {
            moveZ /= 2;
            mybody.Move(transform.forward * slideSpeedZ * Time.deltaTime);
        }

        if (slideSpeedX != 0 && isJumping == true && notTiltedGround == false)
        {
            moveX /= 2;
            mybody.Move(transform.right * slideSpeedX * Time.deltaTime);
        }

        Vector3 myVelocity = Vector3.Normalize(transform.right * moveX) * speed;
        myVelocity += Vector3.Normalize(transform.forward * moveZ) * speed;

        mybody.Move(new Vector3(myVelocity.x, (-gravity + currenJumpSpeed), myVelocity.z) * 200 * Time.unscaledDeltaTime);

    }

    public void ChJump(float horizontalMove, float verticalMove) // Player Jump
    { 
        // Check Ground Slope;
        Debug.DrawRay(transform.position, -transform.up * (mybody.height / (mybody.height * 2)), Color.green);
        if (notTiltedGround == true || mybody.isGrounded == true)
        {
            if (isJumping == false) { currenJumpSpeed = 0; isJumping = true; gravity = gravity_early; airSpeedX = 0; airSpeedZ = 0; }
        }
        else 
        {
            isJumping = false; 
            
            if (Player.instance && Player.instance.animationController)
            {
                Player.instance.animationController.Air();
                if (doJump != true)
                    Player.instance.animationController.WaitngJump();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Start Jump
        {
            if (notTiltedGround == true)
            {
                waitingAnimation = true;
                doJump = true;
                
                if (Player.instance && Player.instance.animationController)
                {
                    Player.instance.animationController.Jump();
                }
                var velocity = new Vector3(horizontalMove * speed, jumpSpeed, verticalMove * speed);
                currenJumpSpeed = jumpSpeed;
                mybody.Move(velocity * Time.unscaledDeltaTime);

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
}
