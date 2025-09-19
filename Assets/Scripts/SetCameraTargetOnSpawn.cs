using Unity.Cinemachine;
using UnityEngine;

public class SetCameraTargetOnSpawn : MonoBehaviour
{
    [Tooltip("Cinemachine Virtual Camera to control")]
    public CinemachineCamera virtualCamera;

    [Tooltip("Tag of the player object")]
    public string playerTag = "Player";

    private bool targetSet = false;

    void Update()
    {
        // If target isn't set yet, try to find the player
        if (!targetSet)
        {
            GameObject player = GameObject.FindWithTag(playerTag);
            if (player != null)
            {
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform; // optional for 2D
                targetSet = true; // don't check again
            }
        }
    }
}
