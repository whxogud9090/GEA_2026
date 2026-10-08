using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class CombatSession : MonoBehaviour
{
    void Awake() { Time.timeScale = 1; }
    void Update()
    {
        if (Time.timeScale == 0 && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        { Time.timeScale = 1; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    }
    void OnGUI()
    {
        if (Time.timeScale != 0) return;
        var style = new GUIStyle(GUI.skin.box) { fontSize = 28, alignment = TextAnchor.MiddleCenter };
        GUI.Box(new Rect(Screen.width / 2 - 220, Screen.height / 2 - 70, 440, 140), "GAME OVER\nR : Restart", style);
    }
}
