using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using RPGCharacterAnims;
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
      ON_FLAMETHROWER_SKILL_START,
      ON_FLAMETHROWER_SKILL_END,
      ON_CONTINOUS_ENERGY_DRAIN_START,
      ON_JISA_ENHANCED_ATTACK,
      ON_UPDATE_CORE_CHARGE_PARTICLES,
      ON_BASIC_ATTACK_HIT,
      ON_SKILL_USED,
      ON_ULTIMATE_USED,
      ON_HIGHLIGHT_FX_TRIGGERED,
      ON_HIGHLIGHT_FX_EXPIRED,
      ON_BUFF_ORB_PICKED_UP,
      ON_VH_FLAMETHROWER_ENABLED,
      ON_VH_BACKLASH_HIT
      

      
   }

   public class SkillUsedParam
   {
      public TileElement.ElementType tile;
      public Vector3 position;
      public float duration;
      public float delay;

      public SkillUsedParam(TileElement.ElementType p1, Vector3 p2, float p4, float p5)
      {
         tile = p1;
         position = p2;
         delay = p4;
         duration = p5;
      }
   }

   public class FlamethrowerStartFXParam
   {
      public Vector3 direction;
      public TileElement.ElementType element;
      public Transform transform;
       public FlamethrowerStartFXParam(TileElement.ElementType p1, Transform p2, Vector3 dir)
       {
           element = p1;
           transform = p2;
           direction = dir;
           
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
      public float intensity;
      public GameObject caller;
      public string myName;
      
      public StatusConditionFXParam(GameObject prefab, GameObject callerObject, string name, float intensity = 1)
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
      public int size; //on a scale of 1-4;
      public Damage.Types firstType;
      public float additionalDamage;
      public Damage.Types secondType;
      
      public FloatingDamageParam(GameObject targetObject, float damageValue, int sizeScale, 
         Damage.Types type = Damage.Types.Generic, float secondaryDamageValue = 0, 
         Damage.Types secondaryType = Damage.Types.Generic)
       {
           target = targetObject;
           damage = damageValue;
           size = sizeScale;
           firstType = type;
           additionalDamage = secondaryDamageValue;
           secondType = secondaryType;

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

   public class HighlightFXParam
   {
      public HighlightProfile tileHighlight;
      public float highlightIntensity;
      public bool isWeaponIncluded;

      public HighlightFXParam(HighlightProfile profile, float intensity, bool weapon)
      {
         tileHighlight = profile;
          highlightIntensity = intensity;
          isWeaponIncluded = weapon;
      }
   }

   public class BackLashHitParam
   {
      public Transform transform;

      public BackLashHitParam(Transform enemyTransform)
      {
         transform = enemyTransform;
      }
   }
}
