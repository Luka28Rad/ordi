using UnityEngine;
using Mirror;
using TMPro;

public class WinnerPanelController : NetworkBehaviour
{
    [Header("Player Position UI References")]
    [SerializeField] private TMP_Text firstPlayerName;
    [SerializeField] private TMP_Text secondPlayerName;
    [SerializeField] private TMP_Text thirdPlayerName;
    [SerializeField] private TMP_Text fourthPlayerName;
    
    [Header("Panel Reference")]
    [SerializeField] private GameObject winnerPanel;
    
    private readonly SyncList<PlayerData> playerDeathOrder = new SyncList<PlayerData>();

    public struct PlayerData
    {
        public string playerName;
        public uint netId;

        public PlayerData(string name, uint id)
        {
            playerName = name;
            netId = id;
        }
    }

    private void Start()
    {
        if (winnerPanel != null)
            winnerPanel.SetActive(false);
        ClearAllPlayerNames();
    }

    private void ClearAllPlayerNames()
    {
        if (firstPlayerName != null) firstPlayerName.text = "";
        if (secondPlayerName != null) secondPlayerName.text = "";
        if (thirdPlayerName != null) thirdPlayerName.text = "";
        if (fourthPlayerName != null) fourthPlayerName.text = "";
    }

    [Server]
    public void AddPlayerToDeathOrder(string playerName, uint netId)
    {
        if (!PlayerExistsInDeathOrder(netId))
        {
            playerDeathOrder.Add(new PlayerData(playerName, netId));
            //RpcUpdateWinnerPanel();
        }
    }

    private bool PlayerExistsInDeathOrder(uint netId)
    {
        foreach (var player in playerDeathOrder)
        {
            if (player.netId == netId)
                return true;
        }
        return false;
    }

    [ClientRpc]
    private void RpcUpdateWinnerPanel()
    {
        if (winnerPanel != null)
            winnerPanel.SetActive(true);

        UpdatePlayerPositions();
    }

    private void UpdatePlayerPositions()
    {
        ClearAllPlayerNames();

        int lastIndex = playerDeathOrder.Count - 1;
        for (int i = 0; i <= lastIndex; i++)
        {
            string playerName = playerDeathOrder[i].playerName;
            
            switch (lastIndex - i)
            {
                case 3:
                    if (fourthPlayerName != null)
                        fourthPlayerName.text = playerName;
                    break;
                case 2:
                    if (thirdPlayerName != null)
                        thirdPlayerName.text = playerName;
                    break;
                case 1:
                    if (secondPlayerName != null)
                        secondPlayerName.text = playerName;
                    break;
                case 0:
                    if (firstPlayerName != null)
                        firstPlayerName.text = playerName;
                    break;
            }
        }
    }

    public void ShowFinalResults()
    {
        if (winnerPanel != null)
            winnerPanel.SetActive(true);
        
        UpdatePlayerPositions();
    }
}