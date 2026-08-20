using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // 외부에서 읽기만 가능하도록 프로퍼티 설정
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }

    // 이동 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    //달리기
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) IsRunning = true;
        else if (context.canceled) IsRunning = false;
    }

    //점프 처리
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) IsRunning = true;
        else if (context.canceled) IsRunning = false;
    }

    //3. 공격 처리
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) IsAttackPressed = true;
        else if (context.canceled) IsAttackPressed = false;
    }

    //상점 (P키)
    public void OnShop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ShopController.instance != null)
            {
                ShopController.instance.ToggleShop();
            }
        }
    }

    //인벤토리 (I키)
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("인벤토리 열기/닫기");
        }
    }

    //FSM할때 상태전환이 다시 되지 않게 하기위해 구현
    public void ConsumeJump() => IsJumpPressed = false;
    public void ConsumeAttack() => IsAttackPressed = false;
}