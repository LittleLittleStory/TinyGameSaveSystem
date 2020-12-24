using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveSystem : Singleton<GameSaveSystem>
{
    private GameData gameData;

    public GameData GameData
    {
        get
        {
            if (null == gameData)
                gameData = ToolUtility.LoadJson<GameData>("GameData");
            if (null == gameData)
                gameData = new GameData();
            return gameData;
        }
        private set
        {
            gameData = value;
        }
    }
}
