using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using TMPro;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager instance;
    public GameObject lobbiesMenu;
    public GameObject lobbyListItemPrefab;
    public GameObject lobbyListContent;
    public GameObject lobbiesButton, hostButton, backButton, backToMainMenuButton;
    public TMP_Text numOfLobbiesText;
    public List<GameObject> listOfLobbies = new List<GameObject>();

    private void Awake(){
        if(instance == null) instance = this;
    }

    public void GetListOfLobbies(){
        DestroyLobbies();
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        backToMainMenuButton.SetActive(false);
        lobbiesMenu.SetActive(true);
        backButton.SetActive(true);
        numOfLobbiesText.gameObject.SetActive(true);

        SteamLobby.Instance.GetLobbiesList();
    }

    public void BackButtonPressed(){
        lobbiesButton.SetActive(true);
        hostButton.SetActive(true);
        backToMainMenuButton.SetActive(true);
        numOfLobbiesText.gameObject.SetActive(false);
        numOfLobbiesText.text = "";
        lobbiesMenu.SetActive(false);
        backButton.SetActive(false);
        DestroyLobbies();
    }
    public Sprite[] zvjezdice;
    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result){
        //DestroyLobbies();
            numOfLobbiesText.gameObject.SetActive(true);
        if(lobbyIDs.Count > 0) numOfLobbiesText.text = "Lobbies found: " + lobbyIDs.Count;
        else {
            numOfLobbiesText.text = "No lobbies found at the moment try again later. :(";
            return;
        }
        for(int i = 0; i< lobbyIDs.Count; i++){
            if(lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby && !listOfLobbies.Any(lobby => lobby.GetComponent<LobbyDataEntry>().lobbyID.m_SteamID == lobbyIDs[i].m_SteamID)) {
                if(lobbyIDs[i].m_SteamID == SteamLobby.Instance.CurrentLobbyId) Debug.Log("blokic");
                GameObject newLobby = Instantiate(lobbyListItemPrefab);
                newLobby.GetComponent<LobbyDataEntry>().lobbyID = (CSteamID) lobbyIDs[i].m_SteamID;
                newLobby.GetComponent<LobbyDataEntry>().lobbyName = SteamMatchmaking.GetLobbyData((CSteamID) lobbyIDs[i].m_SteamID, "name");
                newLobby.GetComponent<LobbyDataEntry>().SetLobbyData();
                newLobby.transform.SetParent(lobbyListContent.transform);
                newLobby.transform.localScale = Vector3.one;
                newLobby.transform.Find("LobbyImage").GetComponent<Image>().sprite = zvjezdice[i%3];
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
