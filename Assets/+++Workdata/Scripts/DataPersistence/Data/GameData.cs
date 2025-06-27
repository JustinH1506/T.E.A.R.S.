using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public bool[] activeJournals;
    public bool[] activeItems;
    public bool defeated2ndWave;
    public bool hasControlRoomKey;
    public bool hasControlPanelKey;
    public bool openedEntranceDoor;
    public bool inactiveCharactersActive;
    public bool activeControlPanel;
    
    public int enemiesDefeated;

    public int playerHp;
    
    public SerializableDictionary<string, EnemyStateMachine.Data> enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
    
    /// <summary>
    /// Creates a default version of how the GameData should be. 
    /// </summary>
    public GameData()
    {
        playerPosition = Vector3.zero;
        playerHp = 250;
        enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
        activeJournals = new bool[4];
        activeItems = new bool[4];
        hasControlRoomKey = false;
        hasControlPanelKey = false;
        openedEntranceDoor = false;
        defeated2ndWave = false;
    }
    
    /// <summary>
    /// Saves the data of the enemies depending on each unique guid. 
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public EnemyStateMachine.Data GetEnemyPosition(string guid)
    {
        if (enemyPositionByGuid.TryGetValue(guid, out var data))
            return data;
        return null;
    }
}
