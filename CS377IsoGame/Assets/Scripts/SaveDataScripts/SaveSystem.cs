using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerStats(StatsTemplate statsTemplate)
    {
        if (!statsTemplate.amIEnemy)
        {
            PlayerStatsData data = new PlayerStatsData(statsTemplate);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("PlayerStats", json);
            PlayerPrefs.Save();
        }
    }

    public static void SaveTileMastery(TileMastery tileMastery)
    {
        if (tileMastery != null)
        {
            TileMasteryData data = new TileMasteryData(tileMastery);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("TileMastery", json);
            PlayerPrefs.Save();
        }
    }

    public static PlayerStatsData LoadPlayerStats()
    {
        if (PlayerPrefs.HasKey("PlayerStats"))
        {
            string json = PlayerPrefs.GetString("PlayerStats");
            PlayerStatsData data = JsonUtility.FromJson<PlayerStatsData>(json);
            return data;
        }
        return null; 
    }

    public static TileMasteryData LoadTileMastery()
    {
        if (PlayerPrefs.HasKey("TileMastery"))
        {
            string json = PlayerPrefs.GetString("TileMastery");
            TileMasteryData data = JsonUtility.FromJson<TileMasteryData>(json);
            return data;
        }

        return null;
    }
    

    public static void ClearStats()
    {
        PlayerPrefs.DeleteKey("PlayerStats");
        PlayerPrefs.DeleteKey("TileMastery");
        PlayerPrefs.Save();
    }
}
