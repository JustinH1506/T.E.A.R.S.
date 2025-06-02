using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public enum GameStates
    {
        MainMenu,
        InGame,
        InBattle
    }
    
    [Header("Game Variables")]
    public static GameManager Instance;

    public int killedEnemies = 0;
    
    public GameStates gameStates;

    public bool[] journalStates;

    public bool hasKey = false;
    
    [FormerlySerializedAs("_debug")]
    [Space]
    
    [Header("Debug variables.")]
    [SerializeField] public DebugAsset debugAsset;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
#if UNITY_EDITOR

        if (debugAsset.useEditorCode)
        {
            string activeSceneName = debugAsset.startScene.name;
            
            if (activeSceneName != String.Empty)
            {
                if (debugAsset.loadGame)
                {
                    DataPersistenceManager.Instance.LoadGame();
                }
                else
                {
                    DataPersistenceManager.Instance.NewGame();
                }
                
                SceneManager.LoadScene(activeSceneName, LoadSceneMode.Additive);
                
                UIManager.Instance.OpenMenu(UIManager.Instance.inGameUi, CursorLockMode.Locked, 1f);
            }
        }
        else
        {
            UIManager.Instance.OpenMenu(UIManager.Instance.mainMenuScreen, CursorLockMode.None, 1f);
        }
#endif
    }

    public void CheckKey()
    {
        if (killedEnemies == 2)
        {
            hasKey = true;
        }
    }

    public void ActivateJournal(int journalIndex)
    {
        journalStates[journalIndex] = true;
        
        UIManager.Instance.journalButtons[journalIndex].interactable = true;
        UIManager.Instance.OpenMenu(UIManager.Instance.journalScreen, CursorLockMode.None, 0f);
        UIManager.Instance.journalButtons[journalIndex].onClick.Invoke();
    }

    public void LoadData(GameData gameData)
    {
        journalStates = gameData.activeJournals;
    }

    public void SaveData(GameData gameData)
    {
        gameData.activeJournals = journalStates;
    }
}
