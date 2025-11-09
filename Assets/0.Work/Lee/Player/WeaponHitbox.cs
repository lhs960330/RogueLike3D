using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField] private PlayerAttack attack;
    private Collider hitBoxCo;
    // 닿는 모든 IDamageable저장
    private List<IDamageable> hitTargets = new List<IDamageable>();

    void Awake()
    {
        hitBoxCo = GetComponent<Collider>();
        hitBoxCo.enabled = false;// 평소에는 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableTarget = other.GetComponent<IDamageable>();
        // 데미지를 줄 수 있고, 이번 공격에서 처음 맞는 대상이라면
        if (damageableTarget != null && !hitTargets.Contains(damageableTarget))
        {
            // 두뇌(PlayerAttack)에 보고
            attack.OnWeaponHit(damageableTarget);
            // 이번 공격에서 때렸다고 기록
            hitTargets.Add(damageableTarget);
        }
          // 애니메이션 이벤트에서 호출할 함수들
    }
      public void Activate()
      {
          hitTargets.Clear(); // 공격 시작 시, 맞은 대상 목록 초기화
          hitBoxCo.enabled = true;
      }

      public void Deactivate()
      {
          hitBoxCo.enabled = false;
      }
}
