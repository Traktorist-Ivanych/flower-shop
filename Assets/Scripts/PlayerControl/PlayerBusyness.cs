using UnityEngine;

[RequireComponent (typeof(PlayerMoving))]
public class PlayerBusyness : MonoBehaviour
{
    // can be moved to IsPlayerFree property
    [SerializeField] private bool isPlayerFree;

    private PlayerMoving playerMoving;

    public bool IsPlayerFree
    {
        get { return isPlayerFree; }
    }

    private void Start()
    {
        playerMoving = GetComponent<PlayerMoving>();
        playerMoving.PlayerHasArrivedEvent += SetPlayerBusy;
    }

    public void SetPlayerBusy()
    {
        isPlayerFree = false;
    }

    public void SetPlayerFree()
    {
        isPlayerFree = true;
    }
}
