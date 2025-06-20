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
			doorAnim.Play("CloseDoors");
			UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.Locked, 1f);
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue());
			inactiveEnemy.SetActive(false);
			activeEnemy.SetActive(true);
		}
	}
}
