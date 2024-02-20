using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using Unity.Netcode;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    private static List<string> deadEnemyIDs;

    public static void Initialize()
    {
        deadEnemyIDs = new List<string>();
    }

    [HarmonyPatch("HitEnemyOnLocalClient")]
    [HarmonyPrefix]
    static void HitEnemyOnLocalClientPatch(ref EnemyAI __instance, PlayerControllerB playerWhoHit, int force)
    {
        if (playerWhoHit == null) return;

        bool fromLocalPlayer = StartOfRound.Instance.localPlayerController == playerWhoHit;

        if (fromLocalPlayer)
        {
            HitmarkerBase.mls.LogInfo($"[LOCAL] HitEnemyOnLocalClient(); {playerWhoHit.playerUsername} hit {__instance.enemyType.enemyName} ({__instance.enemyHP - force} HP)");

            HitEnemy(__instance, playerWhoHit, force, true);
        }
    }

    [HarmonyPatch("HitEnemyServerRpc")]
    [HarmonyPrefix]
    static void HitEnemyServerRpcPatch(ref EnemyAI __instance, int playerWhoHit, int force)
    {
        if (playerWhoHit == -1) return;

        PlayerControllerB playerWhoHitScript = StartOfRound.Instance.allPlayerScripts[playerWhoHit];
        bool fromLocalPlayer = StartOfRound.Instance.localPlayerController == playerWhoHitScript;

        if (!fromLocalPlayer)
        {
            HitmarkerBase.mls.LogInfo($"[SERVER] HitEnemyServerRpc(); {playerWhoHitScript.playerUsername} hit {__instance.enemyType.enemyName} ({__instance.enemyHP - force} HP)");

            HitEnemy(__instance, playerWhoHitScript, force, false);
        }
    }

    [HarmonyPatch("HitEnemyClientRpc")]
    [HarmonyPrefix]
    static void HitEnemyClientRpcPatch(ref EnemyAI __instance, int playerWhoHit, int force)
    {
        if (playerWhoHit == -1) return;

        bool isHostOrServer = NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

        PlayerControllerB playerWhoHitScript = StartOfRound.Instance.allPlayerScripts[playerWhoHit];
        bool fromLocalPlayer = StartOfRound.Instance.localPlayerController == playerWhoHitScript;

        if (!isHostOrServer && !fromLocalPlayer)
        {
            HitmarkerBase.mls.LogInfo($"[CLIENT] HitEnemyClientRpc(); {playerWhoHitScript.playerUsername} hit {__instance.enemyType.enemyName} ({__instance.enemyHP - force} HP)");

            HitEnemy(__instance, playerWhoHitScript, force, false);
        }
    }

    private static void HitEnemy(EnemyAI enemyAI, PlayerControllerB playerWhoHit, int damage, bool fromLocalPlayer)
    {
        if (enemyAI == null || playerWhoHit == null) return;
        if (enemyAI.enemyHP <= 0) return;
        if (!enemyAI.enemyType.canDie) return;

        string enemyId = GetEnemyID(enemyAI);
        if (deadEnemyIDs.Contains(enemyId)) return;

        string enemyName = GetEnemyName(enemyAI);
        bool killedEnemy = enemyAI.enemyHP - damage <= 0;

        LogInfo(enemyAI, playerWhoHit, damage, fromLocalPlayer);

        if (fromLocalPlayer)
        {
            CanvasBehaviour.Instance.ShowHitmarker(killedEnemy);
            CanvasBehaviour.Instance.ShowDamageMessage($"{enemyName} -{damage} HP");
        }

        if (killedEnemy)
        {
            deadEnemyIDs.Add(enemyId);

            string message = fromLocalPlayer ? $"Killed {enemyName}" : $"{playerWhoHit.playerUsername} Killed {enemyName}";
            CanvasBehaviour.Instance.ShowKillMessage(message.Trim(), fromLocalPlayer);
        }
    }

    private static void LogInfo(EnemyAI enemyAI, PlayerControllerB playerWhoHit, int damage, bool fromLocalPlayer)
    {
        string info = string.Empty;

        info += $"HitEnemy - {enemyAI.enemyType.enemyName}\n";
        info += $"PlayerWhoHit - {playerWhoHit.playerUsername}\n\n";
        info += $"Hit from LocalPlayer? {fromLocalPlayer}\n";
        info += $"Current HP: {enemyAI.enemyHP}, Damage: {damage}, New HP: {enemyAI.enemyHP - damage}\n";

        HitmarkerBase.mls.LogInfo($"\n\n{info.Trim()}\n");
    }

    private static string GetEnemyName(EnemyAI enemyAI)
    {
        string enemyName = enemyAI.enemyType.enemyName;

        // Use original enemy names
        if (HitmarkerBase.Instance.configManager.UseOriginalEnemyNames)
        {
            return enemyName;
        }

        // Use other enemy names
        if (enemyName == "Centipede") return "Snare Flea";
        if (enemyName == "Crawler") return "Thumper";
        if (enemyName == "Girl") return "Ghost Girl";
        if (enemyName == "Puffer") return "Spore Lizard";
        if (enemyName == "Spring") return "Coil-Head";
        if (enemyName == "Masked") return "Mimic";
        if (enemyName == "Flowerman") return "Bracken";

        return enemyName;
    }

    private static string GetEnemyID(EnemyAI enemyAI)
    {
        NetworkObject networkObject = enemyAI.gameObject.GetComponent<NetworkObject>();

        string enemyName = enemyAI.enemyType.enemyName;
        string instanceID = enemyAI.gameObject.GetInstanceID().ToString();
        string networkObjectId = networkObject.NetworkObjectId.ToString();

        return $"{enemyName}_{instanceID}_{networkObjectId}";
    }
}
