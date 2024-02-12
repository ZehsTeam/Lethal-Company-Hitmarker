using GameNetcodeStuff;
using HarmonyLib;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    [HarmonyPatch("DamagePlayerFromOtherClientClientRpc")]
    [HarmonyPrefix]
    static void DamagePlayerFromOtherClientClientRpcPatch(ref PlayerControllerB __instance, int damageAmount, int playerWhoHit)
    {
        PlayerControllerB playerWhoHitScript = StartOfRound.Instance.allPlayerScripts[playerWhoHit];

        if (StartOfRound.Instance.localPlayerController == playerWhoHitScript && !__instance.isPlayerDead)
        {
            ShowHitmarker(__instance, damageAmount);
        }
    }

    private static void ShowHitmarker(PlayerControllerB playerScript, int damage)
    {
        CanvasBehaviour.Instance.ShowHitmarker();
        CanvasBehaviour.Instance.ShowDamageText(playerScript.playerUsername, damage);
    }
}
