public enum StatModiferType
{
   Flat,
   Percent, 
   FlatContinuous,
   PercentContinuous
}
public class StatModifier
{
   public readonly float value;
   public readonly StatModiferType myType;

   public StatModifier(float val, StatModiferType type)
   {
      value = val;
      myType = type;
   }
}
