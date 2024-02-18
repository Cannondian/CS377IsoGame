using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTypes 
{
   public enum Events
   {
      ON_PARTICLE_FX_FOR_SKILL,
      ON_CONTINOUS_PACRTICLE_FX_TRIGGER,
      ON_ENERGY_GAIN,
      ON_ULTIMATE_READY,
      ON_LIFE_CURRENT_GAIN,
      ON_JISA_ENHANCED_ATTACK_READY,
      ON_NEW_STATUS_CONDITION,
      ON_EXPIRED_STATUS_CONDITION,
      ON_ENEMY_HIT,
      ON_PLAYER_DAMAGE_TAKEN,
      ON_ATTACK_SWING,

      ON_JISA_ENHANCED_ATTACK,
      ON_UPDATE_CORE_CHARGE_PARTICLES,
      ON_BASIC_ATTACK_HIT,
      ON_SKILL_USED,
      ON_ULTIMATE_USED,

      
   }

   public class SkillUsedParam
   {
      public Color color;
      public Vector3 position;
      public FXList.FXlist fx;
      public float duration;
      public float delay;

      public SkillUsedParam(Color p1, Vector3 p2, FXList.FXlist p3, float p4, float p5)
      {
         color = p1;
         position = p2;
         fx = p3;
         delay = p4;
         duration = p5;
      }
   }

   public class Event2Param
   {
      public FXList.FXlist fx;
      public float duration;
      public float delay;
      public Color color;
      public Transform transform;
       public Event2Param(Color p1, Transform p2, FXList.FXlist p3, float p4, float p5)
       {
           color = p1;
           transform = p2;
           fx = p3;
           duration = p4;
           delay = p5;
       }
   }

   public class Event5Param
   {
      public float multiplier;
      public int attackNumber;

      public Event5Param(int actionNumber, float buffs)
      {
         multiplier = buffs;
         attackNumber = actionNumber;
      }
   }

   public class Event8Param
   {
      public int coreCharge;
      public bool isSuperCharged;
      public FXList.FXlist fx;

      public Event8Param(int charge, bool isSuper, FXList.FXlist FX)
      {
         coreCharge = charge;
         isSuperCharged = isSuper;
         fx = FX;
      }
   }
   public class HitFXParam
   {
      //what if we also sent damage and info for the enemy that is being hit withing the same event?
      public Vector3 hitPosition;
      public int attackType;
      public GameObject hitter;

      public HitFXParam(Vector3 pos, int number, GameObject weapon)
      {
         hitPosition = pos;
          attackType = number;
         hitter = weapon;
      }
   }

   public class StatusConditionFXParam
   {
      public GameObject condition;
      public int intensity;
      public GameObject caller;
      public string myName;
      
      public StatusConditionFXParam(GameObject prefab, GameObject callerObject, string name, int intensity = 1)
       {
           this.condition = prefab;
           this.intensity = intensity;
           myName = name;
           caller = callerObject;
       }
   }

   public class FloatingDamageParam
   {
      public GameObject target;
      public float damage;
      public FloatingDamageParam(GameObject targetObject, float damageValue)
       {
           target = targetObject;
           damage = damageValue;
       }
   }

   public class AttackSwingParam
   {
      public float attackSpeed;
      public int attackType;
      public Rigidbody staffBody;

      public AttackSwingParam(float speed, int type)
      {
         attackSpeed = speed;
          attackType = type;
          

      }
      
   }
}
