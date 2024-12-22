using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraFollowMP : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // If virtual camera isn't assigned in inspector, try to get it from the scene
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        // Subscribe to client scene ready event to set up camera when player spawns
        FindLocalPlayer();
    }
    private void FindLocalPlayer()
    {
        // Wait a frame to ensure player is spawned
        StartCoroutine(WaitForLocalPlayer());
    }

    private System.Collections.IEnumerator WaitForLocalPlayer()
    {
        while (GameObject.Find("LocalGamePlayer") == null)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Get the transform of the local player's object
        Transform playerTransform = NetworkClient.localPlayer.transform.GetChild(0).GetChild(1);
        
        // Set the camera to follow the local player
        if (virtualCamera != null && playerTransform != null)
        {
            virtualCamera.Follow = playerTransform;
        }
    }
}