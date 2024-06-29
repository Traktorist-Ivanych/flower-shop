namespace FlowerShop.Fertilizers
{
    public class GrowingLvlIncreaser : Fertilizer
    {
        private protected override void FinishTreatingPot()
        {
            base.FinishTreatingPot();
            
            potForTreating.TreatPotByGrowingLvlIncreaser();
        }
    }
}