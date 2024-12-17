using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar]public ulong PlayerSteamId;
    [SyncVar(hook=nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook=nameof(PlayerReadyUpdate))] public bool Ready;
    [SyncVar(hook=nameof(SendPlayerColor))] public int PlayerColor;
    private CustomNetworkManager manager;

    private void Start(){
        DontDestroyOnLoad(this.gameObject);
    }
    private void PlayerReadyUpdate(bool oldValue, bool newValue) {
        if(isServer){
            this.Ready = newValue;
        }
        if(isClient){
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CMdSetPlayerReady(){
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    public void ChangeReady(){
        if(isOwned){
            CMdSetPlayerReady();
        }
    }
    private CustomNetworkManager Manager {
        get{
            if(manager != null){
                return manager;
            } 
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority(){
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();

    }

    public override void OnStartClient(){
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient(){
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string playerName) {
        PlayerName = playerName;
    }
    public void PlayerNameUpdate(string OldValue, string NewValue) {
        if(isServer) {
            this.PlayerName = NewValue;
        }
        if(isClient){
            LobbyController.Instance.UpdatePlayerList();
        }
    }
    public void CanStartGame(string sceneName){
        if(isOwned){
            CmdCanStartGame(sceneName);
        }
    }
    [Command]
    public void CmdCanStartGame(string sceneName){
        manager.StartGame(sceneName);
    }

    [Command]
    public void CmdUpdatePlayerColor(int newValue){
        SendPlayerColor(PlayerColor, newValue);
    }

    public void SendPlayerColor(int oldValue, int newValue){
        if(isServer) {
            PlayerColor = newValue;
        }
        if(isClient && (oldValue != newValue)){
            UpdateColor(newValue);
        }
    }

    void UpdateColor(int message){
        PlayerColor = message;
    }
}
