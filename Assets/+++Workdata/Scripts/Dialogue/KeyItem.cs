using UnityEngine;

public class KeyItem : MonoBehaviour
{
	public bool playerInReach = false;

	[SerializeField] private Animator doorAnim;
	
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
		transform.LookAt(Camera.main.transform.position);

		if (playerInReach && Input.GetKeyDown(KeyCode.E))
		{
			doorAnim.Play("EntranceDoors");
			UIManager.Instance.OpenMenu(UIManager.Instance.dialogueUi, CursorLockMode.None, 1f);
			AudioManager.Instance.StartCoroutine(AudioManager.Instance.StartDialogue());
		}
	}
}
