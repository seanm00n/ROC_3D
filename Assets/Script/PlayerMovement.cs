using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    public bool isJumping = false;

    [SerializeField] float levitateMinusTime = 0.06f;

    float airSpeed_x = 0;
    float airSpeed_z = 0;

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
        if (airSpeed_x > 0) airSpeed_x -= Time.deltaTime; else if (airSpeed_x < 0) airSpeed_x += Time.deltaTime; 

        if (airSpeed_z > 0) airSpeed_z -= Time.deltaTime; else if (airSpeed_z < 0) airSpeed_z += Time.deltaTime;

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        ChMove(horizontalMove, verticalMove);
        ChJump(horizontalMove, verticalMove);
    }

    public void ChMove(float horizontalMove, float verticalMove)
    {
        if (gravity_Max > gravity && isJumping == false)
        {
            gravity += gravitySpeed * Time.deltaTime;
        }
        if (currenJumpSpeed > 0) currenJumpSpeed -= levitateMinusTime * Time.deltaTime; else currenJumpSpeed = 0;

        if (Input.GetKeyDown(KeyCode.C)) 
        {
            if (sitContinue) { sitContinue = false; } else { sitContinue = true; } 
        } 
        if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) || sitContinue == true) && isJumping == true) speed = Slowspeed;
        else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isJumping == true) speed = Dashspeed;
        else if(isJumping == true)speed = OriginalSpeed;

        float moveX, moveZ;

        if (airSpeed_x > 0)

            moveX = airSpeed_x - (horizontalMove * stopSpeed) * Time.deltaTime;
        
        else if (airSpeed_x < 0)

            moveX = airSpeed_x + (horizontalMove * stopSpeed) * Time.deltaTime;

        else
            moveX = (horizontalMove);
        
        ////////////////////////////////////
        ///

        if (airSpeed_z > 0)

            moveZ = airSpeed_z - (verticalMove * stopSpeed) * Time.deltaTime;

        else if (airSpeed_z < 0)

            moveZ = airSpeed_z + (verticalMove * stopSpeed) * Time.deltaTime;

        else
            moveZ = (verticalMove);



        if (speed > OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Dash", true); 
        else PlayerAnimControl.instance.Player.SetBool("Dash", false);

        if (speed < OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Crouch", true);
        else PlayerAnimControl.instance.Player.SetBool("Crouch", false);

        PlayerAnimControl.instance.AnimationWork(new Vector2(horizontalMove, verticalMove));

        Vector3 myVelocity = Vector3.Normalize(transform.right * moveX) * speed * Time.deltaTime;
        myVelocity += Vector3.Normalize(transform.forward * moveZ) * speed * Time.deltaTime;

        mybody.Move(new Vector3(myVelocity.x, (-gravity + currenJumpSpeed), myVelocity.z) * 200 * Time.deltaTime); 

    }

    public void ChJump(float horizontalMove, float verticalMove)
    { 
        Debug.DrawRay(transform.position, -transform.up * (mybody.height / (mybody.height * 2)), Color.green);
        if (Physics.Raycast(transform.position, -transform.up, (mybody.height / (mybody.height * 2)), ~playerLayer) || mybody.isGrounded)
        {
            if (isJumping == false) { airSpeed_x = 0; airSpeed_z = 0; currenJumpSpeed = 0; isJumping = true; gravity = gravity_early; }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                waitingAnimation = true;
                DoJump = true;
                PlayerAnimControl.instance.Jump();
                var velocity = new Vector3(horizontalMove * speed, jumpSpeed , verticalMove * speed);
                currenJumpSpeed = jumpSpeed;
                mybody.Move(velocity * Time.deltaTime);

                if (speed == Dashspeed)
                {
                    airSpeed_x = (int)mybody.velocity.x / Anti_Inertia;
                    airSpeed_z = (int)mybody.velocity.z / Anti_Inertia;
                }
                else
                {
                    airSpeed_x = 0;
                    airSpeed_z = 0;

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
