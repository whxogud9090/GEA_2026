using UnityEngine;

public class StatPractice : MonoBehaviour
{
    public string playerName = "Knight";
    public int level = 1;
    public float attack = 12.5f;
    public bool isBoss = false;

    [ContextMenu("스탯")]
    void ShowStat()
    {
        Debug.Log(playerName);
        Debug.Log(level);
        Debug.Log(attack);
        Debug.Log(isBoss);
    }
}
