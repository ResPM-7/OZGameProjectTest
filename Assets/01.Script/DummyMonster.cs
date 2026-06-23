using UnityEngine;
using TMPro; // 텍스트 조작을 위해 필수

public class DummyMonster : MonoBehaviour
{
    [Header("UI 설정")]
    [SerializeField] private TextMeshPro damageText; // 자식 오브젝트인 Damage (TextPro) 연결
    [SerializeField] private float displayTime = 1.0f; // 숫자가 떠있는 시간

    private void Start()
    {
        // 시작할 때는 글자가 안 보이게 숨겨둡니다.
        if (damageText != null)
        {
            damageText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damageText == null) return;

        // 1. 데미지 숫자를 글자에 적용합니다.
        damageText.text = damage.ToString();

        // 2. 글자 오브젝트를 켭니다.
        damageText.gameObject.SetActive(true);

        // 3. 연속으로 맞을 경우를 대비해, 예전에 예약해둔 '끄기' 타이머를 취소하고 다시 1초 뒤에 끄도록 예약합니다.
        CancelInvoke("HideDamageText");
        Invoke("HideDamageText", displayTime);
    }

    private void HideDamageText()
    {
        // 글자 끄기
        damageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 글자가 켜져 있을 때만 메인 카메라를 바라보게 만듭니다. (글자 좌우 반전 방지)
        if (damageText != null && damageText.gameObject.activeSelf)
        {
            damageText.transform.forward = Camera.main.transform.forward;
        }
    }
}