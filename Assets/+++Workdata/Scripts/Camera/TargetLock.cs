using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;           
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] private CineMachineManualFreeLook cineMachineManualFreeLook;
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon; 
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag;
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; 
    [SerializeField] private float maxDistance;
    
    public bool isTargeting;
    public Transform currentTarget;
    
    private float maxAngle;
    private float mouseX;
    private float mouseY;

    /// <summary>
    /// Initiate values.
    /// </summary>
    void Start()
    {
        maxAngle = 60; 
        cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
    }

    /// <summary>
    /// Sets values depending on bool isTargeting. 
    /// </summary>
    void Update()
    {
        if (!isTargeting)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            NewInputTarget(currentTarget);
        }

        if (isTargeting && currentTarget.GetComponent<EnemyStateMachine>().IsDead)
        {
            isTargeting = false;
            currentTarget = null;
        }

        // if (aimIcon) 
        //     aimIcon.gameObject.SetActive(isTargeting);

        cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;
    }

    /// <summary>
    /// When pressing the middle mouse button we look for the closest target or remove the lock on. 
    /// </summary>
    /// <param name="context"></param>
    public void AssignTarget(InputAction.CallbackContext context)
    {
        if (isTargeting)
        {
            isTargeting = false;
            currentTarget = null;
            cineMachineManualFreeLook.isTargeting = false;
            return;
        }

        if (ClosestTarget())
        {
            currentTarget = ClosestTarget().transform;
            isTargeting = true;
            cineMachineManualFreeLook.isTargeting = true;
        }
    }

    /// <summary>
    /// Sets the target and lock on to the enemy. 
    /// </summary>
    /// <param name="target"></param>
    private void NewInputTarget(Transform target)
    {
        if (!currentTarget) return;

        Vector3 lockPosition = target.position;
        lockPosition.y = target.position.y + targetLockOffset.y;
        Vector3 viewPos = mainCamera.WorldToViewportPoint(lockPosition);
        
        // if(aimIcon)
        //     aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        mouseX = (viewPos.x - 0.5f ) * 3f;              
        mouseY = (viewPos.y - 0.5f ) * 3f;              
    }

    /// <summary>
    /// Finds the closest enemy.
    /// </summary>
    /// <returns></returns>
    private GameObject ClosestTarget()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(enemy.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = enemy;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
}
