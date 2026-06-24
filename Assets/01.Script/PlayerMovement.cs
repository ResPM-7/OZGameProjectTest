using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f; // 캐릭터가 도는 속도
    [SerializeField] private float jumpForce = 10f; // 점프 힘
    [SerializeField] private float jumpDelay = 0.4f; // 점프하고 바닥착지할때 잠시 멈추게하기

    [SerializeField] private Transform cameraTransform; // 메인 카메라 연결 필수

    [Header("공격 설정")]
    [SerializeField] private float comboResetTime = 1.0f; // 시간 지나면 1타로 초기화
    [SerializeField] private float endAttackDelay = 0.5f; // 마지막 공격후 딜레이 

    private Animator anim;
    private Rigidbody rb;

    private int animaMove = Animator.StringToHash("Speed");
    private int animaJump = Animator.StringToHash("Jump");
    private int animaIsGround = Animator.StringToHash("IsGrounded");

    private int animaAttack = Animator.StringToHash("Attack");
    private int animaAttackCount = Animator.StringToHash("AttackCount");

    private int animaHit = Animator.StringToHash("Hit");
    private bool isHit = false;

    //private PlayerCharacter playerCharacter;//플레이어 공격할때 이펙트나 소리 사용할때 미리

    private Vector2 moveInput;
    private Vector3 targetMoveDir;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isLanding = false; //착지할때 잠시 못움직이게
    private float currentSpeed;

    private bool isAttacking = false;
    private bool canNextAttack = false;
    private bool isComboQueued = false; // 다음콤보 대기중
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private WaitForSeconds attackResetWait;

    private WaitForSeconds jumpWaitDealy;


    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //playerCharacter = GetComponent<PlayerCharacter>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
        attackResetWait = new WaitForSeconds(endAttackDelay);
        jumpWaitDealy = new WaitForSeconds(jumpDelay);
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
        if (value.isPressed && !isJumping && !isLanding && !isAttacking && !isHit)
        {
            Jump();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && !isJumping && !isLanding && !isHit)
        {
            if (isAttacking && comboStep == 3) return; //왼쪽 클릭을 계속하다가 3타 끝날때쯤 왼클릭을 그만두면 isAttack이 활성화 되어있어서 플레이어 못 움직여서 넣음

            if (!isAttacking || canNextAttack)
            {
                if (!isAttacking)//2타 공격중 exit가기 직전에 클릭하면 count가 3이되어버림 하지만 실제 애니메이션은 idle로 가버림 attack에 문제가 생기기떄문에 이렇게 수정
                {
                    comboStep = 0;
                }
                else if (Time.time - lastAttackTime > comboResetTime || comboStep >= 3)
                {
                    comboStep = 0;
                }

                if (isAttacking && canNextAttack)
                {
                    isComboQueued = true;
                }

                comboStep++;

                isAttacking = true;
                canNextAttack = false;
                lastAttackTime = Time.time;

                anim.SetInteger(animaAttackCount, comboStep);
                anim.SetTrigger(animaAttack);

            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isRunning = value.isPressed;
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    private void Move()
    {
        if (isLanding || isAttacking || isHit)
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
        if (isLanding || isAttacking || isHit)
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
            StartCoroutine(JumpingDelay());
        }
    }


    IEnumerator JumpingDelay()
    {
        yield return jumpWaitDealy;
        isJumping = false;
        isLanding = false;
        anim.SetBool(animaJump, false);
    }

    public void NextAttack() // 애니메이션 이벤트 Nextattack <= anim < EndAttack 사이에서만 다음공격 애니메이션 실행
    {
        canNextAttack = true;
    }

    public void EndAttack()//애니메이션 이벤트 이구간을 지나면 다음 공격애니메이션실행 안됨 다시처음부터
    {
        canNextAttack = false;

        if (isComboQueued)
        {
            isComboQueued = false;
            return;
        }

        if (comboStep == 3)
        {
            StartCoroutine(DelayedResetAttack());
        }
        else
        {
            ResetAttack();
        }
    }

    private IEnumerator DelayedResetAttack()
    {
        yield return attackResetWait;

        ResetAttack();
    }

    private void ResetAttack()// 2타 문제를 count 3가 되어버리는문제에서 다른문제 isattack= true가 되어버려서 2타 애니메이션 끝에 event를 넣어 문제해결
    {
        isAttacking = false;
        comboStep = 0;
        anim.SetInteger(animaAttackCount, 0);
    }

    public void TakeHit()
    {
        isHit = true;
        isAttacking = false;
        comboStep = 0;

        anim.SetInteger(animaAttackCount, 0);
        anim.SetTrigger(animaHit);
    }

    public void EndHit()
    {
        isHit = false;
    }
}