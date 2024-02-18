using HarmonyLib;
using Unity.Netcode;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch(int randomSeed)
    {
        // Call on Host or Server
        HitmarkerBase.Instance.OnNewLevelLoaded(randomSeed);
    }

    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPrefix]
    static void GenerateNewLevelClientRpcPatch(int randomSeed)
    {
        bool isHostOrServer = NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

        if (!isHostOrServer)
        {
            // Call on Client
            HitmarkerBase.Instance.OnNewLevelLoaded(randomSeed);
        }
    }
}
