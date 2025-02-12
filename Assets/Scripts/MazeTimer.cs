using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MazeTimer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    private float detectionRadius = 2f;
    
    private bool hasStarted = false;
    private float startTime;
    private bool isComplete = false;
    private bool isInStartArea = false;
    private SpriteRenderer startPointSprite;
    private SpriteRenderer endPointSprite;
    private bool runWasSuccessful = false;
    private float completionTime = 0f;

    // Time thresholds for each level
    private readonly float[] levelTimeThresholds = { 9f, 26f, 4f, 4f, 2f };

    private void Start()
    {
        startPointSprite = startPoint.GetComponent<SpriteRenderer>();
        endPointSprite = endPoint.GetComponent<SpriteRenderer>();
        
        // Initialize colors
        if (startPointSprite != null)
        {
            startPointSprite.color = Color.red;
        }
        if (endPointSprite != null)
        {
            endPointSprite.color = Color.red;
        }
    }

    private void Update()
    {
        Vector3 playerPos = transform.position;
        bool currentlyInStartArea = IsAtPoint(playerPos, startPoint.position);
        
        // Handle start area entry
        if (currentlyInStartArea && !isInStartArea)
        {
            ResetTimer();
            isInStartArea = true;
        }
        // Handle start area exit - only start timer if it was reset
        else if (!currentlyInStartArea && isInStartArea)
        {
            if (!hasStarted)
            {
                StartTimer();
            }
            isInStartArea = false;
        }
        
        if (hasStarted && !isComplete && IsAtPoint(playerPos, endPoint.position))
        {
            CompleteRun();
        }

        UpdateColors(currentlyInStartArea);
    }
    
    private void UpdateColors(bool playerInStartArea)
    {
        // Start point color logic
        if (startPointSprite != null)
        {
            if (!hasStarted && playerInStartArea)
            {
                startPointSprite.color = Color.yellow;
            }
            else if (hasStarted)
            {
                startPointSprite.color = Color.green;
            }
            else
            {
                startPointSprite.color = Color.red;
            }
        }

        // End point color logic
        if (endPointSprite != null)
        {
            if (isComplete)
            {
                // Use completionTime instead of current time for the final color
                //int seconds = Mathf.FloorToInt(completionTime % 60f);
                bool withinTimeLimit = IsWithinTimeLimit(completionTime);
                endPointSprite.color = withinTimeLimit ? Color.green : Color.red;
                Debug.Log("Mjenjam");
            }
            else if (!hasStarted)
            {
                endPointSprite.color = Color.red;
            }
            else if(hasStarted)
            {
                // During active run
                float currentTime = Time.time - startTime;
                //int seconds = Mathf.FloorToInt(currentTime % 60f);
                bool withinTimeLimit = IsWithinTimeLimit(currentTime);
                endPointSprite.color = withinTimeLimit ? Color.yellow : Color.red;
            }
        }
    }
    
    private bool IsWithinTimeLimit(float seconds)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "Level 1": return seconds <= 9f;
            case "Level 2": return seconds <= 26f;
            case "Level 3": return seconds <= 4f;
            case "Bossfight": return seconds <= 4f;
            case "DemoLevel": return seconds <= 2f;
            default: return false;
        }
    }
    
    private bool IsAtPoint(Vector3 playerPos, Vector3 checkPoint)
    {
        return Vector3.Distance(playerPos, checkPoint) <= detectionRadius;
    }
    
    private void StartTimer()
    {
        hasStarted = true;
        startTime = Time.time;
        runWasSuccessful = false;
        completionTime = 0f;
        Debug.Log("Timer started!");
    }
    
    private void CompleteRun()
    {
        completionTime = Time.time - startTime;
        isComplete = true;
        hasStarted = false;
        runWasSuccessful = true;
        
        //int minutes = Mathf.FloorToInt(completionTime / 60f);
        //int seconds = Mathf.FloorToInt(completionTime % 60f);
        
        // Achievement checks
        if(completionTime <= 9f && SceneManager.GetActiveScene().name == "Level 1") Achievements.UnlockMazeRunnerAchievement();
        if(completionTime <= 26f && SceneManager.GetActiveScene().name == "Level 2") Achievements.UnlockObstacleCourseAchievement();
        if(completionTime <= 4f && SceneManager.GetActiveScene().name == "Level 3") Achievements.UnlockJumpingJackAchievement();
        if(completionTime <= 4f && SceneManager.GetActiveScene().name == "Bossfight") Achievements.UnlockStickyKeysAchievement();
        if(completionTime <= 2f && SceneManager.GetActiveScene().name == "DemoLevel") Achievements.UnlockDemoSpeedAchievement();
        
        //Debug.Log($"Run completed! Time: {minutes:00}:{seconds:00}");
        Debug.Log($"Run completed! Time: {completionTime}");
    }
    
    private void ResetTimer()
    {
        hasStarted = false;
        isComplete = false;
        runWasSuccessful = false;
        completionTime = 0f;
        Debug.Log("Timer reset and ready for new run");
    }
}