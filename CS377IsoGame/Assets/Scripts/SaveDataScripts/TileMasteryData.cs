using System;
using System.Collections.Generic;

[Serializable]
    public class TileMasteryData
    {
        public float masteryOverVelheret;
        public float masteryOverShalharan;
        public float masteryOverIlsihre;
        public bool IlsihreMod1;
        public bool IlsihreMod2;
        public bool VelheretMod1;
        public bool VelheretMod2;
        public bool ShalharanMod1;
        public bool ShalharanMod2;

        public List<ModifierVault.IModifier> TEEList;
        
        public TileMasteryData(){}
        
        public TileMasteryData(TileMastery tileMastery)
        {

            masteryOverIlsihre = tileMastery.masteryOverIlsihre;
            masteryOverShalharan = tileMastery._masteryOverShalharan;
            masteryOverVelheret = tileMastery.masteryOverVelheret;
            IlsihreMod1 = tileMastery.IlsihreMod1;
            IlsihreMod2 = tileMastery.IlsihreMod2;
            VelheretMod1 = tileMastery.VelheretMod1;
            VelheretMod2 = tileMastery.VelheretMod2;
            ShalharanMod1 = tileMastery.ShalharanMod1;
            ShalharanMod2 = tileMastery._ShalharanMod2;
            TEEList = tileMastery.appliedTEE;

        }
        
        
        
    }
    
