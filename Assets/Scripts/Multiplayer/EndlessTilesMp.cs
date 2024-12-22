using UnityEngine;
using UnityEngine.Tilemaps;
using Mirror;

public class EndlessTilesMp : NetworkBehaviour
{/*
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
    
    // SyncVar to track the highest player position
    [SyncVar]
    private float highestPlayerY;

    // Keep track of the last generated platform position
    private int prevStart = -50;
    private int prevLength = -50;

    public override void OnStartServer()
    {
        // Initialize on server
        currentHeight = 0;
        borderTopHeight = -2;
        
        // Initial platform generation up to height 30
        for (float height = 0; height <= 30; height += Random.Range(rowMinDistance, rowMaxDistance))
        {
            SpawnRow(height);
        }
        
        ExtendBorders(borderTopHeight);
    }

    private void Update()
    {
        if (!isServer) return;

        // Find the highest player
        UpdateHighestPlayerPosition();

        // Generate new platforms if needed
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
        float maxHeight = float.MinValue;
        foreach (NetworkGamePlayer player in NetworkManager.singleton.conn)
        {
            if (player != null && player.playerObject != null)
            {
                float playerHeight = player.playerObject.transform.position.y;
                maxHeight = Mathf.Max(maxHeight, playerHeight);
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
    }*/
}