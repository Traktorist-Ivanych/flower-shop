using FlowerShop.PickableObjects;

namespace FlowerShop.Fertilizers
{
    public class GrowingLvlIncreaser : Fertilizer
    {
        public override void TreatPot(Pot potForTreating)
        {
            base.TreatPot(potForTreating);
            
            potForTreating.TreatPotByGrowingLvlIncreaser();
        }
    }
}