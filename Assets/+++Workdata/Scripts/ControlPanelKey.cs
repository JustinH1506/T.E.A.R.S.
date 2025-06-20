using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPanelKey : MonoBehaviour
{
	[SerializeField] private GameObject indicator;
	
	public bool playerInReach = false;
	
	[SerializeField] private ControlPanel controlPanel;
	
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
		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			StartCoroutine(UIManager.Instance.StartText("You got a Control Panel Key!"));
			GameManager.Instance.hasControlPanelKey = true;
			controlPanel.enabled = true;
			gameObject.SetActive(false);
		}
	}
}
