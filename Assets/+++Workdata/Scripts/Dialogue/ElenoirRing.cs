using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class ElenoirRing : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;

	[SerializeField] private GameObject inactiveEnemy;
	[SerializeField] private GameObject activeEnemy;
	
	[SerializeField] private PlayableDirector ringCutscene;
	
	[SerializeField] private CinemachineVirtualCamera cam;
	
	[SerializeField] private GameObject indicator;
	
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
	///  Deactivates indicator and makes the Player seem in reach. 
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
	///  When pressing e start a text and activating the next parts to work. 
	/// </summary>
	private void Update()
	{
		//transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Keyboard.current.eKey.wasPressedThisFrame)
		{
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue(AudioManager.Instance.ringSubtitleData, cam));
			GameManager.Instance.ActivateItem(1);
			doorAnim.Play("CloseDoors");
			inactiveEnemy.SetActive(false);
			activeEnemy.SetActive(true);
		}
	}
}
