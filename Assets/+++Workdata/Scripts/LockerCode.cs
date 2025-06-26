using UnityEngine;
using UnityEngine.InputSystem;

public class LockerCode : MonoBehaviour
{
	[SerializeField] private GameObject indicator;

	private Vector3 rotatePosition;
	
	private bool playerInReach = false;

	[SerializeField] private GameObject controlRoomKey;
	
	private Animator anim;
	private SphereCollider triggerZone;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		triggerZone = GetComponent<SphereCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			indicator.SetActive(true);
			playerInReach = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			indicator.SetActive(false);
			playerInReach = false;
		}
	}

	private void Update()
	{
		transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			anim.Play("LockerOpen");
			controlRoomKey.SetActive(true);
			triggerZone.enabled = false;
		}
	}
}
