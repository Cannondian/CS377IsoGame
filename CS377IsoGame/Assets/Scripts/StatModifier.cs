public enum StatModifierType
{
   Flat,
   Percent, 
   FlatContinuous,
   PercentContinuous
}
public class StatModifier
{
   public readonly float value;
   public readonly StatModifierType myType;

   public StatModifier(float val, StatModifierType type)
   {
      value = val;
      myType = type;
   }
}
