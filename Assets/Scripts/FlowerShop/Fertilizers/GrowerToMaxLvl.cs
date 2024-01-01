using FlowerShop.PickableObjects;

namespace FlowerShop.Fertilizers
{
    public class GrowerToMaxLvl : Fertilizer
    {
        public override void TreatPot(Pot potForTreating)
        {
            base.TreatPot(potForTreating);
            
            potForTreating.TreatPotByGrowerToMaxLvl();
        }
    }
}