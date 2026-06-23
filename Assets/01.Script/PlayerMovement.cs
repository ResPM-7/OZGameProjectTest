using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f; // 캐릭터가 도는 속도
    [SerializeField] private float jumpForce = 10f; // 점프 힘
    [SerializeField] private float delay = 0.4f; // 점프하고 바닥착지할때 잠시 멈추게하기

    [SerializeField] private Transform cameraTransform; // 메인 카메라 연결 필수

    [Header("공격 설정")]
    [SerializeField] private float comboResetTime = 1.0f; // 시간 지나면 1타로 초기화

    private Animator anim;
    private Rigidbody rb;

    private int animaMove = Animator.StringToHash("Speed");
    private int animaJump = Animator.StringToHash("Jump");
    private int animaIsGround = Animator.StringToHash("IsGrounded");

    private int animaAttack = Animator.StringToHash("Attack");
    private int animaAttackCount = Animator.StringToHash("AttackCount");

    private PlayerCharacter playerCharacter;//플레이어 공격할때 이펙트나 소리 사용할때 미리

    private Vector2 moveInput;
    private Vector3 targetMoveDir;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isLanding = false; //착지할때 잠시 못움직이게
    private float currentSpeed;

    private bool isAttacking = false;
    private int comboStep = 0;
    private float lastAttackTime = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCharacter = GetComponent<PlayerCharacter>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        ApplyPhysics();
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
        if (value.isPressed && !isJumping && !isLanding && !isAttacking)
        {
            Jump();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && !isJumping && !isLanding && !isAttacking)
        {
            if (Time.time - lastAttackTime > comboResetTime || comboStep >= 3)
            {
                comboStep = 0;
            }
            comboStep++;

            isAttacking = true;
            lastAttackTime = Time.time;

            anim.SetInteger(animaAttackCount, comboStep);
            anim.SetTrigger(animaAttack);

        }
    }

    public void OnSprint(InputValue value)
    {
        isRunning = value.isPressed;
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    private void Move()
    {
        if (isLanding || isAttacking)
        {
            anim.SetFloat(animaMove, 0f);
            targetMoveDir = Vector3.zero;
            return;
        }

        bool isMove = moveInput.magnitude != 0;

        if (isMove)
        {
            if (!isJumping)
                anim.SetFloat(animaMove, currentSpeed);//점프중일때 걷는 애니메이션이 안나오게

            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;

            targetMoveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);
        }
        else
        {
            anim.SetFloat(animaMove, 0f);
            targetMoveDir = Vector3.zero;
        }
    }

    private void ApplyPhysics()
    {
        if (isLanding || isAttacking)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        bool isMove = targetMoveDir.magnitude != 0;

        if (isMove)
        {
            Quaternion viewRot = Quaternion.LookRotation(targetMoveDir.normalized);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, viewRot, Time.fixedDeltaTime * rotationSpeed));
        }

        Vector3 targetVelocity = targetMoveDir * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }


    private void Jump()
    {
        isJumping = true;

        anim.SetBool(animaJump, true);
        anim.SetBool(animaIsGround, false);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isJumping)
        {
            isLanding = true;
            anim.SetBool(animaIsGround, true);//착지 애니메이션 실행
            Invoke("JumpingDelay", delay);
        }
    }


    void JumpingDelay()
    {
        isJumping = false;
        isLanding = false;
        anim.SetBool(animaJump, false);
    }

    public void EndAttack()
    {
        if (comboStep == 3)
        {
            Invoke("ResetAttack", 0.5f);
        }
        else
        {
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;

        if (Time.time - lastAttackTime > comboResetTime || comboStep == 3)
        {
            comboStep = 0;
            anim.SetInteger(animaAttackCount, 0);
        }
    }
}