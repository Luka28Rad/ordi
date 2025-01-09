using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;
    public TMP_Text LobbyNameText;
    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;
    public ulong CurrentLobbyId;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    public PlayerObjectController LocalPlayerController;
    private CustomNetworkManager manager;
    public Button StartGameButton;
    public Button readyButton;
    public TMP_Text ReadyButtonText;
    public Sprite readySprite, unreadySprite, readyHoverSprite, unreadyHoverSprite;
    private CustomNetworkManager Manager {
        get{
            if(manager != null){
                return manager;
            } 
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public void StartGame(string sceneName) {
        LocalPlayerController.CanStartGame(sceneName);
    }
    void Awake() {
        if(Instance == null) {Instance = this;}
        Debug.Log("Wakeup");
    }

    public void ReadyPlayer(){
        LocalPlayerController.ChangeReady();
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            // This is the host
            Debug.Log("Host is leaving the lobby...");
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            // This is a client
            Debug.Log("Client is leaving the lobby...");
            NetworkManager.singleton.StopClient();
        }
        else
        {
            Debug.LogWarning("No active network session to leave.");
        }

        // Clean up UI and reset local states
        ResetLobbyUI();
    }

    private void ResetLobbyUI()
    {
        // Example: Resetting lobby UI to default state
        Debug.Log("Resetting lobby UI...");
        // Add your UI reset logic here
    }

    public void UpdateButton(){
        if(LocalPlayerController.Ready == true){
            ReadyButtonText.text = "";//"Unready";

            readyButton.GetComponent<Image>().sprite = unreadySprite;

            SpriteState spriteState = readyButton.spriteState;
            spriteState.highlightedSprite = unreadyHoverSprite;
            readyButton.spriteState = spriteState;
        } else {
            ReadyButtonText.text = "";//"Ready";

            readyButton.GetComponent<Image>().sprite = readySprite;

            SpriteState spriteState = readyButton.spriteState;
            spriteState.highlightedSprite = readyHoverSprite;
            readyButton.spriteState = spriteState;
        }
        if (EventSystem.current.currentSelectedGameObject == readyButton.gameObject)
                EventSystem.current.SetSelectedGameObject(null);
    }

    public void CheckIfAllReady(){
        bool allReady = false;
        foreach(PlayerObjectController player in Manager.GamePlayers) {
            if(player.Ready){
                allReady = true;
            } else {
                allReady = false;
                break;
            }
        }

        if(allReady) {
            if(LocalPlayerController && LocalPlayerController.PlayerIdNumber == 1) { // && LocalPlayerController.PlayerIdNumber != null
                StartGameButton.interactable = true;
            } else {
                StartGameButton.interactable = false;
            }
        } else {
            StartGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName(){
        CurrentLobbyId = Manager.GetComponent<SteamLobby>().CurrentLobbyId;
        LobbyNameText.text =  SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), "name");
    }

    public void UpdatePlayerList(){
        if(!PlayerItemCreated) {
            CreateHostPlayerItem();
        }
        if(PlayerListItems.Count < Manager.GamePlayers.Count) {CreateClientPlayerItem();}
        if(PlayerListItems.Count > Manager.GamePlayers.Count ) {RemovePlayerItem();}
        if(PlayerListItems.Count == Manager.GamePlayers.Count) {UpdatePlayerItem();}
    }

    public void FindLocalPlayer() {
        LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>() ;
        LocalPlayerObject.transform.position = new Vector3(0f,0f,0f);
        
        Transform playerTransform = LocalPlayerObject.transform.Find("Player");
        if (playerTransform == null) {
            Debug.LogError("'Player' child object not found under LocalGamePlayer.");
            return;
        }

        // Find the "Zvjezdan" child GameObject under "Player"
        Transform zvjezdanTransform = playerTransform.Find("Zvjezdan");
        if (zvjezdanTransform == null) {
            Debug.LogError("'Zvjezdan' child object not found under 'Player'.");
            return;
        }

        // Set the position of "Zvjezdan"
        zvjezdanTransform.position = new Vector3(0f, 0f, 0f);
        zvjezdanTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void CreateHostPlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers) {
            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionId = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamId = player.PlayerSteamId;
            NewPlayerItemScript.ready = player.Ready;
            NewPlayerItemScript.SetPlayersValues();

            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers) {
            if(!PlayerListItems.Any(b =>b.ConnectionId == player.ConnectionID)) {
                 GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

            NewPlayerItemScript.PlayerName = player.PlayerName;
            NewPlayerItemScript.ConnectionId = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamId = player.PlayerSteamId;
            NewPlayerItemScript.ready = player.Ready;
            NewPlayerItemScript.SetPlayersValues();

            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(NewPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem(){
        foreach(PlayerObjectController player in Manager.GamePlayers) {
            foreach(PlayerListItem PlayerListItemScript in PlayerListItems) {
                if(PlayerListItemScript.ConnectionId == player.ConnectionID){
                    PlayerListItemScript.PlayerName = player.PlayerName;
                    PlayerListItemScript.ready = player.Ready;
                    PlayerListItemScript.SetPlayersValues();
                    if(LocalPlayerController == player){
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }
    public void RemovePlayerItem(){
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();
        foreach(PlayerListItem playerListItem in PlayerListItems) {
            if(!Manager.GamePlayers.Any(b => b.ConnectionID == playerListItem.ConnectionId)) {
                playerListItemToRemove.Add(playerListItem);
            }
        }
        if(playerListItemToRemove.Count > 0) {
            foreach(PlayerListItem playerlistItemToRemove in playerListItemToRemove) {
                GameObject ObjectToRemove = playerlistItemToRemove.gameObject;
                PlayerListItems.Remove(playerlistItemToRemove);
                Destroy(ObjectToRemove);
                ObjectToRemove = null;
            }
        }
    }
}
