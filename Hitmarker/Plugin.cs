using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.Hitmarker.Patches;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.github.zehsteam.Hitmarker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class HitmarkerBase : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static HitmarkerBase Instance;
    internal static ManualLogSource mls;

    internal ConfigManager configManager;

    public GameObject canvasPrefab;
    public AudioClip hitSFX;
    public GameObject messageTextPrefab;
    
    void Awake()
    {
        if (Instance == null) Instance = this;

        mls = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        mls.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(HUDManagerPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        configManager = new ConfigManager();

        LoadAssetsFromAssetBundle();
    }

    private void LoadAssetsFromAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "hitmarker_assets");
            AssetBundle MainAssetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            canvasPrefab = MainAssetBundle.LoadAsset<GameObject>("HitmarkerCanvas");
            canvasPrefab.AddComponent<CanvasBehaviour>();

            hitSFX = MainAssetBundle.LoadAsset<AudioClip>("HitSFX");

            messageTextPrefab = MainAssetBundle.LoadAsset<GameObject>("MessageText");
            messageTextPrefab.AddComponent<TextBehaviour>();

            mls.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (Exception e)
        {
            mls.LogError($"Error: Failed to load assets from AssetBundle.\n\n{e}");
        }
    }

    public void OnNewLevelLoaded(int randomMapSeed)
    {
        EnemyAIPatch.Initialize();
    }

    public void SpawnCanvas()
    {
        if (CanvasBehaviour.Instance != null) return;

        GameObject canvas = Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(canvas, SceneManager.GetActiveScene());

        mls.LogInfo("Spawned HitmarkerCanvas.");
    }
}
