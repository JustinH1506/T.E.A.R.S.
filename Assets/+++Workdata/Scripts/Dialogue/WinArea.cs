using UnityEngine;

public class WinArea : MonoBehaviour
{
	/// <summary>
	/// The area to win the game.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			UIManager.Instance.OpenMenu(UIManager.Instance.demoEndScreen, CursorLockMode.None, 0f, true);
		}
	}
}
