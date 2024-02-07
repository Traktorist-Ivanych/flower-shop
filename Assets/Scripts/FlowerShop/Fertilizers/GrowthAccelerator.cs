using FlowerShop.PickableObjects;

namespace FlowerShop.Fertilizers
{
    public class GrowthAccelerator : Fertilizer
    {
        private protected override void FinishTreatingPot()
        {
            base.FinishTreatingPot();

            potForTreating.TreatPotByGrothAccelerator();
        }
    }
}