using UnityEngine;
using Zenject;

namespace PlayerControl
{
    [RequireComponent (typeof(PlayerMoving))]
    public class PlayerAbilityExecutor : MonoBehaviour
    {
        [Inject] private readonly PlayerMoving playerMoving;
    
        private IPlayerAbility playerAbility;
    
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
}
