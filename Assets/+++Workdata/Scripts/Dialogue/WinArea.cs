using UnityEngine;

public class WinArea : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.OpenMenu(UIManager.Instance.demoEndScreen, CursorLockMode.None, 0f);
		}
	}
}
