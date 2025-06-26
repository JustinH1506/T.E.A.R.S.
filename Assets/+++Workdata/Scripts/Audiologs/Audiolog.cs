using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Audiolog : MonoBehaviour
{
	[SerializeField] private GameObject indicator;

	private Vector3 rotatePosition;
	
	public bool playerInReach = false;

	public int journalId;
	
	private SphereCollider sphereCollider;

	private void Awake()
	{
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
	/// Activates the Journal ui and the journal that was pressed.
	/// </summary>
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			sphereCollider.enabled = false;
			playerInReach = false;
			UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f, true);
			GameManager.Instance.ActivateJournal(journalId);
		}
	}
}
