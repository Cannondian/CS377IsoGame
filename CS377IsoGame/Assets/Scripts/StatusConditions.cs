using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusConditions
{
    public enum statusList
    {
        Burning, //generic debuff that inflicts non-negligible DoT, for scaling with the entity's max HP. 
        Corrosive, //generic stacking debuff that reduces defense.
        Bleeding, //generic stacking debuff that reduces incoming healing and deals non-negligible damage in reaching high stack-counts.
        Confused, //generic debuff that swaps the players input controls with one another.
        Stunned, //generic debuff which incapacitates the entity for a variable duration, preventing them from moving or taking action.
        Slow, //generic debuff that slows the entity's movement by a percentage of their original movement speed.
        Exhausted, //debuff that reduces the energy (or any applicable resource) regeneration rate of the entity.
        Rejuvenation, //generic buff produced by the alien plant Tile. It applies a minimal HoT (heal over time)
        Energized, // generic buff produced by the mech Tile. It increases the energy (or any applicable resource) regeneration rate.
        SlipperySteps, //generic buff produced by the ice/snow tile. It increases the movement speed of the entity by a percentage of their original movement speed.
        SmolderingStrikes, //generic buff produced by the fire Tile. Adds a flat damage increase to every attack, however also imposes a flat health cost upon each.
        Evasive, // generic buff produced by the dust/sand tile, it causes attacks against this entity to have a fixed chance of missing. 
        DefensiveTerrain, // generic buff produced by the rock formations Tile, gives a scaling increase to the entity's defense
        RadiationPoisoning, //debuff that applies a random generic debuff at a set interval. Produced by the radioactive puddle Tile.
        Mutating, // buff that applies two random generic buffs from those available through tiles. This buff changes at set intervals and is also applied by the radioactive puddle Tile
        Glowing, // generic buff produced by the NeonStrips Tile. It cleanses two debuffs upon application and makes the player immune to non-CC debuffs for its duration.
        OneWithTheWorld, //TBD
        Unstoppable, //TBD
        Hacked, //TBD
        Blinded //TBD
        
    }
}








    

