using UnityEngine;

public static class Extensions
{
	/// <summary>
	/// Makes the Canvas Group visible, activates interactable and activates blocks raycasts
	/// </summary>
	/// <param name="myCanvasGroup"></param>
	public static void ShowCanvasGroup(this CanvasGroup myCanvasGroup)
	{
		myCanvasGroup.alpha = 1f;
		myCanvasGroup.interactable = true;
		myCanvasGroup.blocksRaycasts = true;
	}
	
	/// <summary>
	/// Makes the Canvas Group invisible, deactivates interactable and deactivates blocks raycasts
	/// </summary>
	/// <param name="myCanvasGroup"></param>
	public static void HideCanvasGroup(this CanvasGroup myCanvasGroup)
	{
		myCanvasGroup.alpha = 0f;
		myCanvasGroup.interactable = false;
		myCanvasGroup.blocksRaycasts = false;
	}
}
