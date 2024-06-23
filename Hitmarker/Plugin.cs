using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.Hitmarker.MonoBehaviours;
using com.github.zehsteam.Hitmarker.Patches;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static Plugin Instance;
    internal static ManualLogSource logger;

    internal static ConfigManager ConfigManager;

    internal static bool IsHostOrServer => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        logger = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(HUDManagerPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        ConfigManager = new ConfigManager();

        Content.Load();
    }

    public void CreateHitmarkerCanvas()
    {
        if (HitmarkerCanvasBehaviour.Instance != null) return;

        Object.Instantiate(Content.HitmarkerCanvasPrefab);

        Plugin.logger.LogInfo("Instantiated Hitmarker canvas.");
    }

    public void LogInfoExtended(object data)
    {
        if (ConfigManager.ExtendedLogging.Value)
        {
            logger.LogInfo(data);
        }
    }
}
