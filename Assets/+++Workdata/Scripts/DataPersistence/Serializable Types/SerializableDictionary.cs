using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField] private List<TKey> keys = new List<TKey>();
	
	[SerializeField] private List<TValue> values = new List<TValue>();
	
	/// <summary>
	/// This script makes it so we have a serializable dictionary so we can save the data in it. 
	/// </summary>
	
	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach (KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		this.Clear();

		if (keys.Count != values.Count)
		{
			Debug.LogError("OnAfterDeserialize: keys.Count != values.Count " + keys.Count + " " + values.Count);
		}

		for (int i = 0; i < keys.Count; i++)
		{
			this.Add(keys[i], values[i]);
		}
	}
}
