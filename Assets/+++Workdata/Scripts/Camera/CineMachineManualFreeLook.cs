using System;
using Cinemachine;
using UnityEngine;

public class CineMachineManualFreeLook : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    public float horizontalAimingSpeed = 20f;
    public float verticalAimingSpeed = 20f;

    public bool isTargeting = false;
    
    public float yCorrection = 2f;

    private float xAxisValue;
    private float yAxisValue;

    /// <summary>
    /// Gets the free look camera and sets the speed of the camera.
    /// </summary>
    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        horizontalAimingSpeed = UIManager.Instance.sensitivitySlider.value;
        verticalAimingSpeed = UIManager.Instance.sensitivitySlider.value;
    }

    /// <summary>
    /// Overwrites how camera works to prevent issue when moving the mouse too fast.  
    /// </summary>
    private void Update()
    {
        horizontalAimingSpeed = UIManager.Instance.sensitivitySlider.value;
        verticalAimingSpeed = UIManager.Instance.sensitivitySlider.value;
        
        if (isTargeting || UIManager.Instance.stopCam)
            return;
        
        float mouseX = Input.GetAxis("Mouse X") * horizontalAimingSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalAimingSpeed;

        // Correction for Y
        mouseY /= 360f;
        mouseY *= yCorrection;

        xAxisValue += mouseX;
        yAxisValue = Mathf.Clamp01(yAxisValue - mouseY);

        freeLook.m_XAxis.Value = xAxisValue;
        freeLook.m_YAxis.Value = yAxisValue;
    }
}
