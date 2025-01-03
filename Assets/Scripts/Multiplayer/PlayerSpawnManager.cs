using Mirror;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerSpawnManager : NetworkBehaviour
{
    public TMP_Text countdownText;
    private int totalPlayers;

    public delegate void AllPlayersReadyHandler();
    public event AllPlayersReadyHandler OnAllPlayersReady;

    private void Start(){
        StartCoroutine(CountdownToStart());
        }

private IEnumerator CountdownToStart()
{
    int countdown = 5; // Start from 5 seconds

        while (countdown > 0)
        {
            // Update the text to show the countdown
            countdownText.text = $"Game starts in \n{countdown}";
            yield return new WaitForSeconds(1); // Wait for 1 second
            countdown--;
        }

        // Clear the text and invoke the event
        OnAllPlayersReady?.Invoke();
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f); // Optional delay before clearing
        countdownText.text = ""; // Clear text after the countdown
   
   
   // yield return new WaitForSeconds(5); // 5-second countdown
}

}
