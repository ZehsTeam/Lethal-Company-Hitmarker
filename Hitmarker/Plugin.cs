using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using com.github.zehsteam.Hitmarker.Patches;
using UnityEngine.SceneManagement;
using System;

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
    
    void Awake()
    {
        if (Instance == null) Instance = this;

        mls = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        mls.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(PlayerControllerBPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        configManager = new ConfigManager();

        LoadAssetBundle();
    }

    private void LoadAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "hitmarker_assets");
            AssetBundle MainAssetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            canvasPrefab = (GameObject)MainAssetBundle.LoadAsset("HitmarkerCanvas");
            canvasPrefab.AddComponent<CanvasBehaviour>();

            hitSFX = (AudioClip)MainAssetBundle.LoadAsset("HitSFX");

            mls.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (Exception e)
        {
            mls.LogError($"Failed to load assets from AssetBundle.\n\n{e}");
        }
    }

    public void SpawnCanvas()
    {
        if (CanvasBehaviour.Instance != null) return;

        GameObject canvas = Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(canvas, SceneManager.GetActiveScene());

        mls.LogInfo("Spawned HitmarkerCanvas.");
    }
}
