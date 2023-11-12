using UnityEngine;

[RequireComponent (typeof(PlayerMoving))]
public class PlayerAbilityExecutor : MonoBehaviour
{
    private PlayerMoving playerMoving;
    private IPlayerAbility playerAbility;

    private void Start()
    {
        playerMoving = GetComponent<PlayerMoving>();
        playerMoving.PlayerHasArrivedEvent += ExecutePlayerAbility;
    }

    public void SetIPlayerAbility(IPlayerAbility transmittedPlayerAbility)
    {
        playerAbility = transmittedPlayerAbility;
    }

    public void ExecutePlayerAbility()
    {
        playerAbility?.ExecutePlayerAbility();
        playerAbility = null;
    }
}
