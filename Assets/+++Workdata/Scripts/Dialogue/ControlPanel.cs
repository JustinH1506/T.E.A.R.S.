using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPanel : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;
	
	[SerializeField] private GameObject winZone;

	public bool isActive;
	
	public GameObject itemBlinkLight;
	
	private Animator anim;
	
	private SphereCollider sphereCollider;
	
	[SerializeField] private AudioSource audioSource;

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
		if (other.CompareTag("Player") && isActive)
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
		if (other.CompareTag("Player") && isActive)
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

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && GameManager.Instance.hasControlPanelKey)
		{
			playerInReach = false;
			sphereCollider.enabled = false;
			
			itemBlinkLight.SetActive(false);
			
			winZone.SetActive(true);
			
			anim.Play("ControlPanelLever");
			doorAnim.Play("DoorOpens");
			
			audioSource.Stop();
			
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			
			StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.activatingControlPanel));
		}
		else if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame && !GameManager.Instance.hasControlPanelKey)
		{
			StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.controlPanelWithoutKey));
		}
	}
}
