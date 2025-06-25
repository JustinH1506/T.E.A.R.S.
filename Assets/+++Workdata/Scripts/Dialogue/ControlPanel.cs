using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPanel : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;
	
	[SerializeField] private GameObject winZone;
	
	[SerializeField] private GameObject indicator;
	
	[SerializeField] private CinemachineVirtualCamera vCam;
	
	private Animator anim;

	/// <summary>
	/// Get the animator. 
	/// </summary>
	private void Awake()
	{
		anim = GetComponent<Animator>();
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
	/// When pressing e start a text and activating the next parts to work. 
	/// </summary>
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.activatingControlPanel, vCam));
			anim.Play("ControlPanelLever");
			doorAnim.Play("DoorOpens");
			winZone.SetActive(true);
		}
	}
}
