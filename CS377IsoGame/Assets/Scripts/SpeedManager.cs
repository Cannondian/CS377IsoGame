
    using UnityEngine;

    public class SpeedManager: Singleton<SpeedManager>
    {
        private StatModifier currentMod;
        [SerializeField] private StatsTemplate playerStats;
        public void ChangeSpeed(float newSH)
        {
            if (currentMod != null)
            {
                playerStats.RemoveModifier(StatsTemplate.statsList.Speed ,currentMod);
                currentMod = new StatModifier((float)(newSH * 0.12), StatModifierType.Percent);
                playerStats.AddModifier(StatsTemplate.statsList.Speed, currentMod);
            }
            currentMod = new StatModifier((float)(newSH * 0.12), StatModifierType.Percent);
            playerStats.AddModifier(StatsTemplate.statsList.Speed, currentMod);
        } 
    }
