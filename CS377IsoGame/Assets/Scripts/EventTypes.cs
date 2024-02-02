using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTypes 
{
   public enum Events
   {
      ON_PARTICLE_FX_TRIGGER,
      ON_CONTINOUS_PACRTICLE_FX_TRIGGER,
      ON_ENERGY_GAIN,
      ON_ULTIMATE_READY,
      ON_LIFE_CURRENT_GAIN,
      ON_JISA_ENHANCED_ATTACK_READY,
      ON_JISA_ENHANCED_ATTACK
   }

   public class Event1Param
   {
      public Color color;
      public Vector3 position;
      public FXList.FXlist fx;
      public float duration;
      public float delay;

      public Event1Param(Color p1, Vector3 p2, FXList.FXlist p3, float p4, float p5)
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
}
