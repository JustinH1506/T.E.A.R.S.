using UnityEngine;
using UnityEngine.InputSystem;

public class LockerCode : MonoBehaviour
{
	[SerializeField] private GameObject itemBlinkLight;

	[SerializeField] private GameObject controlRoomKey;
	
	private Vector3 rotatePosition;
	
	private bool playerInReach = false;
	
	private Animator anim;
	private SphereCollider sphereCollider;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		sphereCollider = GetComponent<SphereCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.OpenMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f, false);
			playerInReach = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			playerInReach = false;
		}
	}

	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			anim.Play("LockerOpen");
			sphereCollider.enabled = false;
			playerInReach = false;
			itemBlinkLight.SetActive(false);
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			controlRoomKey.SetActive(true);
			sphereCollider.enabled = false;
		}
	}
}
