using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float airSpeedX;
    float airSpeedZ;

    bool sitContinue = false;
    public bool waitingAnimation = false;
    public bool DoJump = false;

    public LayerMask playerLayer;

    [SerializeField] float speed = 3;
    [SerializeField] float Dashspeed = 10;
    [SerializeField] float Slowspeed = 2;

    float OriginalSpeed;
    public float stopSpeed;

    [SerializeField] float jumpSpeed = 0.08f;
    [SerializeField] float SmoothStopIntensity = 0.05f;
    public bool isJumping = false;

    [SerializeField] float levitateMinusTime = 0.06f;

    public float Anti_Inertia = 8;

    float currenJumpSpeed = 0;

    public float gravity = 0.03f;
    public float gravity_early;
    public float gravity_Max = 0.04f;
    public float gravitySpeed = 0.02f;

    CharacterController mybody;

    private void Awake()
    {
        gravity_early = gravity;
        OriginalSpeed = speed;
    }

    void Start()
    {
        mybody = GetComponent<CharacterController>();
    }

    void Update()
    {

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        if (airSpeedX > 0)
        {
            airSpeedX -= airSpeedX * Time.unscaledDeltaTime;
        }
        else if (airSpeedX < 0)
        {
            airSpeedX += airSpeedX * Time.unscaledDeltaTime;
        }
        else { airSpeedX = 0; }

        if (airSpeedZ > 0)
        {
            airSpeedZ -= airSpeedZ * Time.unscaledDeltaTime;
        }
        else if (airSpeedZ < 0)
        {
            airSpeedX += airSpeedX * Time.unscaledDeltaTime;
        }
        else { airSpeedZ = 0; }

        ///////////////////////////////////////////////////////////////

        if ((airSpeedX) > 0) 
        {
            airSpeedX += horizontalMove * SmoothStopIntensity* Time.unscaledDeltaTime;
            if (airSpeedX < 0) airSpeedX = 0;
        }
        else if ((airSpeedX) < 0)
        {
            airSpeedX += horizontalMove * SmoothStopIntensity * Time.unscaledDeltaTime;
            if (airSpeedX > 0) airSpeedX = 0;
        }

        if ((airSpeedZ) > 0)
        {
            airSpeedZ += verticalMove * SmoothStopIntensity * Time.unscaledDeltaTime; 
            if (airSpeedZ < 0) airSpeedZ = 0;
        }
        else if ((airSpeedZ) < 0)
        {
            airSpeedZ += verticalMove * SmoothStopIntensity * Time.unscaledDeltaTime; 
            if (airSpeedZ > 0) airSpeedZ = 0;
        }

        /////////////////////////////////////////////////////////////////
        
        ChMove(horizontalMove, verticalMove);
        ChJump(horizontalMove, verticalMove);
    }

    public void ChMove(float horizontalMove, float verticalMove)
    {
        if (gravity_Max > gravity && isJumping == false)
        {
            gravity += gravitySpeed * Time.unscaledDeltaTime;
        }
        if (currenJumpSpeed > 0) currenJumpSpeed -= levitateMinusTime * Time.unscaledDeltaTime; else currenJumpSpeed = 0;

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (sitContinue) { sitContinue = false; } else { sitContinue = true; }
        }
        if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) || sitContinue == true) && isJumping == true) speed = Slowspeed;
        else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isJumping == true) speed = Dashspeed;
        else if (isJumping == true) speed = OriginalSpeed;

        float moveX = 0, moveZ = 0;

        if (airSpeedX != 0)
            moveX = (airSpeedX);
        else if (isJumping == true || speed != Dashspeed) { moveX = (horizontalMove); }

        if (airSpeedZ != 0)
            moveZ = (airSpeedZ);
        else if (isJumping == true || speed != Dashspeed) {moveZ = (verticalMove); }
        ////////////////////////////////////
        ///

        if (speed > OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Dash", true); 
        else PlayerAnimControl.instance.Player.SetBool("Dash", false);

        if (speed < OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Crouch", true);
        else PlayerAnimControl.instance.Player.SetBool("Crouch", false);

        PlayerAnimControl.instance.AnimationWork(new Vector2(horizontalMove, verticalMove));

        Vector3 myVelocity = Vector3.Normalize(transform.right * moveX) * speed;
        myVelocity += Vector3.Normalize(transform.forward * moveZ) * speed;

        mybody.Move(new Vector3(myVelocity.x, (-gravity + currenJumpSpeed), myVelocity.z) * 200 * Time.unscaledDeltaTime); 

    }

    public void ChJump(float horizontalMove, float verticalMove)
    { 
        Debug.DrawRay(transform.position, -transform.up * (mybody.height / (mybody.height * 2)), Color.green);
        if (Physics.Raycast(transform.position, -transform.up, (mybody.height / (mybody.height * 2)), ~playerLayer) || mybody.isGrounded)
        {
            if (isJumping == false) { currenJumpSpeed = 0; isJumping = true; gravity = gravity_early; airSpeedX = 0; airSpeedZ = 0; }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                waitingAnimation = true;
                DoJump = true;
                PlayerAnimControl.instance.Jump();
                var velocity = new Vector3(horizontalMove * speed, jumpSpeed , verticalMove * speed);
                currenJumpSpeed = jumpSpeed;
                mybody.Move(velocity * Time.unscaledDeltaTime);

                if (speed == Dashspeed)
                {
                    airSpeedX = horizontalMove * speed / Anti_Inertia;
                    airSpeedZ = verticalMove * speed / Anti_Inertia;

                }
                else
                {
                    airSpeedX = 0;
                    airSpeedZ = 0;

                }
            }
        }
        else
        {
            isJumping = false;
            PlayerAnimControl.instance.air();
            if(DoJump != true)
            PlayerAnimControl.instance.waitngJump();

        }
    }
    }
