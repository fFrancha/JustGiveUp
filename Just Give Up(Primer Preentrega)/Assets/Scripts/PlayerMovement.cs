using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Kill y Respawn
    Vector3 posInicial;

    //Wallrun
    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallspeed;
    bool isWallRight, isWallLeft;
    bool isWallRunning;
    public float maxWallRunPeraTilt, wallRunCameraTilt;
    

    [Header("Movement")]
    private float moveSpeed;
    public float floorDrag;
    public float walkSpeed;
    public float sprintSpeed;

    //salto

    public float jumpForce;
    public float jumpCD;
    public float airMultiplier;
    bool readyToJump;

    //agacharse

    [Header("Crounching")]
    public float crounchSpeed;
    public float crounchYScale;
    private float startYScale;
        
    //es piso¿?

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsFloor;
    bool isFloor;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    //movimiento dirigido
    
    public Transform orientation;
    float horizontal;
    float vertical;

    Vector3 moveDirection;
    Vector3 velocityToSet;
    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        
        walking,
        sprinting,
        air,
        crouching,
    }

 
  
   

    void Start()
    {

        posInicial = transform.position;
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }


    private void Update() 
    {   
        WallRunInput();
        CheckForWall();
        SpeedControl();
        MovementInput();
        StateKeeper();
        isFloor = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsFloor);

        if(isFloor)
            {
                rb.drag = floorDrag;
            }
            else 
            {
                rb.drag = 0;
            }
    }


    private void FixedUpdate()
    {
        PlayerMove();
    }


    void MovementInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && isFloor)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCD);
        }

        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, crounchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3 (transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    void WallRunInput()
    {
        if(Input.GetKey(KeyCode.D) && isWallRight)
        {
            StartWallrun();
        }

        if(Input.GetKey(KeyCode.A) && isWallLeft)
        {
            StartWallrun();
        }
    }

    void StartWallrun()
    {
        rb.useGravity = false;
        isWallRunning = true;
        
        
        if(rb.velocity.magnitude <= maxWallspeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime);

            if(isWallRight)
            {
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            }
            else
            { 
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
            }
        }

    }

    void StopWallrun()
    {
        
        rb.useGravity = true;
        isWallRunning = false;
    }

    void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        if(!isWallLeft && !isWallRight)
        {
            StopWallrun();
        }
        
    }

    void PlayerMove()
    {
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        if(isFloor)
        {
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        
        else if (!isFloor)
        {
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
    {
        Vector3 velLimit = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(velLimit.x, rb.velocity.y, velLimit.z);
    }
        
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

 
    void ResetJump()
    {
        readyToJump = true;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        
        
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

    }

    void SetVelocity()
    {
        rb.velocity = velocityToSet;
    }


 
    void StateKeeper()
    {
        if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crounchSpeed;
        }

        if(isFloor && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        else if(isFloor)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.gameObject.tag == "Spikes")
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = posInicial;
    }
   
}