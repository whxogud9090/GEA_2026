using UnityEngine;

public class DamagePractice : MonoBehaviour
{
    public int attack = 12;
    public int defense = 5;
    public bool isCritical = true;
    public bool targetIsBoss = true;

    [ContextMenu("데미지")]
    void Calc()
    {
        int damage = attack - defense;
        damage = Mathf.Max(damage, 1);
        // damage = damage < 1 ? 1 : damage;
        if (isCritical)
        {
            damage = damage * 2;
        }
        if (targetIsBoss)
        {
            damage = damage / 2;
        }
        Debug.Log(damage);
    }
}
