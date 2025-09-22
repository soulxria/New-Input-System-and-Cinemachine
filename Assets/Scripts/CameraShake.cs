using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;

    void Awake()
    {
        // Grab the CinemachineCamera on this object
        var vcam = GetComponent<CinemachineCamera>();
        
        if (vcam == null)
        {
            Debug.LogError("CameraShake: No CinemachineCamera component found!");
            return;
        }

        // Fetch the Perlin component explicitly via the Noise stage
        perlin = vcam.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
        
        if (perlin == null)
        {
            Debug.LogError("CameraShake: No CinemachineBasicMultiChannelPerlin component found! Make sure to add a Noise extension to your CinemachineCamera.");
        }
        else
        {
            Debug.Log("CameraShake: Successfully initialized with Perlin noise component.");
        }
    }

    public void Shake(float intensity, float time)
    {
        if (perlin == null) 
        {
            Debug.LogError("CameraShake: Perlin component is null! Cannot shake camera.");
            return;
        }
        
        // Set both amplitude and frequency for effective shake
        perlin.AmplitudeGain = intensity;
        perlin.FrequencyGain = intensity * 0.5f; // Frequency scales with intensity but at a lower rate
        shakeTimer = time;
        
        Debug.Log($"CameraShake: Setting amplitude to {intensity} and frequency to {intensity * 0.5f} for {time} seconds. Current profile: {(perlin.NoiseProfile != null ? perlin.NoiseProfile.name : "NULL")}");
    }

    void Update()
    {
        if (shakeTimer > 0 && perlin != null)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                // Reset both amplitude and frequency
                perlin.AmplitudeGain = 0f;
                perlin.FrequencyGain = 1f; // Reset to default frequency
            }
        }
    }
}
