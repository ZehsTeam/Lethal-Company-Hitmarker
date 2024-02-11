using HarmonyLib;

namespace com.github.zehsteam.Hitmarker.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAIPatch
{
    [HarmonyPatch("HitEnemyOnLocalClient")]
    [HarmonyPostfix]
    static void HitEnemyOnLocalClientPatch()
    {
        CanvasBehaviour.Instance.ShowHitmarker();
    }
}
