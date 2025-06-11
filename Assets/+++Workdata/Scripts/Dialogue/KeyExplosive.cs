using UnityEngine;
using UnityEngine.InputSystem;

public class KeyExplosive : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;
	
	[SerializeField] private GameObject winZone;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//indicator.SetActive(true);
			playerInReach = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//indicator.SetActive(false);
			playerInReach = false;
		}
	}
	
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			doorAnim.Play("DoorOpens");
			winZone.SetActive(true);
		}
	}
}
