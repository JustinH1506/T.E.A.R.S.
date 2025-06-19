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

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
	}

	public void NewGame()
	{
		gameData = new GameData();
	}

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
	
	public void SaveGame()
	{
		dataPersistenceObjects = FindAllDataPersistenceObjects();

		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.SaveData(gameData);
		}
		
		dataHandler.Save(gameData);
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects()
	{
		IEnumerable<IDataPersistence> dataPersistenceObjs = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();
		
		return new List<IDataPersistence>(dataPersistenceObjs);
	}
}
