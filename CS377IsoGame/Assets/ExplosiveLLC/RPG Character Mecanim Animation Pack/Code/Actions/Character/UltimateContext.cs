using UnityEngine;

namespace RPGCharacterAnims.Actions
{
    public class UltimateContext
    {

        public Transform location;
        public CustomTerrain.Terrains terrain;
        public string type;
        

        public UltimateContext(string HandlerType, Transform loc, CustomTerrain.Terrains ter)
        {
            location = loc;
            terrain = ter;
            type = HandlerType;

        }
        
    }
}