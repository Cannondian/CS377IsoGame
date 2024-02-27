using UnityEngine;

namespace RPGCharacterAnims.Actions
{
    public class FlamethrowerSkillContext
    {

        public Vector3 direction;
        public Transform playerTransform;
        public TileElement.ElementType element; 
        public string type;
        
        

        public FlamethrowerSkillContext(string handlerType, Vector3 loc, TileElement.ElementType ter, Transform nucleus)
        {
            direction = loc;
            element = ter;
            type = handlerType;
            playerTransform = nucleus;

        }
        
    }
}