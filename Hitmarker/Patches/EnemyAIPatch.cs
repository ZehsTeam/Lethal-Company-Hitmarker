using HarmonyLib;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    [HarmonyPatch("HitEnemyOnLocalClient")]
    [HarmonyPrefix]
    static void HitEnemyOnLocalClientPatch(ref EnemyAI __instance, int force)
    {
        ShowHitmarker(__instance, force);
    }

    private static void ShowHitmarker(EnemyAI enemyAI, int damage)
    {
        if (enemyAI.isEnemyDead) return;

        CanvasBehaviour.Instance.ShowHitmarker();
        CanvasBehaviour.Instance.ShowDamageText(enemyAI.enemyType.enemyName, damage);
    }
}
