using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]
	[SerializeField] private string fileName;

	public static DataPersistenceManager Instance;

	private GameData gameData;
	private List<IDataPersistence> dataPersistenceObjects;
	private FileDataHandler dataHandler;

	/// <summary>
	/// Make this object to an instance. 
	/// </summary>
	private void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Creates a new FileDataHandler.
	/// </summary>
	private void Start()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
	}

	/// <summary>
	/// Creates a new GameData. 
	/// </summary>
	public void NewGame()
	{
		gameData = new GameData();
	}

	/// <summary>
	/// Loads the Game. If there are no data creates a new game. 
	/// </summary>
	public void LoadGame()
	{
		gameData = dataHandler.Load();
		
		if (gameData == null)
		{
			NewGame();
		}

		dataPersistenceObjects = FindAllDataPersistenceObjects();
		
		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.LoadData(gameData);
		}
	}
	
	/// <summary>
	/// Calls the save data. 
	/// </summary>
	public void SaveGame()
	{
		dataPersistenceObjects = FindAllDataPersistenceObjects();

		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.SaveData(gameData);
		}
		
		dataHandler.Save(gameData);
	}

	/// <summary>
	/// Finds all objects with the interface IDataPersistence.
	/// </summary>
	/// <returns></returns>
	private List<IDataPersistence> FindAllDataPersistenceObjects()
	{
		IEnumerable<IDataPersistence> dataPersistenceObjs = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();
		
		return new List<IDataPersistence>(dataPersistenceObjs);
	}
}
