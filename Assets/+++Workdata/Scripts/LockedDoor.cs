using UnityEngine;
using UnityEngine.InputSystem;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    
    public bool playerInReach = false;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // indicator.SetActive(true);
            playerInReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // indicator.SetActive(false);
            playerInReach = false;
        }
    }
    void Update()
    {
        if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && GameManager.Instance.hasControlRoomKey)
        {
            anim.Play("DoorOpens");
        }
    }
}
