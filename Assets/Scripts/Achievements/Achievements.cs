using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private void Start()
    {
        UnlockWelcomeAchievement();
    }

    private static void SetAchievement(string ach_ID)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetAchievement (ach_ID);
            SteamUserStats.StoreStats();
            Debug.Log("Achievement " + ach_ID + " unlocked!");
        }
        else
        {
            Debug.LogError("Steamworks API is not initialized. -> " + ach_ID);
        }
    }

    public void UnlockWelcomeAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("WELCOME_ACHIEVEMENT");
    }
    public static void UnlockFirstCoinAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FIRST_COIN_ACHIEVEMENT");
    }
    public static void UnlockAllCoinsAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_ACHIEVEMENT");
    }
    public static void UnlockAllCoinsLvl1Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_LVL1_ACHIEVEMENT");
    }
    public static void UnlockAllCoinsLvl2Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_LVL2_ACHIEVEMENT");
    }
    public static void UnlockAllCoinsLvl3Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_LVL3_ACHIEVEMENT");
    }
    public static void UnlockAllCoinsBfAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_BF_ACHIEVEMENT");
    }
    public static void UnlockFullLifeLvl1Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FULL_LIFE_LVL1_ACHIEVEMENT");
    }
    public static void UnlockFullLifeLvl2Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FULL_LIFE_LVL2_ACHIEVEMENT");
    }
    public static void UnlockFullLifeLvl3Achievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FULL_LIFE_LVL3_ACHIEVEMENT");
    }
    public static void UnlockFullLifeBfAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FULL_LIFE_BF_ACHIEVEMENT");
    }
    public static void UnlockSpotDuskoAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPOT_DUSKO_ACHIEVEMENT");
    }
    public static void UnlockSpotGljivanAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPOT_GLJIVAN_ACHIEVEMENT");
    }
    public static void UnlockSpotSvjetlanaAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPOT_SVJETLANA_ACHIEVEMENT");
    }
    public static void UnlockSpotBrunoAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPOT_BRUNO_ACHIEVEMENT");
    }
    public static void UnlockSpotBlackHoleAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPOT_BLACK_HOLE_ACHIEVEMENT");
    }
    public static void UnlockEnterBlackHoleAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ENTER_BLACK_HOLE_ACHIEVEMENT");
    }
    public static void UnlockHelpGljivanAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("JUMP_GLJIVAN_ACHIEVEMENT");
    }
    public static void UnlockDamageGljivanAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DAMAGE_GLJIVAN_ACHIEVEMENT");
    }
    public static void UnlockBrunoDamageAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("BRUNO_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockBrunoKillAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("BRUNO_KILL_ACHIEVEMENT");
    }
    public static void UnlockDuskoDamageAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DUSKO_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockDuskoKillAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DUSKO_KILL_ACHIEVEMENT");
    }
    public static void UnlockSvjetlanaDamageAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SVJETLANA_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockSvjetlanaKillAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SVJETLANA_KILL_ACHIEVEMENT");
    }
    public static void UnlockCloudThroughAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("CLOUD_THROUGH_ACHIEVEMENT");
    }
    public static void UnlockGainLifeAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("GAIN_LIFE_ACHIVEMENT");
    }
    public static void UnlockLoseLifeAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("LOSE_LIFE_ACHIEVEMENT");
    }
    public static void UnlockEnterCheckpointAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("CHECKPOINT_ACHIEVEMENT");
    }
    public static void UnlockDieAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DIE_ACHIEVEMENT");
    }
    public static void UnlockFinishSpeedrunAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_ACHIEVEMENT");
    }
    public static void UnlockWatchCreditsAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("CREDITS_ACHIEVEMENT");
    }
    public static void UnlockPressPauseAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("PAUSE_ACHIEVEMENT");
    }
    public static void UnlockDieSpeedrunAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_DIE_ACHIEVEMENT");
    }
    public static void UnlockRestoreFullHealthAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("RESTORE_HEALTH_ACHIEVEMENT");
    }
    public static void UnlockPracticeDamageAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("PRACTICE_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockPracticeFinishAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("PRACTICE_FINISH_ACHIEVEMENT");
    }
    public static void UnlockDashAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DASH_ACHIEVEMENT");
    }
    public static void UnlockGameFinishAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("GAME_END_ACHIEVEMENT");
    }
    public static void UnlockEnterStardustAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("STAR_DUST_ACHIEVEMENT");
    }
    public static void UnlockUseTeleportAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("TELEPORT_ACHIEVEMENT");
    }
    public static void UnlockFinishLvl1Achievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FINISH_LVL1_ACHIEVEMENT");
    }
    public static void UnlockFinishLvl2Achievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FINISH_LVL2_ACHIEVEMENT");
    }
    public static void UnlockFinishLvl3Achievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("FINISH_LVL3_ACHIEVEMENT");
    }
    public static void UnlockSpotWizardAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("WIZARD_SPOT_ACHIEVEMENT");
    }
    public static void UnlockDamageWizardAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("WIZARD_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockHurtWizardAchievement()    ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("WIZARD_KILL_ACHIEVEMENT");
    }
    public static void UnlockMusicOffAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("MUSIC_OFF_ACHIEVEMENT");
    }
    public static void UnlockMusicFullAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("MUSIC_FULL_ACHIEVEMENT");
    }

}
