namespace FlowerShop.Fertilizers
{
    public class GrowerToMaxLvl : Fertilizer
    {
        private protected override void FinishTreatingPot()
        {
            base.FinishTreatingPot();

            potForTreating.TreatPotByGrowerToMaxLvl();
        }
    }
}