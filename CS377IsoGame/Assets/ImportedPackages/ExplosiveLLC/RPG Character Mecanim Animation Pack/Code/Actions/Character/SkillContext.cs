using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPGCharacterAnims.Actions
{
    public class SkillContext
    {
        public string type;
        public Vector3 pos;
        public CustomTerrain.Terrains terrainType;
        public int energy;

        public SkillContext(string type, Vector3 location, CustomTerrain.Terrains terrainData, int number = -1)
        {
            this.type = type;
            terrainType = terrainData;
            pos = location;
            this.energy = number;
        }
    }
}
