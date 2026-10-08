using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int currentHp;
    public UnityEngine.UI.Slider hpSlider;
    void Awake() { currentHp = maxHp; UpdateBar(); }
    public void TakeDamage(int damage)
    {
        if (currentHp <= 0 || damage <= 0) return;
        currentHp = Mathf.Max(0, currentHp - damage);
        UpdateBar();
        if (currentHp > 0) return;
        if (CompareTag("Player")) { Debug.Log("게임 오버"); Time.timeScale = 0; Cursor.lockState = CursorLockMode.None; }
        else Destroy(gameObject);
    }
    void UpdateBar() { if (hpSlider != null) hpSlider.value = (float)currentHp / Mathf.Max(1, maxHp); }
}
