using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    public Light lightComponent;
    // Minimum and maximum intensity of the light
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.0f;

    // Flicker speed
    public float flickerSpeed = 10.0f;

    // Current intensity of the light
    float lightIntensity = 0.0f;

    void Update()
    {
        float t = Time.time * flickerSpeed * Random.Range(0.1f, 1f);
        lightIntensity = (Mathf.Sin(t) + Mathf.Sin(t * 0.2f) + Mathf.Sin(t * 2f)) / 3f;
        lightIntensity = Mathf.Lerp(minIntensity, maxIntensity, lightIntensity);

        // Set the intensity of the light
        lightComponent.intensity = lightIntensity;
    }
}
