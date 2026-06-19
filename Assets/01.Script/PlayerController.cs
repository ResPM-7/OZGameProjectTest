using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f; // 캐릭터가 도는 속도
    [SerializeField] private float jumpForce = 10f; // 점프 힘
    [SerializeField] private float delay = 0.4f; // 점프하고 바닥착지할때 잠시 멈추게하기

    [SerializeField] private Transform cameraTransform; // 메인 카메라 연결 필수

    private Animator anim;
    private Rigidbody rb;

    private Vector2 moveInput;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isLanding = false; //착지할때 잠시 못움직이게
    private float currentSpeed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        Move();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && !isJumping && !isLanding)
        {
            Jump();
        }
    }

    public void OnSprint(InputValue value)
    {
        isRunning = value.isPressed;
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    private void Move()
    {
        if(isLanding)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        bool isMove = moveInput.magnitude != 0;

        if (isMove)
        {
            if(!isJumping)
                anim.SetFloat("Speed", currentSpeed);

            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;

            Vector3 moveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

            Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized);//플레이어가 볼 위치
            transform.rotation = Quaternion.Lerp(transform.rotation, viewRot, Time.deltaTime * rotationSpeed);//플레이어 회전할때 기계적으로 움직이지 않고 부드럽게

            transform.position += moveDir * currentSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    private void Jump()
    {
        isJumping = true;

        anim.SetBool("Jump", true);
        anim.SetBool("IsGrounded", false);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && isJumping)
        {
            isLanding = true;
            anim.SetBool("IsGrounded", true);
            Invoke("JumpingDelay", delay);
        }
    }


    void JumpingDelay()
    {
        isJumping = false;
        isLanding = false;
        anim.SetBool("Jump", false);
    }
}