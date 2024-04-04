using GameNetcodeStuff;

namespace com.github.zehsteam.Hitmarker;

internal class Utils
{
    public static bool IsLocalPlayer(PlayerControllerB playerScript)
    {
        return StartOfRound.Instance.localPlayerController == playerScript;
    }

    public static PlayerControllerB GetPlayerScript(int playerWhoHit)
    {
        if (playerWhoHit < 0 || playerWhoHit > StartOfRound.Instance.allPlayerScripts.Length - 1) return null;

        return StartOfRound.Instance.allPlayerScripts[playerWhoHit];
    }
}
