using UnityEngine;

namespace RPGCharacterAnims.Actions
{
    public class FlamethrowerSkillContext
    {

        public Vector3 direction;
        public Transform playerTransform;
        public CustomTerrain.Terrains terrain;
        public string type;
        
        

        public FlamethrowerSkillContext(string handlerType, Vector3 loc, CustomTerrain.Terrains ter, Transform nucleus)
        {
            direction = loc;
            terrain = ter;
            type = handlerType;
            playerTransform = nucleus;

        }
        
    }
}