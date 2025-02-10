using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpCharCounter : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject locPlayer;
    void Start()
    {
        locPlayer = GameObject.Find("LocalGamePlayer");
    }

    private bool sentData = false;
    // Update is called once per frame
    void Update()
    {
        if(locPlayer == null) locPlayer = GameObject.Find("LocalGamePlayer");
        if(locPlayer != null && !sentData) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length >= 2) {
                Achievements.UnlockMultiplayerAchievement();
                Debug.Log("IMA NAS "+ players.Length);
            } else {
                Debug.Log("NEMA NAS "+ players.Length);
            }
            Achievements.UnlockMultiplayerStartAchievement();
            sentData = true;
            string name = locPlayer.transform.GetChild(0).transform.GetChild(1).GetComponent<SpriteRenderer>().sprite.name;
            SendStatUpdate(name);
        }
    }

    private void SendStatUpdate(string name){
        Debug.Log("Sending data " + name);
        SteamStatsManager.Instance.IncrementStat("GamesMP");
        name=name.ToLower();
            if(name.Contains("springshroom"))
            {
                SteamStatsManager.Instance.IncrementStat("GljivanMP");
            }
            else if(name.Contains("dusko"))
            {
                SteamStatsManager.Instance.IncrementStat("DuskoMP");
            }
            else if(name.Contains("wizzy"))
            {
                SteamStatsManager.Instance.IncrementStat("WizzyMP");
            } else if(name.Contains("matchstick")){
                SteamStatsManager.Instance.IncrementStat("BrunoMP");
            } else if(name.Contains("svijeca")){
                SteamStatsManager.Instance.IncrementStat("SvjetlanaMP");
            } else SteamStatsManager.Instance.IncrementStat("ZvjezdanMP");
    }
}
