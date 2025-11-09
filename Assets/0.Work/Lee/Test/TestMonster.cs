using UnityEngine;

public class TestMonster : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 100;
    [SerializeField] int currenthp;

    void Awake()
    {
        currenthp = hp;
    }
    public void TakeDamage(int damage)
    {
        currenthp -= damage;
        Debug.Log(currenthp);
    }
}
