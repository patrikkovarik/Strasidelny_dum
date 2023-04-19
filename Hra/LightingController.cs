using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    [Header("Lighting Settings")]
    public float minIntensity = 0.1f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private Light[] lights;

    void Start()
    {
        lights = GetComponentsInChildren<Light>();
    }

    void Update()
    {
        foreach (Light light in lights)
        {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, light.transform.position.magnitude));
            light.intensity = intensity;
        }
    }
}
