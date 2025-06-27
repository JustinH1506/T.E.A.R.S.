using UnityEngine;
using UnityEngine.InputSystem;

public class LockedLocker : MonoBehaviour
{
	[SerializeField] private GameObject itemBlinkLight;

	[SerializeField] private GameObject controlRoomKey;
	
	private Vector3 rotatePosition;
	
	private bool playerInReach = false;
	
	private Animator anim;
	
	private SphereCollider sphereCollider;

	/// <summary>
	/// Get the animator, sphereCollider. 
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
	/// Activates indicator and makes the Player seem in reach. 
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
	/// Opens the lock when having defeated the 2nd wave or Starts the dialogue that a code is needed. 
	/// </summary>
	private void Update()
	{
		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && GameManager.Instance.defeated2ndWave)
		{
			playerInReach = false;
			sphereCollider.enabled = false;
			sphereCollider.enabled = false;
			
			itemBlinkLight.SetActive(false);
			controlRoomKey.SetActive(true);
			
			anim.Play("LockerOpen");
			
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
		}
		else if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && !GameManager.Instance.defeated2ndWave)
		{
			StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.lockedLockerDialogue));
		}
	}
}
