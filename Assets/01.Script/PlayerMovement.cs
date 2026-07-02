using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
    Hit,
    Landing
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f; // 캐릭터가 도는 속도
    [SerializeField] private float jumpForce = 10f; // 점프 힘
    [SerializeField] private float jumpDelay = 0.4f; // 점프하고 바닥착지할때 잠시 멈추게하기

    [SerializeField] private Transform cameraTransform; // 메인 카메라 연결 필수


    private Animator anim;
    private Rigidbody rb;

    private int animaMove = Animator.StringToHash("Speed");
    private int animaJump = Animator.StringToHash("Jump");
    private int animaIsGround = Animator.StringToHash("IsGrounded");
    private int animaAttack = Animator.StringToHash("Attack");
    private int animaAttackCount = Animator.StringToHash("AttackCount");
    private int animaHit = Animator.StringToHash("Hit");

    //private PlayerCharacter playerCharacter;//플레이어 공격할때 이펙트나 소리 사용할때 미리

    private Vector2 moveInput;
    private Vector3 targetMoveDir;
    private bool isRunning = false;
    private float currentSpeed;

    private bool canNextAttack = false;
    private int comboStep = 0;


    private WaitForSeconds jumpWaitDealy;

    private bool isBusy => currentState == PlayerState.Attacking ||
                           currentState == PlayerState.Hit ||
                           currentState == PlayerState.Landing;

    private bool canAct => currentState == PlayerState.Idle ||
                           currentState == PlayerState.Moving;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //playerCharacter = GetComponent<PlayerCharacter>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
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

    private void ChangeState(PlayerState state)
    {
        if (currentState == state) return;
        currentState = state;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && canAct)
        {
            Jump();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if (canAct)
            {
                comboStep = 1;
                ChangeState(PlayerState.Attacking);

                anim.SetInteger(animaAttackCount, comboStep);
                anim.SetTrigger(animaAttack);

                canNextAttack = false;
            }
            else if (currentState == PlayerState.Attacking && canNextAttack)
            {
                if (comboStep < 3)
                {
                    comboStep++;
                    anim.SetInteger(animaAttackCount, comboStep);
                    anim.SetTrigger(animaAttack);

                    canNextAttack = false;
                }
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
        if (isBusy)
        {
            anim.SetFloat(animaMove, 0f);
            targetMoveDir = Vector3.zero;
            return;
        }

        bool isMove = moveInput.magnitude != 0;

        if (isMove)
        {
            if (currentState != PlayerState.Jumping)
                anim.SetFloat(animaMove, currentSpeed);//점프중일때 걷는 애니메이션이 안나오게

            Vector3 lookForward = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraTransform.right.x, 0f, cameraTransform.right.z).normalized;

            targetMoveDir = (lookForward * moveInput.y) + (lookRight * moveInput.x);

            if (currentState == PlayerState.Idle) ChangeState(PlayerState.Moving);
        }
        else
        {
            anim.SetFloat(animaMove, 0f);
            targetMoveDir = Vector3.zero;

            if (currentState == PlayerState.Moving) ChangeState(PlayerState.Idle);
        }
    }

    private void ApplyPhysics()
    {
        if (isBusy)
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
        ChangeState(PlayerState.Jumping);

        anim.SetBool(animaJump, true);
        anim.SetBool(animaIsGround, false);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void SetNextAttackReady(bool isReady)
    {
        canNextAttack = isReady;
    }

    public void ResetCombo()
    {
        comboStep = 0;
        anim.SetInteger(animaAttackCount, 0);

        anim.ResetTrigger(animaAttack);//만약을 위해 트리거 누적 방지

        if (currentState == PlayerState.Attacking)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public void TakeHit()
    {
        ChangeState(PlayerState.Hit);
        ResetCombo();
        anim.SetTrigger(animaHit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && currentState == PlayerState.Jumping)
        {
            ChangeState(PlayerState.Landing);
            anim.SetBool(animaIsGround, true);//착지 애니메이션 실행
            StartCoroutine(JumpingDelay());
        }
    }


    IEnumerator JumpingDelay()
    {
        yield return jumpWaitDealy;
        anim.SetBool(animaJump, false);
        ChangeState(PlayerState.Idle);
    }


}