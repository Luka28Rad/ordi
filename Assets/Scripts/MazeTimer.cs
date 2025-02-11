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

    private void Update()
    {
        // Get player position (assuming this script is attached to the player)
        Vector3 playerPos = transform.position;
        
        // Check if player is at start point and hasn't started yet
        if (!hasStarted && !isComplete && IsAtPoint(playerPos, startPoint.position))
        {
            StartTimer();
        }
        
        // Check if player has reached end point after starting
        if (hasStarted && !isComplete && IsAtPoint(playerPos, endPoint.position))
        {
            CompleteRun();
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
        Debug.Log("Timer started!");
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }
    
    private void CompleteRun()
    {
        float completionTime = Time.time - startTime;
        isComplete = true;
        hasStarted = false;
        
        // Convert to minutes and seconds
        int minutes = Mathf.FloorToInt(completionTime / 60f);
        int seconds = Mathf.FloorToInt(completionTime % 60f);
        if(seconds <= 13 && SceneManager.GetActiveScene().name == "Level 1") Achievements.UnlockMazeRunnerAchievement();
        if(seconds <= 33 && SceneManager.GetActiveScene().name == "Level 2") Achievements.UnlockObstacleCourseAchievement();
        if(seconds <= 5 && SceneManager.GetActiveScene().name == "Level 3") Achievements.UnlockJumpingJackAchievement();
        if(seconds <= 3 && SceneManager.GetActiveScene().name == "Bossfight") Achievements.UnlockStickyKeysAchievement();
        Debug.Log($"Run completed! Time: {minutes:00}:{seconds:00}");
        ResetTimer();
    }
    
    // Method to reset the timer
    public void ResetTimer()
    {
        hasStarted = false;
        isComplete = false;
        Debug.Log("Timer reset and ready for new run");
        startPoint.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);
    }
    
    // Method to get current time if run is in progress
    public string GetCurrentTime()
    {
        if (!hasStarted || isComplete)
            return "00:00";
            
        float currentTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        
        return $"{minutes:00}:{seconds:00}";
    }
    
    // Optional: Draw detection radius in editor for easy setup
    private void OnDrawGizmos()
    {
        if (startPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, detectionRadius);
        }
        
        if (endPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint.position, detectionRadius);
        }
    }
}