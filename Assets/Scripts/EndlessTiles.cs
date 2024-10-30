using UnityEngine;
using UnityEngine.Tilemaps;

public class EndlessTiles : MonoBehaviour
{
    public Tilemap tilemap;            // Tilemap to place tiles on
    public Tilemap borderTileMap;            // Tilemap to place tiles on
    public TileBase tile;              // Tile to be used for platforms
    public GameObject player;          // Reference to the player object
    public float rowMinDistance = 3f;  // Minimum vertical distance between rows
    public float rowMaxDistance = 3f;  // Maximum vertical distance between rows
    public int minPlatformLength = 1;   // Minimum platform length
    public int maxPlatformLength = 4;   // Maximum platform length
    private float borderTopHeight; 
    private float currentHeight;        // Current Y position for the next row

    private void Start()
    {
        // Initialize the first row
        currentHeight = 0; // Start at height 0
        SpawnRow(currentHeight);
        borderTopHeight = -2;
        ExtendBorders(borderTopHeight);
    }

    private void Update()
    {
        // The player is always moving upward, and we check for row updates
        if (player.transform.position.y > currentHeight-2) // Check if player is sufficiently high
        {
            // Update the height for the next row
            currentHeight += Random.Range(3,3);
            SpawnRow(currentHeight);
        }
    // Extend the border tiles if currentHeight is near the top of the existing border
            if (currentHeight + 10 > borderTopHeight)
            {
                ExtendBorders(borderTopHeight);
            }
    }

    int prevStart = -50;
    int prevLength = -50;
    private void SpawnRow(float y)
    {
        // Determine random platform length between 3 and 7
        int platformLength = Random.Range(minPlatformLength, maxPlatformLength + 1); // +1 to include max
        if(platformLength == prevLength) {
            do{
            platformLength = Random.Range(minPlatformLength, maxPlatformLength + 1);
            } while(platformLength == prevLength);
        }
        prevLength = platformLength;

        // Determine a random starting position for the platform within the range of -10 to +10
        int currentX = Random.Range(-5, 5 - platformLength + 1); // Ensure platform fits within range
        if(prevStart == currentX) {
            do {
                currentX = Random.Range(-5, 5 - platformLength + 1);
            } while(prevStart == currentX);
        } 
        prevStart = currentX;
        // Spawn the platform by setting tiles
        for (int x = currentX; x < currentX + platformLength; x++)
        {
            tilemap.SetTile(new Vector3Int(x, Mathf.FloorToInt(y), 0), tile);
        }
    }

    private void ExtendBorders(float y)
    {
        for(int i =Mathf.FloorToInt(y); i<y+10;i++) {
        // Place border tiles at x = -5 and x = +5 for the specified y row
        borderTileMap.SetTile(new Vector3Int(-6, Mathf.FloorToInt(borderTopHeight), 0), tile);
        borderTileMap.SetTile(new Vector3Int(6, Mathf.FloorToInt(borderTopHeight), 0), tile);
        borderTopHeight++;
        }
    }
}
