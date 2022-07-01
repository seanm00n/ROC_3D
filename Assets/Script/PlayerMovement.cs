using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool waitingAnimation = false;
    public bool DoJump = false;

    public LayerMask playerLayer;

    [SerializeField] float speed = 3;
    [SerializeField] float Dashspeed = 10;
    [SerializeField] float Slowspeed = 2;

    float OriginalSpeed;

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
        if (airSpeed_x > 0) airSpeed_x -= Time.deltaTime; else if (airSpeed_x < 0) airSpeed_x -= Time.deltaTime; 
        else airSpeed_x = 0;

        if (airSpeed_z > 0) airSpeed_z -= Time.deltaTime; else if (airSpeed_z < 0) airSpeed_z -= Time.deltaTime;
        else airSpeed_z = 0;

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

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && isJumping == true) speed = Slowspeed;
        else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isJumping == true) speed = Dashspeed;
        else if(isJumping == true)speed = OriginalSpeed;

        float moveX = (horizontalMove + airSpeed_x / Anti_Inertia);
        float moveZ = (verticalMove + airSpeed_z / Anti_Inertia);

        if (speed > OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Dash", true); 
        else PlayerAnimControl.instance.Player.SetBool("Dash", false);

        if (speed < OriginalSpeed) PlayerAnimControl.instance.Player.SetBool("Crouch", true);
        else PlayerAnimControl.instance.Player.SetBool("Crouch", false);

        PlayerAnimControl.instance.AnimationWork(new Vector2(horizontalMove, verticalMove));

        Vector3 myVelocity = Vector3.Normalize(transform.right * moveX) * speed * Time.deltaTime;
        myVelocity += Vector3.Normalize(transform.forward * moveZ) * speed * Time.deltaTime;

        mybody.Move(new Vector3(myVelocity.x, (-gravity + currenJumpSpeed), myVelocity.z)); 

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
                var velocity = new Vector3(horizontalMove * speed * Time.deltaTime, jumpSpeed, verticalMove * speed * Time.deltaTime);
                currenJumpSpeed = jumpSpeed;
                mybody.Move(velocity);
                
                airSpeed_x = (int)mybody.velocity.x;
                airSpeed_z = (int)mybody.velocity.z;
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
