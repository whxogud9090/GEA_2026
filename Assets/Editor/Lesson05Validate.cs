using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[InitializeOnLoad]
public static class Lesson05Validate
{
    static int stage;
    static float started;
    static Health player;
    static Health target;
    static Mouse mouse;
    static int baseline;
    static Lesson05Validate()
    {
        EditorApplication.playModeStateChanged += Changed;
        if(SessionState.GetBool("Lesson05Validate",false)) EditorApplication.update += Tick;
    }
    public static void Run()
    {
        EditorSceneManager.OpenScene("Assets/Lesson03/Scenes/Lesson03Arena.unity");
        var p=GameObject.Find("Player");
        var input=p.GetComponent<PlayerInput>();
        if(input.notificationBehavior!=PlayerNotifications.SendMessages || input.actions.FindAction("Player/Attack")==null) throw new Exception("Attack input configuration invalid");
        var enemies=GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length<5 || enemies.Any(e=>e.GetComponentsInChildren<Renderer>().Length==0 || e.GetComponent<Health>()==null)) throw new Exception("Enemy configuration invalid");
        if(enemies.Any(e=>e.GetComponentsInChildren<Collider>().Length!=1)) throw new Exception("Enemy collider must be on parent only");
        SessionState.SetBool("Lesson05Validate",true);
        EditorApplication.EnterPlaymode();
    }
    public static void FinalizeScene()
    {
        Lesson05Setup.Run();
        Preview();
        Run();
    }
    static void Changed(PlayModeStateChange state)
    {
        if(!SessionState.GetBool("Lesson05Validate",false) || state!=PlayModeStateChange.EnteredPlayMode) return;
        Time.captureDeltaTime=1f/60;
        Application.runInBackground=true;
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
        InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        player=GameObject.Find("Player").GetComponent<Health>();
        foreach(var e in UnityEngine.Object.FindObjectsByType<EnemyChaser>(FindObjectsSortMode.None)) e.enabled=false;
        foreach(var e in UnityEngine.Object.FindObjectsByType<EnemyShooter>(FindObjectsSortMode.None)) e.enabled=false;
        target=GameObject.FindGameObjectsWithTag("Enemy")[0].GetComponent<Health>();
        var shooter=player.GetComponent<PlayerShooter>();
        target.transform.position=player.transform.position+player.transform.forward*4;
        mouse=Mouse.current??InputSystem.AddDevice<Mouse>();
        started=Time.time; baseline=UnityEngine.Object.FindObjectsByType<Bullet>(FindObjectsSortMode.None).Length;
        EditorApplication.update-=Tick; EditorApplication.update+=Tick;
    }
    static void Tick()
    {
        if(!EditorApplication.isPlaying || player==null) return;
        try
        {
            float elapsed=Time.time-started;
            if(stage==0 && elapsed>.5f) { InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Left)); stage=1; }
            else if(stage==1 && elapsed>.6f) { InputSystem.QueueStateEvent(mouse,new MouseState()); stage=2; }
            else if(stage==2 && elapsed>1.5f)
            {
                if(target.currentHp!=2) throw new Exception("Real Attack input/projectile did not damage enemy: HP="+target.currentHp);
                target.TakeDamage(2); stage=3;
            }
            else if(stage==3 && elapsed>2.6f)
            {
                if(target!=null) throw new Exception("Enemy death did not destroy object");
                if(UnityEngine.Object.FindObjectsByType<Bullet>(FindObjectsSortMode.None).Length!=baseline) throw new Exception("Projectile lifetime failed");
                player.TakeDamage(1); if(player.currentHp!=9 || Mathf.Abs(player.hpSlider.value-.9f)>.001f) throw new Exception("Health UI failed");
                stage=4;
            }
            else if(stage==4 && elapsed>3.3f)
            {
                if(GameObject.FindGameObjectsWithTag("Enemy").Length!=6) throw new Exception("3 second spawner failed");
                UnityEngine.Object.FindFirstObjectByType<EnemySpawner>().enabled=false;
                var chaser=UnityEngine.Object.FindObjectsByType<EnemyChaser>(FindObjectsSortMode.None)[0];
                chaser.transform.position=player.transform.position+player.transform.right;
                chaser.enabled=true;
                started=Time.time; stage=5;
            }
            else if(stage==5 && elapsed>.5f)
            {
                if(player.currentHp>=9) throw new Exception("Melee attack failed");
                foreach(var e in UnityEngine.Object.FindObjectsByType<EnemyChaser>(FindObjectsSortMode.None)) e.enabled=false;
                foreach(var e in UnityEngine.Object.FindObjectsByType<EnemyShooter>(FindObjectsSortMode.None)) e.enabled=false;
                var ranged=UnityEngine.Object.FindObjectsByType<EnemyShooter>(FindObjectsSortMode.None)[0];
                ranged.transform.position=player.transform.position+player.transform.forward*4;
                ranged.enabled=true;
                baseline=player.currentHp; started=Time.time; stage=6;
            }
            else if(stage==6 && elapsed>2.6f)
            {
                if(player.currentHp>=baseline) throw new Exception("Ranged projectile attack failed");
                player.TakeDamage(99); if(Time.timeScale!=0 || player.currentHp!=0) throw new Exception("Game over failed");
                File.WriteAllText(Path.GetFullPath("../work/lesson05-validation.txt"),"PASS: 6 voxel enemies; parent colliders; real left-click Attack input; projectile damage; enemy death; bullet cleanup; player HP slider; 3-second spawner; melee attack; ranged projectile attack; game-over pause.\n");
                Finish(0);
            }
            if(elapsed>8) throw new Exception("Validation timeout");
        }
        catch(Exception e) { Debug.LogException(e); Finish(1); }
    }
    static void Finish(int code) { SessionState.SetBool("Lesson05Validate",false); EditorApplication.update-=Tick; Time.timeScale=1; EditorApplication.Exit(code); }
    public static void Preview()
    {
        EditorSceneManager.OpenScene("Assets/Lesson03/Scenes/Lesson03Arena.unity");
        var camera=Camera.main;
        camera.transform.SetParent(null); camera.transform.position=new Vector3(18,20,-22); camera.transform.LookAt(Vector3.zero);
        var rt=new RenderTexture(1440,900,24); camera.targetTexture=rt; camera.Render(); RenderTexture.active=rt;
        var tex=new Texture2D(1440,900,TextureFormat.RGB24,false); tex.ReadPixels(new Rect(0,0,1440,900),0,0); tex.Apply();
        File.WriteAllBytes(Path.GetFullPath("../work/lesson05-preview.png"),tex.EncodeToPNG());
        camera.targetTexture=null; RenderTexture.active=null; UnityEngine.Object.DestroyImmediate(tex); UnityEngine.Object.DestroyImmediate(rt);
    }
}
