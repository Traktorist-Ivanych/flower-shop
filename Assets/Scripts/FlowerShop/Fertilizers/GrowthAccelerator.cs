using FlowerShop.PickableObjects;

namespace FlowerShop.Fertilizers
{
    public class GrowthAccelerator : Fertilizer
    {
        public override void TreatPot(Pot potForTreating)
        {
            base.TreatPot(potForTreating);
            
            potForTreating.TreatPotByGrothAccelerator();
        }
    }
}