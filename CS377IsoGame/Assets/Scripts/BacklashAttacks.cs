

using System;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class BacklashAttacks: MonoBehaviour
{
   private bool VHFlamethrowerOn;
   public TileMastery tileMastery;
   [SerializeField] private StatsTemplate playerStats;
   
   private UnityAction<bool> FlamethrowerState;
   private void OnEnable()
   {
      FlamethrowerState += FlamethrowerUpdate;
      EventBus.StartListening(EventTypes.Events.ON_VH_FLAMETHROWER_ENABLED, FlamethrowerState);
      EnemyAI.OnPlayerHit += Backlash;

   }

   private void Start()
   {
      tileMastery = TileMastery.Instance;
   }
   //takes in 4 pieces of input
   // - when the  VHFlamethrower event is called, the BacklashAttacks class reacts by saving the state of the FT
   // - then when the player is damaged the actual backlash function is called
   // - this function checks if VHMod2 is enabled
   // - and temporary health to determine if a backlash will happen

   private void FlamethrowerUpdate(bool on)
   {
      VHFlamethrowerOn = on;
   }

   private void Backlash(EnemyAI damageSource)
   {
      if (tileMastery.VelheretMod2 && VHFlamethrowerOn && playerStats.bonusHealth > 0)
      {
         float damage = DamageCalculator.Instance.VHBacklashDamage();
         damageSource.GetComponent<StatsTemplate>().TakeDamage(damage);
         
         Transform damageTransform = damageSource.transform;
         var context = new EventTypes.BackLashHitParam(damageTransform);
         EventBus.TriggerEvent(EventTypes.Events.ON_VH_BACKLASH_HIT, context);
         EventBus.TriggerEvent(EventTypes.Events.ON_ENEMY_HIT, new EventTypes.FloatingDamageParam(damageSource.gameObject,
            damage,1, Damage.Types.Poison) );
         
      }
   }
}
