using UnityEngine;

[RequireComponent (typeof(PlayerMoving))]
public class PlayerAbilityExecutor : MonoBehaviour
{
    [SerializeField] private PlayerMoving playerMoving;
    
    private IPlayerAbility playerAbility;
    
    private void OnValidate()
    {
        playerMoving = GetComponent<PlayerMoving>();
    }
    
    private void OnEnable()
    {
        playerMoving.PlayerHasArrivedEvent += ExecutePlayerAbility;
    }

    private void OnDisable()
    {
        playerMoving.PlayerHasArrivedEvent -= ExecutePlayerAbility;
    }

    public void SetIPlayerAbility(IPlayerAbility transmittedPlayerAbility)
    {
        playerAbility = transmittedPlayerAbility;
    }

    private void ExecutePlayerAbility()
    {
        playerAbility?.ExecutePlayerAbility();
        playerAbility = null;
    }
}
