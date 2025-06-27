using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject itemBlinkLight;
    
    public bool playerInReach = false;
    private bool opened = false;

    private Animator anim;
    
    private SphereCollider sphereCollider;
    
    private AudioSource audioSource;

    /// <summary>
    /// Get the animator. 
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Activates indicator and makes the Player seem in reach. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            UIManager.Instance.OpenMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f, false);
            playerInReach = true;
        }
    }

    /// <summary>
    /// Deactivates indicator and makes the Player seem in reach. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
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
            opened = true;
            
            itemBlinkLight.SetActive(false);
            
            AudioManager.Instance.PlaySound(AudioManager.Instance.officeDoor, audioSource);
            
            anim.Play("DoorOpens");
            
            UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
        }
        else if(playerInReach && Keyboard.current.eKey.wasPressedThisFrame && !GameManager.Instance.hasControlRoomKey)
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.lockedDoor));
        }
    }
}