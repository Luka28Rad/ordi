using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class NetworkedEndlessTiles : NetworkBehaviour
{
    public Tilemap tilemap;
    public Tilemap borderTileMap;
    public TileBase tile;
    
    [Header("Platform Generation Settings")]
    public float rowMinDistance = 3f;
    public float rowMaxDistance = 3f;
    public int minPlatformLength = 1;
    public int maxPlatformLength = 4;

    private float borderTopHeight;
    private float currentHeight;
    
    // Reference to the CustomNetworkManager
    private CustomNetworkManager networkManager;

    [SyncVar]
    private float highestPlayerY;

    private int prevStart = -50;
    private int prevLength = -50;
private PlayerSpawnManager spawnManager;

        public override void OnStartServer()
    {

        base.OnStartServer();
        networkManager = (CustomNetworkManager)NetworkManager.singleton;
        spawnManager = FindObjectOfType<PlayerSpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.OnAllPlayersReady += StartSpawning;
        }
        enabled = false; // Disable spawning until players are ready
    }
        [Server]
    private void StartSpawning()
    {
        // Initialize on server
        currentHeight = -5;
        borderTopHeight = -11;
        
        // Initial platform generation up to height 30
        
        ExtendBorders(borderTopHeight);
        Debug.Log("All players are ready. Starting platform spawning...");
        enabled = true; // Enable platform spawning
    }

    private void Update()
    {
        if (!isServer) return;

        // Generate new platforms if needed
        UpdateHighestPlayerPosition();
        
        if (highestPlayerY > currentHeight - 2)
        {
            currentHeight += Random.Range(rowMinDistance, rowMaxDistance);
            SpawnRow(currentHeight);
        }

        // Extend borders if needed
        if (currentHeight + 10 > borderTopHeight)
        {
            ExtendBorders(borderTopHeight);
        }
    }

    private void UpdateHighestPlayerPosition()
    {
        if (networkManager == null) return;

        float maxHeight = float.MinValue;
        foreach (PlayerObjectController player in networkManager.GamePlayers)
        {
            if (player != null && player.gameObject != null)
            {
                float playerHeight = player.gameObject.transform.GetChild(0).GetChild(1).transform.position.y;
                maxHeight = Mathf.Max(maxHeight, playerHeight);
                //Debug.Log("AAA "+ player.name);
            }
        }
        if (maxHeight != float.MinValue)
        {
            highestPlayerY = maxHeight;
        }
    }

    [Server]
    private void SpawnRow(float y)
    {
        int platformLength = Random.Range(minPlatformLength, maxPlatformLength + 1);
        if (platformLength == prevLength)
        {
            do
            {
                platformLength = Random.Range(minPlatformLength, maxPlatformLength + 1);
            } while (platformLength == prevLength);
        }
        prevLength = platformLength;

        int currentX = Random.Range(-5, 5 - platformLength + 1);
        if (prevStart == currentX)
        {
            do
            {
                currentX = Random.Range(-5, 5 - platformLength + 1);
            } while (prevStart == currentX);
        }
        prevStart = currentX;

        // Call the RPC to spawn tiles on all clients
        RpcSpawnTiles(currentX, platformLength, Mathf.FloorToInt(y));
    }

    [ClientRpc]
    private void RpcSpawnTiles(int startX, int length, int y)
    {
        for (int x = startX; x < startX + length; x++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), tile);
        }
    }

    [Server]
    private void ExtendBorders(float y)
    {
        for (int i = Mathf.FloorToInt(y); i < y + 10; i++)
        {
            RpcExtendBorders(Mathf.FloorToInt(borderTopHeight));
            borderTopHeight++;
        }
    }

    [ClientRpc]
    private void RpcExtendBorders(int height)
    {
        borderTileMap.SetTile(new Vector3Int(-6, height, 0), tile);
        borderTileMap.SetTile(new Vector3Int(6, height, 0), tile);
    }
}