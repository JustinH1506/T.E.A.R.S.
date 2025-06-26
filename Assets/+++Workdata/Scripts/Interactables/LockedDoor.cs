using UnityEngine;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    
    public bool playerInReach = false;
    private bool opened = false;

    private Animator anim;
    
    private SphereCollider sphereCollider;

    /// <summary>
    /// Get the animator. 
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
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
            sphereCollider.enabled = false;
            playerInReach = false;
            indicator.SetActive(false);
            anim.Play("DoorOpens");
            opened = true;
        }
    }
}