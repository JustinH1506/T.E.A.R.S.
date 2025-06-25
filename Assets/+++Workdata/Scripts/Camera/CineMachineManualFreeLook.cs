using Cinemachine;
using UnityEngine;

public class CineMachineManualFreeLook : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    public float horizontalAimingSpeed = 20f;
    public float verticalAimingSpeed = 20f;

    public bool isTargeting;

    [Tooltip("This depends on your Free Look rigs setup, use to correct Y sensitivity,"
             + " about 1.5 - 2 results in good Y-X square responsiveness")]
    public float yCorrection = 2f;

    private float xAxisValue;
    private float yAxisValue;

    /// <summary>
    /// Gets the free look camera.
    /// </summary>
    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    /// <summary>
    /// Overwrites how camera works to prevent when moving the mouse fast.  
    /// </summary>
    private void Update()
    {
        if (isTargeting)
            return;
        
        float mouseX = Input.GetAxis("Mouse X") * horizontalAimingSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalAimingSpeed * Time.deltaTime;

        // Correction for Y
        mouseY /= 360f;
        mouseY *= yCorrection;

        xAxisValue += mouseX;
        yAxisValue = Mathf.Clamp01(yAxisValue - mouseY);

        freeLook.m_XAxis.Value = xAxisValue;
        freeLook.m_YAxis.Value = yAxisValue;
    }
}
