using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Debug", menuName = "Scriptable Objects/Debug")]
public class DebugAsset : ScriptableObject
{ 
#if UNITY_EDITOR
	public SceneAsset startScene;
	public bool useEditorCode;
	public bool loadGame;

	private void OnValidate()
	{
		
	}
	
#endif
}
