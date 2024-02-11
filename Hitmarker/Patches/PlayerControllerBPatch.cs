using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    [HarmonyPatch("DamagePlayerFromOtherClientClientRpc")]
    [HarmonyPostfix]
    static void DamagePlayerFromOtherClientClientRpcPatch(ref int playerWhoHit)
    {
        PlayerControllerB playerScript = StartOfRound.Instance.allPlayerScripts[playerWhoHit];
        if (StartOfRound.Instance.localPlayerController != playerScript) return;

        CanvasBehaviour.Instance.ShowHitmarker();
    }
}
