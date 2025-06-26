using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPanelKey : MonoBehaviour
{
	[SerializeField] private GameObject indicator;
	
	public bool playerInReach = false;
	
	[SerializeField] private ControlPanel controlPanel;
	
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
		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			StartCoroutine(UIManager.Instance.StartText("You got a Control Panel Key!"));
			GameManager.Instance.ActivateItem(3);
			GameManager.Instance.hasControlPanelKey = true;
			controlPanel.enabled = true;
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
