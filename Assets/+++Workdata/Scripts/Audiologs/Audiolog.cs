using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Audiolog : MonoBehaviour
{
	[SerializeField] private GameObject itemBlinkLight;
	
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
	/// Activates the Journal ui and the journal that was pressed.
	/// </summary>
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			itemBlinkLight.SetActive(false);
			sphereCollider.enabled = false;
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			playerInReach = false;
			UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f, true);
			GameManager.Instance.ActivateJournal(journalId);
		}
	}
}
