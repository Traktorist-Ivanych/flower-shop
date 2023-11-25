using UnityEngine;

namespace PlayerControl
{
    [RequireComponent (typeof(PlayerMoving))]
    public class PlayerBusyness : MonoBehaviour
    {
        [SerializeField] private PlayerMoving playerMoving;
        [field: SerializeField] public bool IsPlayerFree { get; private set; }

        private void OnValidate()
        {
            playerMoving = GetComponent<PlayerMoving>();
        }

        private void OnEnable()
        {
            playerMoving.PlayerHasArrivedEvent += SetPlayerBusy;
        }

        private void OnDisable()
        {
            playerMoving.PlayerHasArrivedEvent -= SetPlayerBusy;
        }

        public void SetPlayerBusy()
        {
            IsPlayerFree = false;
        }

        public void SetPlayerFree()
        {
            IsPlayerFree = true;
        }
    }
}
