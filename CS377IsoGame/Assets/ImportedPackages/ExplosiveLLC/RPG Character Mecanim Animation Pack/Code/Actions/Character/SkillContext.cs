using RPGCharacterAnims.Lookups;
using UnityEngine;

namespace RPGCharacterAnims.Actions
{
    public class SkillContext
    {
        public string type;
        public Vector3 pos;
        public TileElement.ElementType element;
        public int energy;

        public SkillContext(string type, Vector3 location, TileElement.ElementType e, int number = -1)
        {
            this.type = type;
            element = e;
            pos = location;
            this.energy = number;
        }
    }
}
