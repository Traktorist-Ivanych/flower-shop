using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Moving;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class WateringCan : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly GameConfiguration gameConfiguration;
        [Inject] private readonly PlayerComponents playerComponents;
    
        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    
        [SerializeField] private Transform wateringCanIndicatorTransform;
        [SerializeField] private Mesh[] wateringCanLvlMeshes = new Mesh[2];

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        [HideInInspector, SerializeField] private MeshFilter wateringCanMeshFilter;
        [HideInInspector, SerializeField] private MeshRenderer wateringCanIndicatorMeshRenderer;
    
        private int maxWateringsNumber;
        private int wateringCanLvl;

        public int CurrentWateringsNumber { get; private set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            wateringCanMeshFilter = GetComponent<MeshFilter>();
            wateringCanIndicatorMeshRenderer = wateringCanIndicatorTransform.GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            maxWateringsNumber = gameConfiguration.WateringsNumber;
            CurrentWateringsNumber = maxWateringsNumber;
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            objectMoving.MoveObject(targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                                    movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                                    setPlayerFree: true);
            wateringCanIndicatorMeshRenderer.enabled = true;
            UpdateWateringCanIndicator();
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform) 
        {
            objectMoving.MoveObject(targetFinishTransform: targetTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.GiveBigObjectTrigger, 
                setPlayerFree: true);
            wateringCanIndicatorMeshRenderer.enabled = false;
        }

        public void PourPotWithWateringCan()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.PourTrigger);
            CurrentWateringsNumber--;
            UpdateWateringCanIndicator();
        }

        public bool IsWateringCanNeedForReplenish()
        {
            return CurrentWateringsNumber < maxWateringsNumber;
        }

        public float ReplenishWateringCanTime()
        {
            return gameConfiguration.ReplenishWateringCanTime * (maxWateringsNumber - CurrentWateringsNumber) / maxWateringsNumber;
        }

        public void ReplenishWateringCan()
        {
            CurrentWateringsNumber = maxWateringsNumber;
        }

        public void ImproveWateringCan()
        {
            wateringCanLvl++;
            maxWateringsNumber = gameConfiguration.WateringsNumber + gameConfiguration.WateringsNumberLvlDelta * wateringCanLvl;
            CurrentWateringsNumber = maxWateringsNumber;
            wateringCanMeshFilter.mesh = wateringCanLvlMeshes[wateringCanLvl - 1];
        }

        private void UpdateWateringCanIndicator()
        {
            float indicatorScaleZ = 0.9f * CurrentWateringsNumber / maxWateringsNumber + 0.1f;
            wateringCanIndicatorTransform.localScale = new Vector3(1, 1, indicatorScaleZ);
        }
    }
}
