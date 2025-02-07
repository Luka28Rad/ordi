using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class EndlessTiles : MonoBehaviour
{
    public TMP_Text scoreText;
    public Tilemap tilemap;
    public Tilemap borderTileMap;
    public TileBase tile;
    public GameObject player;
    public GameObject prefabToSpawn;  // Reference to the prefab to spawn
    public GameObject starDust;  // Reference to the prefab to spawn
    public float rowMinDistance = 3f;
    public float rowMaxDistance = 3f;
    public int minPlatformLength = 1;
    public int maxPlatformLength = 4;

    public static int score = 0;
    private float borderTopHeight; 
    private float currentHeight;
    private float lastPlayerHeight;

    private void Start()
    {
        Achievements.UnlockStartEndlessAchievement();
        score = 0;
        scoreText.text = "";
        currentHeight = 0;
        SpawnRow(currentHeight);
        borderTopHeight = -2;
        ExtendBorders(borderTopHeight);

        lastPlayerHeight = player.transform.position.y;
        //UpdateScoreText();
    }

    private void Update()
    {
        if(player == null) return;
        if (player.transform.position.y > currentHeight - 2)
        {
            currentHeight += Random.Range(rowMinDistance, rowMaxDistance);
            SpawnRow(currentHeight);

            score++;
            CheckTrophies(score);
            UpdateScoreText();
        }

        if (player.transform.position.y - lastPlayerHeight >= 6)
        {
            Debug.Log("Combo!");
            lastPlayerHeight = player.transform.position.y;
        }

        if (currentHeight + 10 > borderTopHeight)
        {
            ExtendBorders(borderTopHeight);
        }

        // Check if the 'R' key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnPrefabsInCurrentRow();
        }
    }

    int prevStart = -50;
    int prevLength = -50;
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

        if(Mathf.FloorToInt(y)%25 == 0 && platformLength>1) {SpawnStarDust(new Vector3Int(currentX+1, Mathf.FloorToInt(y+1), 0));}
        for (int x = currentX; x < currentX + platformLength; x++)
        {
            tilemap.SetTile(new Vector3Int(x, Mathf.FloorToInt(y), 0), tile);
        }
    }
    public DangerLine dangerLine;

    private void SpawnStarDust(Vector3 vec) {
        //Vector3 spawnPosition = tilemap.CellToWorld(new Vector3Int(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y+2), 0));
        //Instantiate(starDust, vec, Quaternion.identity);
        // vise smetaju nego sto pomazu
    }

    private void SpawnPrefabsInCurrentRow()
    {
        // Loop through each tile position in the current row
            //Vector3 spawnPosition = tilemap.CellToWorld(new Vector3Int(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y+2), 0));
            //Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    private void ExtendBorders(float y)
    {
        for (int i = Mathf.FloorToInt(y); i < y + 10; i++)
        {
            borderTileMap.SetTile(new Vector3Int(-6, Mathf.FloorToInt(borderTopHeight), 0), tile);
            borderTileMap.SetTile(new Vector3Int(6, Mathf.FloorToInt(borderTopHeight), 0), tile);
            borderTopHeight++;
        }
    }

    private void UpdateScoreText()
    {   if(score%25 == 0) dangerLine.IncreaseSpeed();
        scoreText.text = score.ToString();
    }
    private void CheckTrophies(int number){
        if(number > 101 || number < 10) return;
        if(number == 10) Achievements.UnlockTileTenAchievement();
        else if(number == 51) Achievements.UnlockTile51Achievement();
        else if(number == 66) Achievements.UnlockTile66Achievement();
        else if(number == 101) Achievements.UnlockTile101Achievement();
    }
}
