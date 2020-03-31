using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public KeyCode walkLeftKey;
    public KeyCode walkRightKey;
    public KeyCode jumpKey;

    private bool CollUp;
    private bool CollRight;
    public bool CollDown;
    private bool CollLeft;

    public int maxJumps;
    private int jumpsDone;

    public Vector2 velocity;

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

    private bool singleInput;

    private InputManager iMan;

    public bool movementEnabled = true;

    private bool pause = false;

    void Start()
    {
        pause = true;
        iMan = FindObjectOfType<InputManager>();
    }
    public bool isSwinging;
    public Vector2 ropeHook;
    public float swingForce = 4f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (pause)
        {
            if (iMan.axis != Vector2.zero)
            {
                pause = false;
            }
            return;
        }
        inputAxis = iMan.axis;
        buttonAxis = iMan.buttonAxis;


        Movement();

        if (buttonAxis.x == 1 && singleInput && jumpsDone < maxJumps && movementEnabled)
        {
            singleInput = false;
            Jump();
        }
        if (buttonAxis.x != 1) singleInput = true;


        transform.Translate(velocity * Time.deltaTime);
        RayCast();
        if (CollDown) jumpsDone = 0;
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

    }

    public void Grappling()
    {
        if(inputAxis.x < 0f || inputAxis.x > 0f)
        {
            if (isSwinging)
            {
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;
                Vector2 perpendicularDirection;
                if (inputAxis.x < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Force);
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
