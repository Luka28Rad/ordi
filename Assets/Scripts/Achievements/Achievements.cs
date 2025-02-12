using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private static readonly string[] AllAchievementIDs = new string[]
    {
        "WELCOME_ACHIEVEMENT",
        "FIRST_COIN_ACHIEVEMENT",
        "ALL_COINS_ACHIEVEMENT",
        "ALL_COINS_LVL1_ACHIEVEMENT",
        "ALL_COINS_LVL2_ACHIEVEMENT",
        "ALL_COINS_LVL3_ACHIEVEMENT",
        "ALL_COINS_BF_ACHIEVEMENT",
        "FULL_LIFE_LVL1_ACHIEVEMENT",
        "FULL_LIFE_LVL2_ACHIEVEMENT",
        "FULL_LIFE_LVL3_ACHIEVEMENT",
        "FULL_LIFE_BF_ACHIEVEMENT",
        "SPOT_DUSKO_ACHIEVEMENT",
        "SPOT_GLJIVAN_ACHIEVEMENT",
        "SPOT_SVJETLANA_ACHIEVEMENT",
        "SPOT_BRUNO_ACHIEVEMENT",
        "SPOT_BLACK_HOLE_ACHIEVEMENT",
        "ENTER_BLACK_HOLE_ACHIEVEMENT",
        "JUMP_GLJIVAN_ACHIEVEMENT",
        "DAMAGE_GLJIVAN_ACHIEVEMENT",
        "BRUNO_DAMAGE_ACHIEVEMENT",
        "BRUNO_KILL_ACHIEVEMENT",
        "DUSKO_DAMAGE_ACHIEVEMENT",
        "DUSKO_KILL_ACHIEVEMENT",
        "SVJETLANA_DAMAGE_ACHIEVEMENT",
        "SVJETLANA_KILL_ACHIEVEMENT",
        "CLOUD_THROUGH_ACHIEVEMENT",
        "GAIN_LIFE_ACHIEVEMENT",
        "LOSE_LIFE_ACHIEVEMENT",
        "CHECKPOINT_ACHIEVEMENT",
        "DIE_ACHIEVEMENT",
        "SPEEDRUN_ACHIEVEMENT",
        "CREDITS_ACHIEVEMENT",
        "PAUSE_ACHIEVEMENT",
        "SPEEDRUN_DIE_ACHIEVEMENT",
        "RESTORE_HEALTH_ACHIEVEMENT",
        "PRACTICE_DAMAGE_ACHIEVEMENT",
        "PRACTICE_FINISH_ACHIEVEMENT",
        "DASH_ACHIEVEMENT",
        "GAME_END_ACHIEVEMENT",
        "STAR_DUST_ACHIEVEMENT",
        "TELEPORT_ACHIEVEMENT",
        "FINISH_LVL1_ACHIEVEMENT",
        "FINISH_LVL2_ACHIEVEMENT",
        "FINISH_LVL3_ACHIEVEMENT",
        "WIZARD_SPOT_ACHIEVEMENT",
        "WIZARD_DAMAGE_ACHIEVEMENT",
        "WIZARD_KILL_ACHIEVEMENT",
        "MUSIC_OFF_ACHIEVEMENT",
        "MUSIC_FULL_ACHIEVEMENT",
        "SPEEDRUN_ZVJEZDAN_ACHIEVEMENT",
        "SPEEDRUN_SPIDERS_ACHIEVEMENT",
        "SPEEDRUN_GLJIVAN_ACHIEVEMENT",
        "SPEEDRUN_DUSKO_ACHIEVEMENT",
        "SPEEDRUN_SVJETLANA_ACHIEVEMENT",
        "SPEEDRUN_BRUNO_ACHIEVEMENT",
        "SPEEDRUN_DARKO_ACHIEVEMENT",
        "SPEEDRUN_NESTASKO_ACHIEVEMENT",
        "SPEEDRUN_ZIR_ACHIEVEMENT",
        "SPEEDRUN_USER_ACHIEVEMENT",
        "SPEEDRUN_BLACKHOLE_ACHIEVEMENT",
        "SPEEDRUN_EVERYONE_ACHIEVEMENT",
        "CHECK_LEADERBOARDS_ACHIEVEMENT",
        "ENDLESS_10_ACHIEVEMENT",
        "ENDLESS_51_ACHIEVEMENT",
        "ENDLESS_101_ACHIEVEMENT",
        "ENDLESS_66_ACHIEVEMENT",
        "ENDLESS_115_ACHIEVEMENT",
        "ENDLESS_DEATH_ACHIEVEMENT",
        "ENDLESS_RUNAWAY_ACHIEVEMENT",
        "ENDLESS_START_ACHIEVEMENT",
        "MULTIPLAYER_START_ACHIEVEMENT",
        "MULTIPLAYER_END_ACHIEVEMENT",
        "TRUE_MULTIPLAYER_ACHIEVEMENT",
        "MULTIPLAYER_HOST_ACHIEVEMENT",
        "MULTIPLAYER_CLIENT_ACHIEVEMENT",
        "DEMO_FINISH_ACHIEVEMENT",
        "CHECK_STATS_ACHIEVEMENT",
        "FIRST_COMIC_ACHIEVEMENT",
        "HALF_COMIC_ACHIEVEMENT",
        "FULL_COMIC_ACHIEVEMENT",
        "RED_GREEN_SPIDER_ACHIEVEMENT",
        "BLACK_YELLOW_SPIDER_ACHIEVEMENT",
        "RED_GREEN_SPIDER_DAMAGE_ACHIEVEMENT",
        "BLACK_YELLOW_SPIDER_DAMAGE_ACHIEVEMENT",
        "ALL_SPIDERS_ACHIEVEMENT",
        "ENTER_WEB_ACHIEVEMENT",
        "ALL_WEBS_ACHIEVEMENT",
        "DEATH_STORY_ACHIEVEMENT",
        "RESPAWN_ACHIEVEMENT",
        "LOAD_GAME_ACHIEVEMENT",
        "GLJIVAN_SOUNDS_ACHIEVEMENT",
        "EVERY_DAY_ACHIEVEMENT",
        "MAZE_RUNNER_ACHIEVEMENT",
        "OBSTACLE_COURSE_ACHIEVEMENT",
        "JUMPING_JACK_ACHIEVEMENT",
        "STICKY_KEYS_ACHIEVEMENT",
        "ALL_COINS_DEMO_ACHIEVEMENT",
        "DEMO_SPEED_ACHIEVEMENT",
        "DIE_DEMO_ACHIEVEMENT"
    };
    private void Start()
    {
        UnlockWelcomeAchievement();
        CheckAllAchievementsUnlocked();
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
                CheckAllAchievementsUnlocked();
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

    public static void UnlockAllCoinsDemoAchievement()  ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("ALL_COINS_DEMO_ACHIEVEMENT");
    }

    public static void UnlockDemoSpeedAchievement(){        ///////////////////////////////////////////////////////////////////////
        SetAchievement("DEMO_SPEED_ACHIEVEMENT");
    }
    public static void UnlockDieDemoAchievement()   ///////////////////////////////////////////////////////////////////////
    {
        SetAchievement("DIE_DEMO_ACHIEVEMENT");
    }

    public static void UnlockAllAchievement()
    {
        SetAchievement("ALL_ACHIEVEMENT");
    }

    private static void CheckAllAchievementsUnlocked()
    {
        if (SteamManager.Initialized)
        {
            bool allUnlocked = true;
            foreach (string achievementID in AllAchievementIDs)
            {
                bool achievementUnlocked;
                SteamUserStats.GetAchievement(achievementID, out achievementUnlocked);
                if (!achievementUnlocked)
                {
                    allUnlocked = false;
                    Debug.Log(achievementID + " NOT UNLOCKED YET.");
                    break;
                }
            }

            if (allUnlocked)
            {
                UnlockAllAchievement();
            }
        }
        else
        {
            Debug.LogError("Steamworks API is not initialized.");
        }
    }
}
