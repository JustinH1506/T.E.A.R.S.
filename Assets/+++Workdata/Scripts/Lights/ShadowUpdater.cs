using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class ShadowUpdater : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.3f;
    private Light targetLight;
    private float timeSinceLastUpdate = 0f;

    /// <summary>
    /// Gets the light. 
    /// </summary>
    private void Awake()
    {
        targetLight = GetComponent<Light>();
    }

    /// <summary>
    /// Updates the Shadows every 0.3 seconds to save performance.
    /// </summary>
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            if (targetLight != null && targetLight.TryGetComponent<HDAdditionalLightData>(out var lightData))
            {
                lightData.RequestShadowMapRendering();
                timeSinceLastUpdate = 0f;
            }
        }
    }
}
