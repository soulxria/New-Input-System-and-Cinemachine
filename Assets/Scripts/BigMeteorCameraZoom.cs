using Unity.Cinemachine;
using UnityEngine;

public class BigMeteorCameraZoom : MonoBehaviour
{
    [Header("Cinemachine Camera")]
    public CinemachineCamera virtualCamera;

    [Header("Settings")]
    public float targetFOV = 90f;
    public float zoomSpeed = 5f; // How fast it zooms

    private bool zooming = false;

    void Update()
    {
        // Smoothly interpolate FOV if zooming
        if (zooming && virtualCamera != null)
        {
            virtualCamera.Lens.FieldOfView = Mathf.Lerp(
                virtualCamera.Lens.FieldOfView,
                targetFOV,
                Time.deltaTime * zoomSpeed
            );

            // Stop zooming once close enough
            if (Mathf.Abs(virtualCamera.Lens.FieldOfView - targetFOV) < 0.1f)
                zooming = false;
        }
    }

    // Call this method when a BigMeteor is spawned
    public void OnBigMeteorSpawned()
    {
        zooming = true;
    }
}
