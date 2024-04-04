using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.Hitmarker.MonoBehaviours;
using com.github.zehsteam.Hitmarker.Patches;
using HarmonyLib;
using Unity.Netcode;

namespace com.github.zehsteam.Hitmarker;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class HitmarkerBase : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static HitmarkerBase Instance;
    internal static ManualLogSource mls;

    internal ConfigManager configManager;

    internal static bool IsHostOrServer => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        mls = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        mls.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(HUDManagerPatch));
        harmony.PatchAll(typeof(EnemyAIPatch));

        configManager = new ConfigManager();

        Content.Load();
    }

    internal void SpawnHitmarkerCanvas()
    {
        if (HitmarkerCanvasBehaviour.Instance != null) return;

        UnityEngine.Object.Instantiate(Content.hitmarkerCanvasPrefab);
    }
}
