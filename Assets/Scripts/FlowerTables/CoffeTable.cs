using UnityEngine;

public class CoffeTable : FlowerTable
{
    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && playerDinamicObject.IsPlayerDinamicObjectNull())
        {
            
        }
    }

    public override void ExecutePlayerAbility()
    {
        
    }
}
