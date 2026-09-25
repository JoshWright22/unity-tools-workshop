using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Yarn.Unity;

// Rebuilds every lesson scene, prefab and asset in Assets/Workshop.
// Workshop > Rebuild Lessons. Overwrites the scenes, so don't run it after you've started a lesson.
public static class WorkshopBuilder
{
    const string Root = "Assets/Workshop";
    const string Art = Root + "/Art";
    const string Scenes = Root + "/Scenes";

    static Sprite Spr(string name) => AssetDatabase.LoadAssetAtPath<Sprite>($"{Art}/{name}.png");

    [MenuItem("Workshop/Rebuild Lessons")]
    public static void BuildAll()
    {
        AssetDatabase.DeleteAsset(Scenes);
        foreach (var dir in new[] { "Prefabs", "Materials", "Tiles", "Data", "Animation", "Scenes", "Scenes/Finished" })
            Directory.CreateDirectory($"{Root}/{dir}");

        ImportArt();
        AssetDatabase.ImportAsset($"{Root}/Dialogue/Workshop.yarnproject", ImportAssetOptions.ForceUpdate);
        var materials = MakeMaterials();
        var tiles = MakeTiles();
        var enemies = MakeEnemyData();
        var controller = MakeAnimator();
        var prefabs = MakePrefabs(controller);

        CoinGate(prefabs, tiles, enemies, materials);
        Lesson01(prefabs);
        Lesson02(prefabs);
        Lesson03(prefabs);
        LessonAnimator(prefabs);
        Lesson04(prefabs, tiles);
        Lesson05(prefabs);
        Lesson06(prefabs, enemies);
        Lesson07(prefabs, tiles);
        Lesson08(materials);
        Lesson09();

        EditorBuildSettings.scenes = Directory.GetFiles(Scenes, "*.unity", SearchOption.AllDirectories)
            .OrderBy(p => p).Select(p => new EditorBuildSettingsScene(p.Replace('\\', '/'), true)).ToArray();
        AssetDatabase.SaveAssets();
        Debug.Log("Workshop lessons rebuilt");
    }

    // ---------- assets ----------

    static void ImportArt()
    {
        foreach (var path in Directory.GetFiles(Art, "*.png"))
        {
            var imp = (TextureImporter)AssetImporter.GetAtPath(path.Replace('\\', '/'));
            imp.textureType = TextureImporterType.Sprite;
            imp.spriteImportMode = SpriteImportMode.Single;
            imp.spritePixelsPerUnit = 32;
            imp.filterMode = FilterMode.Point;
            imp.textureCompression = TextureImporterCompression.Uncompressed;
            imp.SaveAndReimport();
        }
    }

    static Material[] MakeMaterials()
    {
        return new[] { "Flash", "Wave", "Dissolve" }.Select(n =>
        {
            var path = $"{Root}/Materials/Sprite {n}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(Shader.Find("Workshop/Sprite " + n));
                AssetDatabase.CreateAsset(mat, path);
            }
            return mat;
        }).ToArray();
    }

    static Tile[] MakeTiles()
    {
        return new[] { "grass", "dirt", "stone", "brick" }.Select(n =>
        {
            var path = $"{Root}/Tiles/{n}.asset";
            var tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                AssetDatabase.CreateAsset(tile, path);
            }
            tile.sprite = Spr("tile_" + n);
            tile.colliderType = Tile.ColliderType.Grid;
            EditorUtility.SetDirty(tile);
            return tile;
        }).ToArray();
    }

    static EnemyData[] MakeEnemyData()
    {
        EnemyData Make(string name, int hp, float speed, int dmg, Color tint, float size)
        {
            var path = $"{Root}/Data/Enemy_{name}.asset";
            var d = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            if (d == null)
            {
                d = ScriptableObject.CreateInstance<EnemyData>();
                AssetDatabase.CreateAsset(d, path);
            }
            d.displayName = name; d.health = hp; d.speed = speed; d.damage = dmg; d.tint = tint; d.size = size;
            EditorUtility.SetDirty(d);
            return d;
        }
        return new[]
        {
            Make("Slime", 20, 1.5f, 5, new Color(0.55f, 0.9f, 0.5f), 1f),
            Make("Ghost", 8, 4f, 3, new Color(0.6f, 0.8f, 1f), 0.8f),
            Make("Boss", 400, 0.7f, 25, new Color(0.9f, 0.3f, 0.35f), 2.2f),
        };
    }

    static AnimationClip Clip(string name, bool loop)
    {
        var path = $"{Root}/Animation/{name}.anim";
        AssetDatabase.DeleteAsset(path);
        var clip = new AnimationClip { frameRate = 12 };
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        AssetDatabase.CreateAsset(clip, path);
        return clip;
    }

    static void SpriteKeys(AnimationClip clip, params (float t, Sprite s)[] keys)
    {
        var binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
        AnimationUtility.SetObjectReferenceCurve(clip, binding,
            keys.Select(k => new ObjectReferenceKeyframe { time = k.t, value = k.s }).ToArray());
    }

    static void ScaleKeys(AnimationClip clip, string axis, params (float t, float v)[] keys)
    {
        var curve = new AnimationCurve(keys.Select(k => new Keyframe(k.t, k.v)).ToArray());
        clip.SetCurve("", typeof(Transform), "m_LocalScale." + axis, curve);
    }

    static AnimatorController MakeAnimator()
    {
        var idle = Clip("Idle", true);
        SpriteKeys(idle, (0, Spr("player_idle")));
        ScaleKeys(idle, "y", (0, 1), (0.5f, 0.94f), (1, 1));

        var walk = Clip("Walk", true);
        SpriteKeys(walk, (0, Spr("player_walk1")), (0.25f, Spr("player_walk2")), (0.5f, Spr("player_walk1")));
        ScaleKeys(walk, "y", (0, 1), (0.5f, 1));
        AnimationUtility.SetAnimationEvents(walk, new[]
        {
            new AnimationEvent { time = 0f, functionName = "Footstep" },
            new AnimationEvent { time = 0.25f, functionName = "Footstep" },
        });

        var jump = Clip("Jump", false);
        SpriteKeys(jump, (0, Spr("player_walk2")));
        ScaleKeys(jump, "x", (0, 0.85f), (0.2f, 0.95f));
        ScaleKeys(jump, "y", (0, 1.2f), (0.2f, 1.05f));

        foreach (var c in new[] { idle, walk, jump }) EditorUtility.SetDirty(c);

        var path = $"{Root}/Animation/Player.controller";
        AssetDatabase.DeleteAsset(path);
        var ctrl = AnimatorController.CreateAnimatorControllerAtPath(path);
        ctrl.AddParameter("Speed", AnimatorControllerParameterType.Float);
        ctrl.AddParameter("Grounded", AnimatorControllerParameterType.Bool);
        ctrl.parameters = ctrl.parameters.Select(p => { if (p.name == "Grounded") p.defaultBool = true; return p; }).ToArray();

        var sm = ctrl.layers[0].stateMachine;
        var sIdle = sm.AddState("Idle", new Vector3(300, 0));
        var sWalk = sm.AddState("Walk", new Vector3(300, 120));
        var sJump = sm.AddState("Jump", new Vector3(550, 60));
        sIdle.motion = idle; sWalk.motion = walk; sJump.motion = jump;
        sm.defaultState = sIdle;

        AnimatorStateTransition T(AnimatorState from, AnimatorState to)
        {
            var t = from.AddTransition(to);
            t.hasExitTime = false; t.duration = 0;
            return t;
        }
        T(sIdle, sWalk).AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        T(sWalk, sIdle).AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        T(sIdle, sJump).AddCondition(AnimatorConditionMode.IfNot, 0, "Grounded");
        T(sWalk, sJump).AddCondition(AnimatorConditionMode.IfNot, 0, "Grounded");
        T(sJump, sIdle).AddCondition(AnimatorConditionMode.If, 0, "Grounded");
        return ctrl;
    }

    class Prefabs { public GameObject player, playerAnimated, coin, spinner, ground, npc, enemy; }

    static GameObject SavePrefab(GameObject go, string name)
    {
        var p = PrefabUtility.SaveAsPrefabAsset(go, $"{Root}/Prefabs/{name}.prefab");
        Object.DestroyImmediate(go);
        return p;
    }

    static GameObject SpriteObject(string name, Sprite sprite, int order = 0)
    {
        var go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = order;
        return go;
    }

    static Prefabs MakePrefabs(AnimatorController controller)
    {
        var p = new Prefabs();

        var player = SpriteObject("Player", Spr("player_idle"), 10);
        player.tag = "Player";
        var body = player.AddComponent<Rigidbody2D>();
        body.gravityScale = 3;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        var box = player.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.6f, 0.85f);
        box.offset = new Vector2(0, -0.06f);
        player.AddComponent<PlayerMover>();
        player.AddComponent<PlayerHealth>();
        p.player = SavePrefab(player, "Player");

        var animated = (GameObject)PrefabUtility.InstantiatePrefab(p.player);
        animated.name = "Player (Animated)";
        animated.AddComponent<Animator>().runtimeAnimatorController = controller;
        animated.AddComponent<PlayerAnimator>();
        animated.AddComponent<Footsteps>();
        p.playerAnimated = PrefabUtility.SaveAsPrefabAsset(animated, $"{Root}/Prefabs/Player (Animated).prefab");
        Object.DestroyImmediate(animated);

        var coin = SpriteObject("Coin", Spr("coin"), 5);
        coin.AddComponent<CircleCollider2D>().isTrigger = true;
        var collectible = coin.AddComponent<Collectible>();
        var touched = coin.AddComponent<Touched>();
        touched.onTouched = new UnityEvent();
        UnityEventTools.AddVoidPersistentListener(touched.onTouched, collectible.Collect);
        UnityEventTools.AddBoolPersistentListener(touched.onTouched, coin.SetActive, false);
        p.coin = SavePrefab(coin, "Coin");

        var spinner = SpriteObject("Spinner", Spr("square"));
        spinner.AddComponent<Spinner>();
        p.spinner = SavePrefab(spinner, "Spinner");

        var ground = SpriteObject("Ground", Spr("square"));
        ground.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.3f);
        ground.transform.localScale = new Vector3(30, 1, 1);
        ground.AddComponent<BoxCollider2D>();
        p.ground = SavePrefab(ground, "Ground");

        var npc = SpriteObject("Shopkeeper", Spr("npc"), 5);
        var trigger = npc.AddComponent<CircleCollider2D>();
        trigger.isTrigger = true;
        trigger.radius = 1.5f;
        p.npc = SavePrefab(npc, "Shopkeeper");

        var enemy = SpriteObject("Enemy", Spr("slime"), 5);
        enemy.AddComponent<Enemy>();
        enemy.AddComponent<CircleCollider2D>().isTrigger = true;
        enemy.AddComponent<Hazard>();
        p.enemy = SavePrefab(enemy, "Enemy");

        return p;
    }

    // ---------- scene helpers ----------

    static Scene NewScene(string title, string steps)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var cam = new GameObject("Main Camera") { tag = "MainCamera" };
        var c = cam.AddComponent<Camera>();
        c.orthographic = true;
        c.orthographicSize = 6;
        c.clearFlags = CameraClearFlags.SolidColor;
        c.backgroundColor = new Color(0.08f, 0.08f, 0.1f);
        cam.transform.position = new Vector3(0, 2, -10);
        cam.AddComponent<UniversalAdditionalCameraData>();

        var light = new GameObject("Global Light 2D").AddComponent<Light2D>();
        var so = new SerializedObject(light);
        so.FindProperty("m_LightType").intValue = (int)Light2D.LightType.Global;
        so.ApplyModifiedPropertiesWithoutUndo();

        var info = new GameObject("Lesson Info").AddComponent<LessonInfo>();
        info.title = title;
        info.steps = steps;
        return scene;
    }

    static GameObject Place(GameObject prefab, Vector3 pos, string name = null)
    {
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = pos;
        if (name != null) go.name = name;
        return go;
    }

    static void Save(Scene scene, string name)
    {
        EditorSceneManager.SaveScene(scene, $"{Scenes}/{name}.unity");
    }

    static void Finished(Scene scene, string name)
    {
        var info = Object.FindFirstObjectByType<LessonInfo>();
        info.title += " (finished)";
        info.steps = "This is what the lesson looks like when it's done.\nCompare it with your version.";
        EditorSceneManager.SaveScene(scene, $"{Scenes}/Finished/{name} (Finished).unity");
    }

    static GameObject Score() => new GameObject("Score").AddComponent<ScoreCounter>().gameObject;

    static void PaintLevel(Tilemap map, Tile[] t, bool platforms = true)
    {
        for (int x = -12; x <= 40; x++)
        {
            map.SetTile(new Vector3Int(x, -1, 0), t[0]);
            for (int y = -4; y < -1; y++) map.SetTile(new Vector3Int(x, y, 0), t[1]);
        }
        for (int y = 0; y < 8; y++) { map.SetTile(new Vector3Int(-12, y, 0), t[2]); map.SetTile(new Vector3Int(40, y, 0), t[2]); }
        if (platforms) PaintPlatforms(map, t);
    }

    // jump height is about 2.4 units, so each platform is at most 2 above the last
    static void PaintPlatforms(Tilemap map, Tile[] t)
    {
        foreach (var (x0, x1, y) in new[] { (2, 5, 1), (8, 11, 3), (14, 16, 5), (29, 32, 1) })
            for (int x = x0; x <= x1; x++) map.SetTile(new Vector3Int(x, y, 0), t[3]);
    }

    static Tilemap MakeGrid(Tile[] tiles, bool paint)
    {
        var grid = new GameObject("Grid").AddComponent<Grid>();
        var mapGo = new GameObject("Level");
        mapGo.transform.SetParent(grid.transform);
        var map = mapGo.AddComponent<Tilemap>();
        mapGo.AddComponent<TilemapRenderer>();
        if (paint)
        {
            PaintLevel(map, tiles);
            var body = mapGo.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Static;
            mapGo.AddComponent<TilemapCollider2D>().compositeOperation = Collider2D.CompositeOperation.Merge;
            mapGo.AddComponent<CompositeCollider2D>();
        }
        return map;
    }

    // ---------- the workshop game ----------

    static void CoinGate(Prefabs p, Tile[] tiles, EnemyData[] enemies, Material[] mats)
    {
        var gateMat = AssetDatabase.LoadAssetAtPath<Material>($"{Root}/Materials/Gate Dissolve.mat");
        if (gateMat == null)
        {
            gateMat = new Material(Shader.Find("Workshop/Sprite Dissolve"));
            AssetDatabase.CreateAsset(gateMat, $"{Root}/Materials/Gate Dissolve.mat");
        }
        gateMat.SetFloat("_Amount", 0);
        gateMat.SetColor("_EdgeColor", new Color(0.12f, 0.71f, 0.67f));
        EditorUtility.SetDirty(gateMat);

        var s = NewScene("Coin Gate: build a game in 90 minutes",
            "1. Tilemap: paint platforms, add colliders (15 min)\n" +
            "2. Cinemachine: follow camera (5 min)\n" +
            "3. Animator: idle, walk, jump (20 min)\n" +
            "4. UnityEvents: coins + the flag (10 min)\n" +
            "5. ScriptableObjects: slimes (10 min)\n" +
            "6. Yarn Spinner: the gatekeeper (15 min)\n" +
            "7. Shaders: hit flash, gate color (5 min)\n" +
            "8. Commit + push, play each other's (10 min)\n" +
            "Full steps: Guide/Coin-Gate.md");

        var map = MakeGrid(tiles, false);
        PaintLevel(map, tiles, false);
        var player = Place(p.player, new Vector3(-9, 1, 0), "Player");
        Score();
        var game = new GameObject("Game").AddComponent<GameState>();

        var keeper = Place(p.npc, new Vector3(22.5f, 0.5f, 0), "Gatekeeper");

        var gate = SpriteObject("Gate", Spr("tile_brick"), 5);
        gate.transform.position = new Vector3(26.5f, 2, 0);
        gate.transform.localScale = new Vector3(1, 4, 1);
        gate.AddComponent<BoxCollider2D>();
        gate.AddComponent<Gate>();
        gate.GetComponent<SpriteRenderer>().sharedMaterial = gateMat;

        var flag = SpriteObject("Flag", Spr("flag"), 5);
        flag.transform.position = new Vector3(35.5f, 0.5f, 0);
        flag.AddComponent<BoxCollider2D>().isTrigger = true;
        var flagTouched = flag.AddComponent<Touched>();
        flagTouched.onTouched = new UnityEvent();

        Save(s, "00 Coin Gate");

        // finished version
        Object.DestroyImmediate(GameObject.Find("Grid"));
        MakeGrid(tiles, true);

        Object.DestroyImmediate(player);
        player = Place(p.playerAnimated, new Vector3(-9, 1, 0), "Player");
        player.GetComponent<SpriteRenderer>().sharedMaterial = mats[0];

        Camera.main.gameObject.AddComponent<CinemachineBrain>();
        var vcam = new GameObject("CinemachineCamera").AddComponent<CinemachineCamera>();
        vcam.Follow = player.transform;
        vcam.Lens = new LensSettings { OrthographicSize = 6, NearClipPlane = 0.3f, FarClipPlane = 1000 };
        vcam.transform.position = new Vector3(-9, 2, -10);
        var composer = vcam.gameObject.AddComponent<CinemachinePositionComposer>();
        composer.Damping = new Vector3(0.6f, 0.4f, 0);
        composer.CameraDistance = 10;

        foreach (var pos in new[] { new Vector2(-4, 0.5f), new Vector2(0, 0.5f), new Vector2(3.5f, 2.6f), new Vector2(4.5f, 2.6f),
                     new Vector2(9.5f, 4.6f), new Vector2(10.5f, 4.6f), new Vector2(15.5f, 6.6f) })
            Place(p.coin, pos);

        foreach (var (x, y, d) in new[] { (7f, 0.5f, 0), (13f, 0.5f, 0), (19f, 2.5f, 1) })
        {
            var e = Place(p.enemy, new Vector3(x, y, 0), enemies[d].displayName);
            e.GetComponent<Enemy>().data = enemies[d];
        }

        var runner = AddDialogueSystem();
        if (runner != null)
        {
            var talker = keeper.AddComponent<Talker>();
            talker.dialogueRunner = runner;
            talker.startNode = "Gatekeeper";
        }

        UnityEventTools.AddVoidPersistentListener(flagTouched.onTouched, game.Win);
        Finished(s, "00 Coin Gate");
    }

    static DialogueRunner AddDialogueSystem()
    {
        var prefabPath = AssetDatabase.FindAssets("Dialogue System t:Prefab")
            .Select(AssetDatabase.GUIDToAssetPath).FirstOrDefault(x => x.Contains("yarnspinner"));
        if (prefabPath == null) { Debug.LogError("Yarn Spinner Dialogue System prefab not found"); return null; }
        var system = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
        var runner = system.GetComponentInChildren<DialogueRunner>();
        var so = new SerializedObject(runner);
        so.FindProperty("yarnProject").objectReferenceValue = AssetDatabase.LoadAssetAtPath<YarnProject>($"{Root}/Dialogue/Workshop.yarnproject");
        so.FindProperty("autoStart").boolValue = false;
        so.ApplyModifiedPropertiesWithoutUndo();
        if (Object.FindFirstObjectByType<EventSystem>() == null)
            new GameObject("EventSystem").AddComponent<EventSystem>().gameObject.AddComponent<InputSystemUIInputModule>();
        return runner;
    }

    // ---------- lessons ----------

    static void Lesson01(Prefabs p)
    {
        const string title = "01 - GameObjects, Scripts, Prefabs";
        var s = NewScene(title, "1. Make a square: Hierarchy > + > 2D Object > Sprites > Square\n2. Add Component > Spinner\n3. Press Play, change Speed in the Inspector\n4. Drag it into Prefabs to make a prefab\nFull steps: Guide/01-Basics.md");
        Save(s, "01 Basics");
        var colors = new[] { new Color(0.96f, 0.54f, 0.12f), new Color(0.88f, 0.23f, 0.24f), new Color(0.12f, 0.71f, 0.67f) };
        for (int i = 0; i < 5; i++)
        {
            var go = Place(p.spinner, new Vector3(-6 + i * 3, 2, 0));
            go.GetComponent<SpriteRenderer>().color = colors[i % 3];
            go.GetComponent<Spinner>().speed = 60 * (i + 1) * (i % 2 == 0 ? 1 : -1);
        }
        Finished(s, "01 Basics");
    }

    static void Lesson02(Prefabs p)
    {
        const string title = "02 - UnityEvents: a pickup";
        var s = NewScene(title, "A/D to move, Space to jump.\n1. Make a coin: sprite + Circle Collider 2D (Is Trigger)\n2. Add Touched, wire On Touched in the Inspector\n3. Also call Score > ScoreCounter.Add(1)\nFull steps: Guide/02-UnityEvents.md");
        Place(p.ground, new Vector3(0, -1, 0));
        Place(p.player, new Vector3(-6, 1, 0));
        var score = Score();
        SpriteObject("Coin (make me work)", Spr("coin"), 5).transform.position = new Vector3(-2, 0.5f, 0);
        Save(s, "02 UnityEvents");

        Object.DestroyImmediate(GameObject.Find("Coin (make me work)"));
        for (int i = 0; i < 6; i++)
            Place(p.coin, new Vector3(-3 + i * 1.6f, 0.5f + (i % 2) * 1.5f, 0));
        Finished(s, "02 UnityEvents");
    }

    static void Lesson03(Prefabs p)
    {
        const string title = "03 - The Asset Store";
        var s = NewScene(title, "1. Get Pixel Adventure 1 (free) on assetstore.unity.com\n2. Package Manager > My Assets > Download > Import\n3. Slice a character sheet: Sprite Mode Multiple,\n   Sprite Editor > Slice > Grid By Cell Size 32x32\n4. Swap the Player's sprite for your character\nFull steps: Guide/03-AssetStore.md");
        Place(p.ground, new Vector3(0, -1, 0));
        Place(p.player, new Vector3(-4, 1, 0));
        Save(s, "03 Asset Store");
    }

    static void LessonAnimator(Prefabs p)
    {
        const string title = "04 - The Animator";
        var s = NewScene(title, "Uses the character you imported in lesson 03.\n1. Select Player, open Window > Animation > Animation\n2. Make Idle, Run, Jump clips from the sliced frames\n3. Animator: add Speed + Grounded, make transitions\n4. Add Player Animator + Footsteps components\nFull steps: Guide/04-Animator.md");
        Place(p.ground, new Vector3(0, -1, 0));
        var player = Place(p.player, new Vector3(-4, 1, 0));
        Save(s, "04 Animator");

        Object.DestroyImmediate(player);
        Place(p.playerAnimated, new Vector3(-4, 1, 0), "Player");
        Finished(s, "04 Animator");
    }

    static void Lesson04(Prefabs p, Tile[] tiles)
    {
        const string title = "05 - Tilemaps";
        var s = NewScene(title, "1. Window > 2D > Tile Palette, make a palette\n2. Drag the Tiles folder into it\n3. Paint on Grid > Level with the brush (B)\n4. Add Tilemap Collider 2D + Composite Collider 2D\nFull steps: Guide/05-Tilemap.md");
        MakeGrid(tiles, false);
        var player = Place(p.player, new Vector3(0, 1, 0));
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Camera.main.gameObject.AddComponent<CameraFollow>().target = player.transform;
        Save(s, "05 Tilemap");

        Object.DestroyImmediate(GameObject.Find("Grid"));
        MakeGrid(tiles, true);
        player.GetComponent<Rigidbody2D>().gravityScale = 3;
        PrefabUtility.RecordPrefabInstancePropertyModifications(player.GetComponent<Rigidbody2D>());
        Finished(s, "05 Tilemap");
    }

    static void Lesson05(Prefabs p)
    {
        const string title = "06 - Yarn Spinner dialogue";
        var s = NewScene(title, "1. GameObject > Yarn Spinner > Dialogue System\n2. Drag Dialogue/Workshop project into Yarn Project\n3. Add Talker to the Shopkeeper, drag in the Dialogue Runner\n4. Walk up, press E. Then edit Shopkeeper.yarn\nFull steps: Guide/06-YarnSpinner.md");
        Place(p.ground, new Vector3(0, -1, 0));
        Place(p.player, new Vector3(-6, 1, 0));
        Place(p.npc, new Vector3(2, 0.5f, 0));
        Score();
        Save(s, "06 Yarn Spinner");

        var prefabGuid = AssetDatabase.FindAssets("Dialogue System t:Prefab")
            .Select(AssetDatabase.GUIDToAssetPath).FirstOrDefault(x => x.Contains("yarnspinner"));
        if (prefabGuid == null) { Debug.LogError("Yarn Spinner Dialogue System prefab not found"); return; }
        var system = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(prefabGuid));
        var runner = system.GetComponentInChildren<DialogueRunner>();
        var so = new SerializedObject(runner);
        so.FindProperty("yarnProject").objectReferenceValue = AssetDatabase.LoadAssetAtPath<YarnProject>($"{Root}/Dialogue/Workshop.yarnproject");
        var auto = so.FindProperty("autoStart") ?? so.FindProperty("startAutomatically");
        if (auto != null) auto.boolValue = false;
        so.ApplyModifiedPropertiesWithoutUndo();
        if (Object.FindFirstObjectByType<EventSystem>() == null)
            new GameObject("EventSystem").AddComponent<EventSystem>().gameObject.AddComponent<InputSystemUIInputModule>();

        var talker = GameObject.Find("Shopkeeper").AddComponent<Talker>();
        talker.dialogueRunner = runner;
        Finished(s, "06 Yarn Spinner");
    }

    static void Lesson06(Prefabs p, EnemyData[] data)
    {
        const string title = "07 - ScriptableObjects";
        var s = NewScene(title, "1. Click Data/Enemy_Slime, change its numbers\n2. Right-click in Data > Create > Workshop > Enemy Data\n3. Drop a new Enemy prefab in, give it your data\nFull steps: Guide/07-ScriptableObjects.md");
        Place(p.ground, new Vector3(0, -1, 0));
        Place(p.enemy, new Vector3(-4, 0.5f, 0)).GetComponent<Enemy>().data = data[0];
        Save(s, "07 ScriptableObjects");

        var ghost = Place(p.enemy, new Vector3(1, 3, 0), "Ghost");
        ghost.GetComponent<Enemy>().data = data[1];
        var boss = Place(p.enemy, new Vector3(6, 1.2f, 0), "Boss");
        boss.GetComponent<Enemy>().data = data[2];
        Finished(s, "07 ScriptableObjects");
    }

    static void Lesson07(Prefabs p, Tile[] tiles)
    {
        const string title = "08 - Cinemachine + Timeline";
        var s = NewScene(title, "1. GameObject > Cinemachine > 2D Camera\n2. Set Tracking Target to the Player\n3. Try Position Composer damping + dead zone\n4. Bonus: Window > Sequencing > Timeline intro\nFull steps: Guide/08-Cinemachine-Timeline.md");
        MakeGrid(tiles, true);
        var player = Place(p.playerAnimated, new Vector3(0, 1, 0), "Player");
        Save(s, "08 Cinemachine Timeline");

        Camera.main.gameObject.AddComponent<CinemachineBrain>();
        var vcam = new GameObject("CinemachineCamera").AddComponent<CinemachineCamera>();
        vcam.Follow = player.transform;
        vcam.Lens = new LensSettings { OrthographicSize = 6, NearClipPlane = 0.3f, FarClipPlane = 1000 };
        vcam.transform.position = new Vector3(0, 2, -10);
        var composer = vcam.gameObject.AddComponent<CinemachinePositionComposer>();
        composer.Damping = new Vector3(0.6f, 0.4f, 0);
        composer.CameraDistance = 10;
        Finished(s, "08 Cinemachine Timeline");
    }

    static void Lesson08(Material[] mats)
    {
        const string title = "09 - Shaders";
        var s = NewScene(title, "1. Drag Materials/Sprite Flash onto a sprite\n2. Drag its sliders while playing\n3. Make your own in Shader Graph:\n   Create > Shader Graph > URP > Sprite Unlit\nFull steps: Guide/09-Shaders.md");
        var names = new[] { "Flash", "Wave", "Dissolve" };
        for (int i = 0; i < 3; i++)
        {
            var go = SpriteObject(names[i], Spr(i == 1 ? "npc" : "player_idle"), 0);
            go.transform.position = new Vector3(-5 + i * 5, 2, 0);
            go.transform.localScale = Vector3.one * 4;
        }
        Save(s, "09 Shaders");

        for (int i = 0; i < 3; i++) GameObject.Find(names[i]).GetComponent<SpriteRenderer>().sharedMaterial = mats[i];
        mats[0].SetFloat("_FlashAmount", 0.6f);
        Finished(s, "09 Shaders");
    }

    static void Lesson09()
    {
        const string title = "10 - Visual Scripting";
        var s = NewScene(title, "1. Select the Square, Add Component > Script Machine\n2. New graph, add On Update > Transform Rotate\n3. Expose a Speed variable, change it while playing\nFull steps: Guide/10-VisualScripting.md");
        var sq = SpriteObject("Square", Spr("square"));
        sq.transform.position = new Vector3(0, 2, 0);
        sq.transform.localScale = Vector3.one * 2;
        sq.GetComponent<SpriteRenderer>().color = new Color(0.12f, 0.71f, 0.67f);
        Save(s, "10 Visual Scripting");
    }
}
