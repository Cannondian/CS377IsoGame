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

    public static void ClearPlayerStats()
    {
        PlayerPrefs.DeleteKey("PlayerStats");
        PlayerPrefs.Save();
    }
}
