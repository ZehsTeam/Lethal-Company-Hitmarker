using com.github.zehsteam.Hitmarker.MonoBehaviours;
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    [HarmonyPatch("HitEnemyOnLocalClient")]
    [HarmonyPrefix]
    static void HitEnemyOnLocalClientPatch(ref EnemyAI __instance, int force, PlayerControllerB playerWhoHit = null)
    {
        if (playerWhoHit == null) return;
        if (!Utils.IsLocalPlayer(playerWhoHit)) return;

        HitEnemy(__instance, force, playerWhoHit);
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPrefix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int force, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;

        PlayerControllerB playerWhoHitScript = Utils.GetPlayerScript(playerWhoHit);
        if (Utils.IsLocalPlayer(playerWhoHitScript)) return;

        HitEnemy(__instance, force, playerWhoHitScript);
    }

    [HarmonyPatch("HitEnemyClientRpc")]
    [HarmonyPrefix]
    static void HitEnemyClientRpcPatch(ref EnemyAI __instance, int force, int playerWhoHit)
    {
        if (playerWhoHit == -1) return;
        if (Plugin.IsHostOrServer) return;

        PlayerControllerB playerWhoHitScript = Utils.GetPlayerScript(playerWhoHit);
        if (Utils.IsLocalPlayer(playerWhoHitScript)) return;

        HitEnemy(__instance, force, playerWhoHitScript);
    }

    private static void HitEnemy(EnemyAI enemyAI, int force, PlayerControllerB playerWhoHit)
    {
        if (!enemyAI.enemyType.canDie) return;
        if (enemyAI.isEnemyDead || enemyAI.enemyHP <= 0) return;

        bool fromLocalPlayer = Utils.IsLocalPlayer(playerWhoHit);
        string enemyName = enemyAI.enemyType.enemyName;
        bool isKilled = enemyAI.enemyHP - force <= 0;

        if (fromLocalPlayer)
        {
            HitmarkerCanvasBehaviour.Instance.ShowHitmarker(killed: isKilled);
            HitmarkerCanvasBehaviour.Instance.ShowDamageMessage(enemyName, force);
        }

        if (isKilled)
        {
            HitmarkerCanvasBehaviour.Instance.ShowKillMessage(enemyName, fromLocalPlayer, playerWhoHit.playerUsername);
        }

        LogInfoExtended("HitEnemy();", enemyAI, force, playerWhoHit);
    }

    private static void LogInfoExtended(string functionName, EnemyAI enemyAI, int force, PlayerControllerB playerWhoHit)
    {
        NetworkObject networkObject = enemyAI.gameObject.GetComponent<NetworkObject>();

        string localText = Utils.IsLocalPlayer(playerWhoHit) ? " (LOCAL)" : "";

        string message = $"{functionName} NetworkObjectId: {networkObject.NetworkObjectId}\n\n";
        message += $"Player \"{playerWhoHit.playerUsername}\"{localText} hit \"{enemyAI.enemyType.enemyName}\" for {force} force.\n";
        message += $"isEnemyDead: {enemyAI.isEnemyDead}, enemyHP: {enemyAI.enemyHP}, (new enemyHP should be {enemyAI.enemyHP - force})\n";

        Plugin.Instance.LogInfoExtended($"\n\n{message.Trim()}\n");
    }
}
