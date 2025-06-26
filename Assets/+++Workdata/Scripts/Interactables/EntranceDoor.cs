using UnityEngine;
using UnityEngine.InputSystem;

public class EntranceDoor : MonoBehaviour
{
    public bool playerInReach = false;
 	
 	[SerializeField] private GameObject indicator;
 	
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
 		if (other.CompareTag("Player"))
 		{
 			indicator.SetActive(false);
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
 			anim.Play("DoorOpens");
		    indicator.SetActive(false);
		    sphereCollider.enabled = false;
 		}
 	}
}
