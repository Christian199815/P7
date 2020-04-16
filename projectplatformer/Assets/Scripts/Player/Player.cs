using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{

    private bool CollUp;
    private bool CollRight;
    //groundcheck
    public bool CollDown;
    private bool CollLeft;

    public int maxJumps;
    private int jumpsDone;

    public Vector2 velocity;
    private float dashVelocity;

    public float dashSpeed;
    public float dashFriction;

    public float maxMovementSpeed;
    public float accelerationSpeed;
    public float groundDecelerationSpeed;
    public float airDecelerationSpeed;

    public float terminalVelocity;
    public float gravityScale;
    public float jumpForce;
    public float wallJumpForce;

    public Vector2 inputAxis = Vector2.zero;
    public Vector4 buttonAxis = Vector4.zero;

    private bool singleInputJump;
    private bool singleInputDash;

    public GameObject Character;

    private InputManager iMan;

    public bool movementEnabled = true;

    private bool pause = false;

    //for Grappling
    public bool isSwinging;
    private Rigidbody2D rb;
    private bool isJumping;
    public Vector2 ropeHook;
    public float swingForce = 4f;

    

    void Start()
    {
        pause = true;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        iMan = FindObjectOfType<InputManager>();
        pause = false;
    }
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(iMan != null)
        {
            inputAxis = iMan.axis;
            buttonAxis = iMan.buttonAxis;


            Movement();

            
            if (buttonAxis.x == 1 && iMan.axis.y == 0 && singleInputJump && jumpsDone < maxJumps && movementEnabled)
            {
                singleInputJump = false;
                Jump();
            }
            if (buttonAxis.x == 0) singleInputJump = true;


            if (buttonAxis.y == 1 && singleInputDash && movementEnabled && dashVelocity == 0)
            {
                singleInputDash = false;
                Dash();
            }
            if (buttonAxis.y != 1) singleInputDash = true;

          

            transform.Translate(new Vector2(velocity.x + dashVelocity, velocity.y) * Time.deltaTime);
            RayCast();
            if (CollDown) jumpsDone = 0;

            //PlayerTurn();
        }

    }

    private void PlayerTurn()
    {
        if(iMan.axis.x > 0)
        {
            Character.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if(iMan.axis.x < 0)
        {
            Character.transform.rotation = new Quaternion(0, 45, 0, 0);
        } 
    }

    private void Dash()
    {
        if (velocity.x > 0) dashVelocity = dashSpeed;
        if (velocity.x < 0) dashVelocity = -dashSpeed;
        
        if (velocity.x != 0)
            StartCoroutine(DashController());
    }

    private IEnumerator DashController()
    {
        while(dashVelocity != 0)
        {
            yield return null;
            if (dashVelocity < 0) dashVelocity += dashFriction;
            if (dashVelocity > 0) dashVelocity -= dashFriction;
            if (dashVelocity >= -dashFriction / 2 && dashVelocity <= dashFriction / 2)
            {
                dashVelocity = 0;
            }
        }

    }

    private void Jump()
    {
        if (velocity.y <= 0) velocity.y = jumpForce;
        else velocity.y += jumpForce;
        if (jumpsDone > 0)
        {
            if (CollLeft)
            {
                velocity.x = wallJumpForce;
                return;
            }
            if (CollRight)
            {
                velocity.x = -wallJumpForce;
                return;
            }
        }

        jumpsDone += 1;
    }


    private void Movement()
    {
        if (!movementEnabled)
        {
            inputAxis = Vector2.zero;
        }
        if ((velocity.x > 0 && CollRight) || velocity.x < 0 && CollLeft)
        {
            velocity.x = 0;
        }
        if (!CollDown && velocity.y >= -terminalVelocity + gravityScale * Time.deltaTime)
        {
            velocity.y -= gravityScale * Time.deltaTime;
        }

        if (CollDown)
        {
            if (inputAxis.x == 0 && velocity.x != 0)
            {
                if (velocity.x < groundDecelerationSpeed * Time.deltaTime / 2 && velocity.x > -groundDecelerationSpeed * Time.deltaTime / 2) velocity.x = 0;
                if (velocity.x >= groundDecelerationSpeed * Time.deltaTime / 2) velocity.x -= groundDecelerationSpeed * Time.deltaTime;
                if (velocity.x <= -groundDecelerationSpeed * Time.deltaTime / 2) velocity.x += groundDecelerationSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (inputAxis.x == 0 && velocity.x != 0)
            {
                if (velocity.x < airDecelerationSpeed * Time.deltaTime / 2 && velocity.x > -airDecelerationSpeed * Time.deltaTime / 2) velocity.x = 0;
                if (velocity.x >= airDecelerationSpeed * Time.deltaTime / 2) velocity.x -= airDecelerationSpeed * Time.deltaTime;
                if (velocity.x <= -airDecelerationSpeed * Time.deltaTime / 2) velocity.x += airDecelerationSpeed * Time.deltaTime;
            }
        }

        if (inputAxis.x > 0)
        {
            if (velocity.x <= -groundDecelerationSpeed * Time.deltaTime) velocity.x += groundDecelerationSpeed * Time.deltaTime;
            else if (velocity.x <= maxMovementSpeed - accelerationSpeed * Time.deltaTime) velocity.x += accelerationSpeed * Time.deltaTime;
        }
        if (inputAxis.x < 0)
        {
            if (velocity.x >= groundDecelerationSpeed * Time.deltaTime) velocity.x -= groundDecelerationSpeed * Time.deltaTime;
            else if (velocity.x >= -maxMovementSpeed + accelerationSpeed * Time.deltaTime) velocity.x -= accelerationSpeed * Time.deltaTime;
        }

        if (!movementEnabled)
        {
            inputAxis = iMan.axis;
        }
    }

    void FixedUpdate()
    {
        //if (iMan != null) Grappling();
    }

    public void Grappling()
    {
        if(iMan.axis.x < 0f || iMan.axis.x > 0f)
        {
            if (isSwinging)
            {
                var playertoHookDirection = (ropeHook - (Vector2)transform.position).normalized;
                Vector2 perpendicularDirection;
                if(iMan.axis.x < 0)
                {
                    perpendicularDirection = new Vector2(-playertoHookDirection.y, playertoHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playertoHookDirection.y, -playertoHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Force);
            }
            else
            {
                if (!CollDown)
                {
                    var groundforce = maxMovementSpeed * 2f;
                    rb.AddForce(new Vector2((iMan.axis.x * groundforce - rb.velocity.x) * groundforce, 0));
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }
            }
        }

        if (!isSwinging)
        {
            if (!CollDown) return;

            isJumping = iMan.axis.y > 0f;
            if (isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }


    #region Raycasts
    private void RayCast()
    {
        RayCastRight();
        RayCastLeft();
        RayCastUp();
        RayCastDown();

        if (velocity.y < 0 && CollDown) velocity.y = 0;
        if (velocity.y > 0 && CollUp) velocity.y = 0;
    }

    private void RayCastRight()
    {
        Debug.DrawLine(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y + (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x + transform.lossyScale.x / 2, transform.position.y + (transform.lossyScale.y / 3), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y, transform.position.z), new Vector3(transform.position.x + transform.lossyScale.x / 2, transform.position.y, transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y - (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x + transform.lossyScale.x / 2, transform.position.y - (transform.lossyScale.y / 3), transform.position.z));

        RaycastHit2D[] hitRU = Physics2D.RaycastAll(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y + (transform.lossyScale.y / 3)), Vector3.right, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitR = Physics2D.RaycastAll(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y), Vector3.right, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitRL = Physics2D.RaycastAll(new Vector3(transform.position.x + (transform.lossyScale.x / 4), transform.position.y - (transform.lossyScale.y / 3)), Vector3.right, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));

        List<RaycastHit2D> hitRight = new List<RaycastHit2D>();
        foreach (RaycastHit2D h in hitRU) hitRight.Add(h);
        foreach (RaycastHit2D h in hitR) hitRight.Add(h);
        foreach (RaycastHit2D h in hitRL) hitRight.Add(h);

        CollRight = false;
        foreach (RaycastHit2D h in hitRight)
        {
            if (h.collider.gameObject == gameObject) continue;
            CollRight = true;
            if (velocity.x > 0) transform.position = new Vector2(h.point.x - transform.lossyScale.x / 2, transform.position.y);
        }


    }

    private void RayCastLeft()
    {
        Debug.DrawLine(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y + (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x - transform.lossyScale.x / 2, transform.position.y + (transform.lossyScale.y / 3), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y, transform.position.z), new Vector3(transform.position.x - transform.lossyScale.x / 2, transform.position.y, transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y - (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x - transform.lossyScale.x / 2, transform.position.y - (transform.lossyScale.y / 3), transform.position.z));

        RaycastHit2D[] hitLU = Physics2D.RaycastAll(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y + (transform.lossyScale.y / 3)), Vector3.left, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitL = Physics2D.RaycastAll(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y), Vector3.left, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitLL = Physics2D.RaycastAll(new Vector3(transform.position.x - (transform.lossyScale.x / 4), transform.position.y - (transform.lossyScale.y / 3)), Vector3.left, transform.lossyScale.x / 4, ~LayerMask.GetMask("Player"));

        List<RaycastHit2D> hitLeft = new List<RaycastHit2D>();
        foreach (RaycastHit2D h in hitLU) hitLeft.Add(h);
        foreach (RaycastHit2D h in hitL) hitLeft.Add(h);
        foreach (RaycastHit2D h in hitLL) hitLeft.Add(h);

        CollLeft = false;
        foreach (RaycastHit2D h in hitLeft)
        {
            if (h.collider.gameObject == gameObject) continue;
            else CollLeft = true;
            if (velocity.x < 0) transform.position = new Vector2(h.point.x + transform.lossyScale.x / 2, transform.position.y);
        }
    }

    private void RayCastUp()
    {
        Debug.DrawLine(new Vector3(transform.position.x - (transform.lossyScale.x / 3), transform.position.y + (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x - transform.lossyScale.x / 3, transform.position.y + (transform.lossyScale.y / 3) + (transform.lossyScale.y / 6), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x, transform.position.y + (transform.lossyScale.y / 3) + (transform.lossyScale.y / 6), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x + (transform.lossyScale.x / 3), transform.position.y + (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x + transform.lossyScale.x / 3, transform.position.y + (transform.lossyScale.y / 3) + (transform.lossyScale.y / 6), transform.position.z));

        RaycastHit2D[] hitUL = Physics2D.RaycastAll(new Vector3(transform.position.x - (transform.lossyScale.x / 3), transform.position.y + (transform.lossyScale.y / 3)), Vector3.up, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitU = Physics2D.RaycastAll(new Vector3(transform.position.x, transform.position.y + (transform.lossyScale.y / 3)), Vector3.up, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitUR = Physics2D.RaycastAll(new Vector3(transform.position.x + (transform.lossyScale.x / 3), transform.position.y + (transform.lossyScale.y / 3)), Vector3.up, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));

        List<RaycastHit2D> hitUp = new List<RaycastHit2D>();
        foreach (RaycastHit2D h in hitUL) hitUp.Add(h);
        foreach (RaycastHit2D h in hitU) hitUp.Add(h);
        foreach (RaycastHit2D h in hitUR) hitUp.Add(h);

        CollUp = false;
        foreach (RaycastHit2D h in hitUp)
        {
            if (h.collider.gameObject == gameObject) continue;
            else CollUp = true;
            if (velocity.y > 0) transform.position = new Vector2(transform.position.x, h.point.y - transform.lossyScale.y / 2);
        }
    }

    private void RayCastDown()
    {
        Debug.DrawLine(new Vector3(transform.position.x - (transform.lossyScale.x / 3), transform.position.y - (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x - transform.lossyScale.x / 3, transform.position.y - (transform.lossyScale.y / 3) - (transform.lossyScale.y / 6), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 3) - (transform.lossyScale.y / 6), transform.position.z));
        Debug.DrawLine(new Vector3(transform.position.x + (transform.lossyScale.x / 3), transform.position.y - (transform.lossyScale.y / 3), transform.position.z), new Vector3(transform.position.x + transform.lossyScale.x / 3, transform.position.y - (transform.lossyScale.y / 3) - (transform.lossyScale.y / 6), transform.position.z));

        RaycastHit2D[] hitDL = Physics2D.RaycastAll(new Vector3(transform.position.x - (transform.lossyScale.x / 3), transform.position.y - (transform.lossyScale.y / 3)), Vector3.down, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitD = Physics2D.RaycastAll(new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 3)), Vector3.down, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));
        RaycastHit2D[] hitDR = Physics2D.RaycastAll(new Vector3(transform.position.x + (transform.lossyScale.x / 3), transform.position.y - (transform.lossyScale.y / 3)), Vector3.down, transform.lossyScale.y / 6, ~LayerMask.GetMask("Player"));

        List<RaycastHit2D> hitDown = new List<RaycastHit2D>();
        foreach (RaycastHit2D h in hitDL) hitDown.Add(h);
        foreach (RaycastHit2D h in hitD) hitDown.Add(h);
        foreach (RaycastHit2D h in hitDR) hitDown.Add(h);

        CollDown = false;
        foreach (RaycastHit2D h in hitDown)
        {
            if (h.collider.gameObject == gameObject) continue;
            else CollDown = true;
            if (velocity.y < 0) transform.position = new Vector2(transform.position.x, h.point.y + transform.lossyScale.y / 2);
        }
    }
    #endregion
}
