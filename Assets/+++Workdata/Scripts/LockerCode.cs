using UnityEngine;
using UnityEngine.InputSystem;

public class LockerCode : MonoBehaviour
{
	[SerializeField] private GameObject indicator;

	private Vector3 rotatePosition;
	
	public bool playerInReach = false;
	
	[SerializeField] int journalIndex = 0;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && GameManager.Instance.defeated2ndWave)
		{
			indicator.SetActive(true);
			playerInReach = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && GameManager.Instance.defeated2ndWave)
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
			GameManager.Instance.ActivateJournal(journalIndex);
		}
	}
}
