using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    
    public bool playerInReach = false;
    private bool opened = false;

    private Animator anim;

    /// <summary>
    /// Get the animator. 
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Activates indicator and makes the Player seem in reach. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            indicator.SetActive(true);
            playerInReach = true;
        }
    }

    /// <summary>
    /// Deactivates indicator and makes the Player seem in reach. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && opened)
        {
            indicator.SetActive(false);
            playerInReach = false;
        }
    }
    
    /// <summary>
    /// When pressing e start a text and activating the next parts to work. 
    /// </summary>
    void Update()
    {
        if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && GameManager.Instance.hasControlRoomKey)
        {
            anim.Play("DoorOpens");
            opened = true;
        }
    }
}
