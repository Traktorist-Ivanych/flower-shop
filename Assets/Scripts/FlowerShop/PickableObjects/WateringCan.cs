using FlowerShop.Flowers;
using FlowerShop.PickableObjects.Moving;
using FlowerShop.Tables;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.PickableObjects
{
    [RequireComponent(typeof(ObjectMoving))]
    public class WateringCan : MonoBehaviour, IPickableObject
    {
        [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
        [Inject] private readonly TablesSettings tablesSettings;
        [Inject] private readonly PlayerComponents playerComponents;
    
        [SerializeField] private Transform wateringCanIndicatorTransform;
        [SerializeField] private Mesh[] wateringCanLvlMeshes = new Mesh[2];

        [HideInInspector, SerializeField] private ObjectMoving objectMoving;
        [HideInInspector, SerializeField] private MeshFilter wateringCanMeshFilter;
        [HideInInspector, SerializeField] private MeshRenderer wateringCanIndicatorMeshRenderer;
    
        private int maxWateringsNumber;
        private int wateringCanLvl;

        [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
        
        public int CurrentWateringsNumber { get; private set; }

        private void OnValidate()
        {
            objectMoving = GetComponent<ObjectMoving>();
            wateringCanMeshFilter = GetComponent<MeshFilter>();
            wateringCanIndicatorMeshRenderer = wateringCanIndicatorTransform.GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            maxWateringsNumber = tablesSettings.WateringsNumber;
            CurrentWateringsNumber = maxWateringsNumber;
        }

        public void TakeInPlayerHandsAndSetPlayerFree()
        {
            playerPickableObjectHandler.CurrentPickableObject = this;
            objectMoving.MoveObject(
                targetFinishTransform: playerComponents.PlayerHandsForBigObjectTransform, 
                movingObjectAnimatorTrigger: PlayerAnimatorParameters.TakeBigObjectTrigger, 
                setPlayerFree: true);
            wateringCanIndicatorMeshRenderer.enabled = true;
            UpdateWateringCanIndicator();
        }

        public void PutOnTableAndSetPlayerFree(Transform targetTransform) 
        {
            objectMoving.MoveObject(
                targetFinishTransform: targetTransform, 
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
            return tablesSettings.ReplenishWateringCanTime * (maxWateringsNumber - CurrentWateringsNumber) / maxWateringsNumber;
        }

        public void ReplenishWateringCan()
        {
            CurrentWateringsNumber = maxWateringsNumber;
        }

        public void UpgradeWateringCan()
        {
            wateringCanLvl++;
            maxWateringsNumber = tablesSettings.WateringsNumber + tablesSettings.WateringsNumberLvlDelta * wateringCanLvl;
            CurrentWateringsNumber = maxWateringsNumber;
            wateringCanMeshFilter.mesh = wateringCanLvlMeshes[wateringCanLvl - 1];
        }

        private void UpdateWateringCanIndicator()
        {
            float indicatorScaleZ = tablesSettings.WateringIndicatorChangeablePart *
                CurrentWateringsNumber / maxWateringsNumber +
                tablesSettings.WateringIndicatorUnchangeablePart;
            wateringCanIndicatorTransform.localScale = new Vector3(1, 1, indicatorScaleZ);
        }
    }
}
