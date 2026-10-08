using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class Lesson05Setup
{
    const string Root = "Assets/Lesson05";
    public static void Run()
    {
        AssetDatabase.Refresh();
        foreach (var folder in new[]{Root, Root+"/Prefabs",Root+"/Materials"})
            if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(folder).Replace('\\','/'),System.IO.Path.GetFileName(folder));
        var scene = EditorSceneManager.OpenScene("Assets/Lesson03/Scenes/Lesson03Arena.unity");
        var tags = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var array = tags.FindProperty("tags");
        if (!Enumerable.Range(0,array.arraySize).Any(i=>array.GetArrayElementAtIndex(i).stringValue=="Enemy")) { array.InsertArrayElementAtIndex(array.arraySize); array.GetArrayElementAtIndex(array.arraySize-1).stringValue="Enemy"; tags.ApplyModifiedProperties(); }
        var old = GameObject.Find("Lesson05Combat"); if (old != null) UnityEngine.Object.DestroyImmediate(old);
        var combat = new GameObject("Lesson05Combat"); combat.AddComponent<CombatSession>();
        var yellow = Material("PlayerBullet",new Color(1,.8f,.08f));
        var red = Material("EnemyBullet",new Color(1,.15f,.12f));
        var bullet = Projectile("Bullet",yellow,"Enemy","Player");
        var enemyBullet = Projectile("EnemyBullet",red,"Player","Enemy");
        var melee = Enemy("EnemyMelee","ember_stalker",false,enemyBullet);
        var ranged = Enemy("EnemyRanged","voxel_human",true,enemyBullet);
        var player = GameObject.Find("Player"); if (player == null) throw new Exception("Existing Player missing");
        player.tag="Player";
        var hp=player.GetComponent<Health>() ?? player.AddComponent<Health>(); hp.maxHp=10;
        var shooter=player.GetComponent<PlayerShooter>() ?? player.AddComponent<PlayerShooter>();
        var fire=player.transform.Find("FirePoint"); if (fire==null) { fire=new GameObject("FirePoint").transform; fire.SetParent(player.transform,false); }
        fire.localPosition=new Vector3(0,1,.8f); shooter.firePoint=fire; shooter.bulletPrefab=bullet;
        hp.hpSlider=HealthBar(combat.transform);
        var points=new[]{new Vector3(6,0,6),new Vector3(-6,0,6),new Vector3(6,0,-6),new Vector3(-6,0,-6)};
        // Use the existing arena floor's top surface, rather than assuming its height.
        var floor=GameObject.Find("Floor"); float ground=0;
        if(floor!=null) { var cols=floor.GetComponentsInChildren<Collider>(); if(cols.Length>0) ground=cols.Max(c=>c.bounds.max.y); }
        var spawn=new GameObject("EnemySpawner"); spawn.transform.SetParent(combat.transform);
        var spawner=spawn.AddComponent<EnemySpawner>(); spawner.enemyPrefabs=new[]{melee,ranged};
        spawner.spawnPoints=points.Select((p,i)=>{ var t=new GameObject("SpawnPoint"+(i+1)).transform; t.SetParent(spawn.transform); p.y=ground; t.position=p; return t; }).ToArray();
        var initial=new[]{new Vector3(5,ground,6),new Vector3(-5,ground,6),new Vector3(6,ground,0),new Vector3(-6,ground,0),new Vector3(5,ground,-6),new Vector3(-5,ground,-6)};
        for(int i=0;i<initial.Length;i++) { var e=(GameObject)PrefabUtility.InstantiatePrefab(i<4?melee:ranged); e.transform.SetParent(combat.transform); e.transform.position=initial[i]; }
        EditorSceneManager.SaveScene(scene);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(scene.path,true)};
        AssetDatabase.SaveAssets();
        if(UnityEngine.Object.FindObjectsByType<Health>(FindObjectsSortMode.None).Length!=7) throw new Exception("Expected player and six enemies");
        Debug.Log("LESSON05_SETUP_OK: existing arena updated, 6 voxel enemies, health UI, projectiles, spawner.");
    }
    static Material Material(string name,Color color)
    {
        string path=Root+"/Materials/"+name+".mat";
        var m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(m==null) { m=new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(m,path); }
        m.color=color; return m;
    }
    static GameObject Projectile(string name,Material mat,string target,string owner)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Sphere); g.name=name; g.transform.localScale=Vector3.one*.2f;
        g.GetComponent<Renderer>().sharedMaterial=mat; g.GetComponent<Collider>().isTrigger=true;
        var rb=g.AddComponent<Rigidbody>(); rb.useGravity=false; rb.collisionDetectionMode=CollisionDetectionMode.ContinuousSpeculative;
        var b=g.AddComponent<Bullet>(); b.targetTag=target; b.ownerTag=owner;
        var prefab=PrefabUtility.SaveAsPrefabAsset(g,Root+"/Prefabs/"+name+".prefab"); UnityEngine.Object.DestroyImmediate(g); return prefab;
    }
    static GameObject Enemy(string name,string model,bool ranged,GameObject bullet)
    {
        var g=new GameObject(name); g.tag="Enemy"; g.AddComponent<Health>();
        var asset=AssetDatabase.LoadAssetAtPath<GameObject>(Root+"/Models/"+model+".obj"); if(asset==null) throw new Exception("Missing voxel model "+model);
        var visual=(GameObject)PrefabUtility.InstantiatePrefab(asset); visual.name="VoxelModel"; visual.transform.SetParent(g.transform,false);
        visual.transform.localRotation=Quaternion.Euler(0,180,0);
        var renderers=visual.GetComponentsInChildren<Renderer>();
        var bounds=renderers[0].bounds; foreach(var r in renderers) bounds.Encapsulate(r.bounds);
        visual.transform.localPosition=new Vector3(-bounds.center.x,-bounds.min.y,-bounds.center.z);
        bounds=renderers[0].bounds; foreach(var r in renderers) bounds.Encapsulate(r.bounds);
        var collider=g.AddComponent<BoxCollider>(); collider.center=bounds.center; collider.size=bounds.size;
        foreach(var r in renderers) foreach(var m in r.sharedMaterials) if(m!=null && m.shader.name!="Universal Render Pipeline/Lit") { var color=m.color; m.shader=Shader.Find("Universal Render Pipeline/Lit"); m.color=color; EditorUtility.SetDirty(m); }
        if(ranged) { var s=g.AddComponent<EnemyShooter>(); s.bulletPrefab=bullet; var f=new GameObject("FirePoint").transform; f.SetParent(g.transform,false); f.localPosition=new Vector3(0,1,collider.size.z*.5f+.25f); s.firePoint=f; }
        else g.AddComponent<EnemyChaser>();
        var prefab=PrefabUtility.SaveAsPrefabAsset(g,Root+"/Prefabs/"+name+".prefab"); UnityEngine.Object.DestroyImmediate(g); return prefab;
    }
    static UnityEngine.UI.Slider HealthBar(Transform parent)
    {
        var canvas=new GameObject("CombatCanvas",typeof(Canvas),typeof(UnityEngine.UI.CanvasScaler)); canvas.transform.SetParent(parent);
        canvas.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=canvas.GetComponent<UnityEngine.UI.CanvasScaler>(); scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1280,720);
        var bar=new GameObject("PlayerHP",typeof(RectTransform),typeof(UnityEngine.UI.Image),typeof(UnityEngine.UI.Slider)); bar.transform.SetParent(canvas.transform,false);
        var rect=(RectTransform)bar.transform; rect.anchorMin=rect.anchorMax=rect.pivot=new Vector2(0,1); rect.anchoredPosition=new Vector2(24,-24); rect.sizeDelta=new Vector2(300,30);
        bar.GetComponent<UnityEngine.UI.Image>().color=new Color(.1f,.1f,.12f,.9f);
        var fill=new GameObject("Fill",typeof(RectTransform),typeof(UnityEngine.UI.Image)); fill.transform.SetParent(bar.transform,false);
        var fr=(RectTransform)fill.transform; fr.anchorMin=Vector2.zero; fr.anchorMax=Vector2.one; fr.offsetMin=new Vector2(3,3); fr.offsetMax=new Vector2(-3,-3);
        fill.GetComponent<UnityEngine.UI.Image>().color=new Color(.9f,.12f,.18f);
        var slider=bar.GetComponent<UnityEngine.UI.Slider>(); slider.fillRect=fr; slider.interactable=false; slider.value=1; return slider;
    }
}
