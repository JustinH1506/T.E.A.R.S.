using UnityEngine;
using UnityEngine.InputSystem;

public class Audiolog : MonoBehaviour
{
	[SerializeField] private GameObject indicator;

	private Vector3 rotatePosition;
	
	public bool playerInReach = false;

	public int journalId;
	
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
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f, true);
			GameManager.Instance.ActivateJournal(journalId);
		}
	}
}
