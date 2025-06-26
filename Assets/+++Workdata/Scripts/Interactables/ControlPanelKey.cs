using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPanelKey : MonoBehaviour
{
	[SerializeField] private GameObject itemBlinkLight;
	
	[SerializeField] private ControlPanel controlPanel;
	
	
	
	public bool playerInReach = false;
	
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
	/// When pressing e start a text and activating the next parts to work. 
	/// </summary>
	private void Update()
	{
		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			sphereCollider.enabled = false;
			itemBlinkLight.SetActive(false);
			UIManager.Instance.CloseMenu(UIManager.Instance.indicatorScreen, CursorLockMode.Locked, 1f);
			playerInReach = false;
			StartCoroutine(UIManager.Instance.StartText("You got a Control Panel Key!"));
			GameManager.Instance.ActivateItem(3);
			GameManager.Instance.hasControlPanelKey = true;
			controlPanel.enabled = true;
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
