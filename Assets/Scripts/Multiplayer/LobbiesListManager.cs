using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager instance;
    public GameObject lobbiesMenu;
    public GameObject lobbyListItemPrefab;
    public GameObject lobbyListContent;
    public GameObject lobbiesButton, hostButton, backButton;
    public List<GameObject> listOfLobbies = new List<GameObject>();

    private void Awake(){
        if(instance == null) instance = this;
    }

    public void GetListOfLobbies(){
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        lobbiesMenu.SetActive(true);
        backButton.SetActive(true);

        SteamLobby.Instance.GetLobbiesList();
    }

    public void BackButtonPressed(){
        lobbiesButton.SetActive(true);
        hostButton.SetActive(true);
        lobbiesMenu.SetActive(false);
        backButton.SetActive(false);
        DestroyLobbies();
    }

    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result){
        for(int i = 0; i< lobbyIDs.Count; i++){
            if(lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby) {
                GameObject newLobby = Instantiate(lobbyListItemPrefab);
                newLobby.GetComponent<LobbyDataEntry>().lobbyID = (CSteamID) lobbyIDs[i].m_SteamID;
                newLobby.GetComponent<LobbyDataEntry>().lobbyName = SteamMatchmaking.GetLobbyData((CSteamID) lobbyIDs[i].m_SteamID, "name");
                newLobby.GetComponent<LobbyDataEntry>().SetLobbyData();
                newLobby.transform.SetParent(lobbyListContent.transform);
                newLobby.transform.localScale = Vector3.one;
                listOfLobbies.Add(newLobby);

                RectTransform rectTransform = lobbyListContent.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    Vector2 size = rectTransform.sizeDelta;
                    size.y = listOfLobbies.Count * 88 + listOfLobbies.Count * 5 + 5;
                    rectTransform.sizeDelta = size;
                }
            }
        }
    }

    public void DestroyLobbies(){
        foreach(GameObject lobby in listOfLobbies){
            Destroy(lobby);
        }
        listOfLobbies.Clear();
    }
}
