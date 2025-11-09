
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponHitbox weaponHitbox; // '센서'인 히트박스 참조
    [SerializeField] private PlayerAnimator playerAnimator;

    private PlayerStats stats; // 스탯 컴포넌트 참조

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    public void StartAttack()
    {
        playerAnimator.AttackAnimation(true);
    }

    // 공격 종료 (버튼에서 손을 뗐을 때)
    public void CancelAttack()
    {
        playerAnimator.AttackAnimation(false);
    }

    // WeaponHitbox로부터 보고를 받는 함수
    public void OnWeaponHit(IDamageable target)
    {
        // stats 컴포넌트에서 공격력을 가져와 사용합니다.
        target.TakeDamage(stats.attackDamage);
        Debug.Log($"{target}에게 {stats.attackDamage} 데미지를 입혔습니다!");
        // 여기에 추가적인 타격 효과(사운드, 파티클)를 넣을 수 있습니다.
    }

    // --- 아래 함수들은 애니메이션 이벤트에서 직접 호출됩니다 ---
    public void EnableHitbox()
    {
        weaponHitbox.Activate();
    }

    public void DisableHitbox()
    {
        weaponHitbox.Deactivate();
    }
}