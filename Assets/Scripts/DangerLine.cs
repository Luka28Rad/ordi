using UnityEngine;
using TMPro;  // Ensure TextMeshPro namespace is included

public class DangerLine : MonoBehaviour
{
    public float startSpeed = 1f;            // Initial speed of the line 
    public GameObject player;                 // Reference to the player object
    public TMP_Text distanceText;             // TextMeshPro text to display distance

    private float currentSpeed;                // Current speed of the line
    public float speedIncreaseAmount = 50f;    // Amount to increase the speed by
    private bool alive = true;

    private void Start()
    {
        // Set initial speed
        currentSpeed = startSpeed;
    }

    private void Update()
    {
        if(alive){
        MoveDangerLine();

        UpdateDistanceText();
        } else{
            if(!set) {
            gameOverPanel.SetActive(true);
            set = true;
            DisplayData();
            }
        }
    }
    public TMP_Text currentRecord;
    public TMP_Text previousRecordText;
    public TMP_Text infoText;

    private void DisplayData() {
        long score = SteamLeaderboardDisplay.userPreviousRecordEndless;
        Debug.Log("PROSLI " +score);
        if(score==-404){
            previousRecordText.text = "No previous record!";
        }else if(score <0) {
        previousRecordText.text = "Error loading previous record..";
        } else {
            previousRecordText.text = "Previous record:\n" + score.ToString();
        }
        currentRecord.text = "Current tiles:\n" + EndlessTiles.score.ToString();
    }

    public void ContinueButton() {
        try{
        SteamLeaderboardManager.UpdateScoreEndless(EndlessTiles.score);

        } catch(System.Exception e) {
            infoText.text = "Error saving data try again..";
            Debug.Log(e);
            return;
        }
        btn1ToShow.SetActive(true);
        btn2ToShow.SetActive(true);
        btnToHide.SetActive(false);
    }
    public GameObject btn1ToShow;
    public GameObject btn2ToShow;
    public GameObject btnToHide;
    private bool set = false;
    public GameObject gameOverPanel;
    private int heighest  = 0;
    private void MoveDangerLine()
    {
        // Calculate the distance from the player score
        float playerScoreY = player.transform.position.y;
        float dangerLineY = transform.position.y;
        if(playerScoreY > heighest) heighest = (int)playerScoreY;
        // If the danger line is more than 30 units away from the player's score, adjust its position
        if (Mathf.Abs(dangerLineY - playerScoreY) > 30f)
        {
            if (dangerLineY < playerScoreY - 30f)
            {
                // Move the danger line to be 30 units below the player's score
                transform.position = new Vector3(transform.position.x, playerScoreY - 30f, transform.position.z);
            }
            else if (dangerLineY > playerScoreY + 30f)
            {
                // Move the danger line to be 30 units above the player's score
                transform.position = new Vector3(transform.position.x, playerScoreY + 30f, transform.position.z);
            }
        }
        else
        {
            // Move the line upwards normally
            transform.position += Vector3.up * currentSpeed * Time.deltaTime;
        }
    }

    public void IncreaseSpeed()
    {
        currentSpeed *= (1+speedIncreaseAmount / 100); // Increase the current speed by the specified amount
        if (currentSpeed > 5f) currentSpeed = 5f;    // Cap the speed at 5
        Debug.Log("Current Speed: " + currentSpeed);
    }

    private void UpdateDistanceText()
    {
        // Calculate vertical distance between the line and the player
        float distance = player.transform.position.y - transform.position.y;
        distance = distance / 3; // Adjust based on platform height
        // Update the TextMeshPro text with the current distance
        distanceText.text = distance.ToString("F0"); // "F1" for one decimal place
        if(distanceText.text == "10") {Achievements.UnlockendlessRunAwayAchievement();}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the line collides with the player
        if (other.gameObject == player)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        // Add your player death logic here, such as:
        Debug.Log("Player has been killed by the danger line!");
        Destroy(player);
        alive = false;
        Achievements.UnlockendlessDeathAchievement();
        // Optionally, trigger a respawn, end the game, etc.
    }
}
