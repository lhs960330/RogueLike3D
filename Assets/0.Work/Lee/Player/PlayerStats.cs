using UnityEngine;

// IDamageable 인터페이스를 구현하여 데미지 처리 책임을 가집니다.
public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] public int maxHp = 100;
    public int currentHp { get; private set; }

    [Header("Movement")]
    [SerializeField] public float moveForce = 50f;
    [SerializeField] public float maxSpeed = 5f;
    [SerializeField] public float drag = 10f;
    [SerializeField] public float rotationSpeed = 10f;

    [Header("Dash")]
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] public float dashDuration = 0.2f;

    [Header("Attack")]
    [SerializeField] public int attackDamage = 10;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Player took {damage} damage. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 여기에 사망 관련 로직을 구현합니다.
        Debug.Log("Player has died.");
        // 예: gameObject.SetActive(false);
    }
}
