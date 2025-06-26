using UnityEngine;
using UnityEngine.InputSystem;

public class EntranceDoor : MonoBehaviour
{
    public bool playerInReach = false;
 	
    [SerializeField] private GameObject itemBlinkLight;
 	
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
 		if (other.CompareTag("Player"))
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
 	private void Update()
 	{
 		//transform.LookAt(Camera.main.transform.position);
 
 		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
 		{
		    sphereCollider.enabled = false;
		    UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
		    itemBlinkLight.SetActive(false);
		    playerInReach = false;
 			anim.Play("DoorOpens");
		    sphereCollider.enabled = false;
 		}
 	}
}
