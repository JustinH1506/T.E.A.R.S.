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

    public int playerHp;
    
    public SerializableDictionary<string, EnemyStateMachine.Data> enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
    
    public GameData()
    {
        playerPosition = Vector3.zero;
        playerHp = 250;
        enemyPositionByGuid = new SerializableDictionary<string, EnemyStateMachine.Data>();
        activeJournals = new bool[4];
        activeItems = new bool[4];
    }
    
    public EnemyStateMachine.Data GetEnemyPosition(string guid)
    {
        if (enemyPositionByGuid.TryGetValue(guid, out var data))
            return data;
        return null;
    }
}
