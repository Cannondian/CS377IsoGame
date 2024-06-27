using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ModifierVault
{
    public static List<IModifier> Prohibited = new List<IModifier>();
    public static List<TileEffectEnhancement> TEEModifiers;

    public interface IModifier
    {
        void ApplyModifier();
    }

    public enum RareMechanicModifiers
    {
        Adaptability, //allows the player to hold two tile buffs at once
        Specialization, //give up on one skill to greatly buff the other
        Boost //increase the rarity of all tile stat increase modifiers (currently obtained and going forward)
        
    }
    
    public struct TileStatModifier : IModifier
    {
        public int myRarity; //ranges from 1-3, from most to least common and least to most intensity
        public TileElement.ElementType myStat;
        public TileMastery playerMastery;
        public TileStatModifier(int rarity, TileElement.ElementType stat, TileMastery masterySheet)
        {
            myRarity = rarity;
            myStat = stat;
            playerMastery = masterySheet;
        }

        public void ApplyModifier()
        {
            switch (myStat)
            {
                case TileElement.ElementType.Shalharan:
                    playerMastery._masteryOverShalharan += myRarity * 10;
                    break;
                case TileElement.ElementType.Ilsihre:
                    playerMastery.masteryOverIlsihre += myRarity * 10;
                    break;
                case TileElement.ElementType.Velheret:
                    playerMastery.masteryOverVelheret += myRarity * 10;
                    break;
            }
        }
    }

    public struct TileEffectEnhancement: IModifier
    {
        public TileElement.ElementType element;
        public int modIndex;

        public TileEffectEnhancement(TileElement.ElementType elem, int modI)
        {
            element = elem;
            modIndex = modI;

        }

        public void ApplyModifier()
        {
            switch (element)
            {
                case TileElement.ElementType.Ilsihre:
                    if (modIndex == 1)
                    {
                        
                        TileMastery.Instance.IlsihreMod1 = true;
                    }
                    else
                    {
                        TileMastery.Instance.IlsihreMod2 = true;

                    }

                    break;
                case TileElement.ElementType.Shalharan:
                    if (modIndex == 1)
                    {
                        TileMastery.Instance.ShalharanMod1 = true;
                    }
                    else
                    {
                        TileMastery.Instance._ShalharanMod2 = true;
                    }
                    break;
                case TileElement.ElementType.Velheret:
                    if (modIndex == 1)
                    {
                        TileMastery.Instance.VelheretMod1 = true;
                    }
                    else
                    {
                        TileMastery.Instance.VelheretMod2 = true;
                    }
                    
                    break;
                    
            }
                
        }
    }

    public struct RareMechanicModifier : IModifier
    {
        public RareMechanicModifiers variety;

        public RareMechanicModifier(RareMechanicModifiers var)
        {
            variety = var;
        }

        public void ApplyModifier()
        {
            throw new System.NotImplementedException();
        }
    } 
    public static List<IModifier> GenerateModifiers(List<IModifier> listToFill)
    {
        var r = new Random();
        if (listToFill.Count != 3)
        {
            for (int i = 0; i < 3; i++)
            {
                var pick = r.Next(1, 100);
                if (pick <= 70)
                {
                    var pickRarity = r.Next(1, 11);
                    if (pickRarity <= 5)
                    {
                        pickRarity = 1;
                    }
                    else if (pickRarity <= 8)
                    {
                        pickRarity = 2;
                    }
                    else
                    {
                        pickRarity = 3;
                    }

                    TileElement.ElementType pickElement = (TileElement.ElementType)r.Next(0, 3);
                    var basicModifier = new TileStatModifier(pickRarity, pickElement, TileMastery.Instance);
                    if (listToFill.Contains(basicModifier))
                    {
                        return GenerateModifiers(listToFill);

                    }
                    else
                    {
                        listToFill.Add(basicModifier);
                        return GenerateModifiers(listToFill);
                    }
                }
                else if (pick <= 100)
                {
                    var pickElement = (TileElement.ElementType)r.Next(0, 3);
                    var pickMod = r.Next(1, 3);
                    var intermediateModifier = new TileEffectEnhancement(pickElement, pickMod);
                    if (listToFill.Contains(intermediateModifier) || Prohibited.Contains(intermediateModifier))
                    {
                        return GenerateModifiers(listToFill);
                    }
                    else
                    {
                        listToFill.Add(intermediateModifier);
                        Prohibited.Add(intermediateModifier);
                        return GenerateModifiers(listToFill);
                    }
                }
                else
                {
                    RareMechanicModifiers pickVariety = (RareMechanicModifiers)r.Next(0, 2);
                    var advancedModifier = new RareMechanicModifier(pickVariety);
                    if (listToFill.Contains(advancedModifier) || Prohibited.Contains(advancedModifier))
                    {
                        return GenerateModifiers(listToFill);
                    }
                    else
                    {
                        listToFill.Add(advancedModifier);
                        Prohibited.Add(advancedModifier);
                        return GenerateModifiers(listToFill);
                    }

                }

            }
        }
        return listToFill;
    }
    

    // Update is called once per frame
    
}
