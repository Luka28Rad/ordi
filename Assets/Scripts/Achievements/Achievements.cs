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
            bool achievementUnlocked;
            // Check if the achievement is already unlocked
            SteamUserStats.GetAchievement(ach_ID, out achievementUnlocked);
            if (!achievementUnlocked)
            {
                SteamUserStats.SetAchievement (ach_ID);
                SteamUserStats.StoreStats();
                Debug.Log("Achievement " + ach_ID + " unlocked!");
            } else {
               // Debug.Log("Achievement " + ach_ID + " already unlocked.");
            }
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
        if(Variables.gameMode == "Speedrun") SteamStatsManager.Instance.IncrementStat("KillsSR");
        if(Variables.gameMode == "NewGame" || Variables.gameMode == "LoadGame") SteamStatsManager.Instance.IncrementStat("KillsSP");
    }
    public static void UnlockDuskoDamageAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DUSKO_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockDuskoKillAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DUSKO_KILL_ACHIEVEMENT");
        if(Variables.gameMode == "Speedrun") SteamStatsManager.Instance.IncrementStat("KillsSR");
        if(Variables.gameMode == "NewGame" || Variables.gameMode == "LoadGame") SteamStatsManager.Instance.IncrementStat("KillsSP");
    }
    public static void UnlockSvjetlanaDamageAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SVJETLANA_DAMAGE_ACHIEVEMENT");
    }
    public static void UnlockSvjetlanaKillAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SVJETLANA_KILL_ACHIEVEMENT");
        if(Variables.gameMode == "Speedrun") SteamStatsManager.Instance.IncrementStat("KillsSR");
        if(Variables.gameMode == "NewGame" || Variables.gameMode == "LoadGame") SteamStatsManager.Instance.IncrementStat("KillsSP");
    }
    public static void UnlockCloudThroughAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("CLOUD_THROUGH_ACHIEVEMENT");
    }
    public static void UnlockGainLifeAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("GAIN_LIFE_ACHIEVEMENT");
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
        SteamStatsManager.Instance.IncrementStat("WinsSR");
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
        SteamStatsManager.Instance.IncrementStat("WinsSP");
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

    public static void UnlockBeatZvjezdanAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_ZVJEZDAN_ACHIEVEMENT");
    }

    public static void UnlockBeatSpidersAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_SPIDERS_ACHIEVEMENT");
    }

    public static void UnlockBeatGljivanAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_GLJIVAN_ACHIEVEMENT");
    }

    public static void UnlockBeatDuskoAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_DUSKO_ACHIEVEMENT");
    }

    public static void UnlockBeatSvjetlanaAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_SVJETLANA_ACHIEVEMENT");
    }

    public static void UnlockBeatBrunoAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_BRUNO_ACHIEVEMENT");
    }

    public static void UnlockBeatDarkoAchievement() ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_DARKO_ACHIEVEMENT");
    }

    public static void UnlockBeatNestaskoAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_NESTASKO_ACHIEVEMENT");
    }

    public static void UnlockBeatZirAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_ZIR_ACHIEVEMENT");
    }

    public static void UnlockBeatYourselfAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_USER_ACHIEVEMENT");
    }

        public static void UnlockBeatCrnaRupaAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_BLACKHOLE_ACHIEVEMENT");
    }

    public static void UnlockBeatEveryoneAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("SPEEDRUN_EVERYONE_ACHIEVEMENT");
    }

    public static void UnlockCheckLeaderboardsAchievement(){    ///////////////////////////////////////////////////////////////////////
        SetAchievement("CHECK_LEADERBOARDS_ACHIEVEMENT");
    }

    public static void UnlockTileTenAchievement(){      ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_10_ACHIEVEMENT");
    }

    public static void UnlockTile51Achievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_51_ACHIEVEMENT");
    }

    public static void UnlockTile101Achievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_101_ACHIEVEMENT");
    }

    public static void UnlockTile66Achievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_66_ACHIEVEMENT");
    }
    public static void UnlockTile115Achievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_115_ACHIEVEMENT");
    }
    public static void UnlockendlessDeathAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_DEATH_ACHIEVEMENT");
    }

    public static void UnlockendlessRunAwayAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_RUNAWAY_ACHIEVEMENT");
    }

    public static void UnlockStartEndlessAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENDLESS_START_ACHIEVEMENT");
    }

    public static void UnlockMultiplayerStartAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("MULTIPLAYER_START_ACHIEVEMENT");
    }

    public static void UnlockMultiplayerEndAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("MULTIPLAYER_END_ACHIEVEMENT");
    }

    public static void UnlockMultiplayerAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("TRUE_MULTIPLAYER_ACHIEVEMENT");
    }

    public static void UnlockMultiplayerHostAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("MULTIPLAYER_HOST_ACHIEVEMENT");
    }

    public static void UnlockMultiplayerClientAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("MULTIPLAYER_CLIENT_ACHIEVEMENT");
    }

    public static void UnlockDemoAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("DEMO_FINISH_ACHIEVEMENT");
    }

    public static void UnlockStatsAchievement(){   ///////////////////////////////////////////////////////////////////////
        SetAchievement("CHECK_STATS_ACHIEVEMENT");
    }

    public static void UnlockFirstEpisodeAchievement(){     ///////////////////////////////////////////////////////////////////////
        SetAchievement("FIRST_COMIC_ACHIEVEMENT");
    }

    public static void UnlockHalfComicAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("HALF_COMIC_ACHIEVEMENT");
    }

    public static void UnlockReadComicAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("FULL_COMIC_ACHIEVEMENT");
    }

    public static void UnlockRedGreenSpiderSpotted(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("RED_GREEN_SPIDER_ACHIEVEMENT");
    }

    public static void UnlockBlackYellowSpiderSpotted(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("BLACK_YELLOW_SPIDER_ACHIEVEMENT");
    }

    public static void UnlockRedGreenSpiderDamage(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("RED_GREEN_SPIDER_DAMAGE_ACHIEVEMENT");
    }

    public static void UnlockBlackYellowSpiderDamage(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("BLACK_YELLOW_SPIDER_DAMAGE_ACHIEVEMENT");
    }

    public static void UnlockSurviveFourBites(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("ALL_SPIDERS_ACHIEVEMENT");
    }

    public static void UnlockEnterWeb(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("ENTER_WEB_ACHIEVEMENT");
    }

    public static void UnlockEnterAllWebs(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("ALL_WEBS_ACHIEVEMENT");
    }

    public static void UnlockDieStoryModeAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("DEATH_STORY_ACHIEVEMENT");
    }

    public static void UnlockRespawnAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("RESPAWN_ACHIEVEMENT");
    }

    public static void UnlockLoadGameAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("LOAD_GAME_ACHIEVEMENT");
    }

    public static void UnlockGljivanSoundsAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("GLJIVAN_SOUNDS_ACHIEVEMENT");
    }

    public static void UnlockPlayEveryDayAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("EVERY_DAY_ACHIEVEMENT");
    }

    public static void UnlockMazeRunnerAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("MAZE_RUNNER_ACHIEVEMENT");
    }

    public static void UnlockObstacleCourseAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("OBSTACLE_COURSE_ACHIEVEMENT");
    }
    public static void UnlockJumpingJackAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("JUMPING_JACK_ACHIEVEMENT");
    }

    public static void UnlockStickyKeysAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("STICKY_KEYS_ACHIEVEMENT");
    }


}
