using UnityEngine;

public class HpPractice : MonoBehaviour
{
    public int currentHp = 7;
    public int maxHp = 10;

    [ContextMenu("체력")]
    void PrintHp()
    {
        int a = (int)((float)currentHp / maxHp * 100);
        Debug.Log("HP " + a + "%");

        int b = currentHp * 100 / maxHp;
        Debug.Log("HP " + b + "%");
    }
}
