using FlowerShop.Settings;
using UniRx;
using UnityEngine;
using Zenject;

namespace FlowerShop.Effects
{
    public class ActionProgressbar : MonoBehaviour
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;

        [SerializeField] private MeshRenderer mainIndicatorMeshRenderer;
        [SerializeField] private MeshRenderer progressIndicatorMeshRenderer;

        [HideInInspector, SerializeField] private Transform mainIndicatorTransform;
        [HideInInspector, SerializeField] private Transform progressIndicatorTransform;

        private float currentProgressTime;
        private float endProgressTime;

        private readonly CompositeDisposable progressbarCompositeDisposable = new();

        private void OnValidate()
        {
            mainIndicatorTransform = mainIndicatorMeshRenderer.GetComponent<Transform>();
            progressIndicatorTransform = progressIndicatorMeshRenderer.GetComponent<Transform>();
        }

        public void EnableActionProgressbar(float expectedProgressTime)
        {
            endProgressTime = expectedProgressTime;
            currentProgressTime = 0;

            EnableActionProgressbarMain();
        }

        public void EnableActionProgressbar(float expectedProgressTime, float progressTime)
        {
            endProgressTime = expectedProgressTime;
            currentProgressTime = progressTime;

            EnableActionProgressbarMain();
        }

        private void EnableActionProgressbarMain()
        {
            mainIndicatorMeshRenderer.enabled = true;
            progressIndicatorMeshRenderer.enabled = true;

            mainIndicatorTransform.rotation = Quaternion.Euler(actionsWithTransformSettings.ConstantProgressbarRotation);

            UpdateProgressbarTransform(currentProgressTime / endProgressTime);

            Observable.EveryUpdate().Subscribe(updateProgressbar =>
            {
                currentProgressTime += Time.deltaTime;

                UpdateProgressbarTransform(currentProgressTime / endProgressTime);

                if (currentProgressTime >= endProgressTime)
                {
                    mainIndicatorMeshRenderer.enabled = false;
                    progressIndicatorMeshRenderer.enabled = false;

                    progressbarCompositeDisposable.Clear();
                }
            }).AddTo(progressbarCompositeDisposable);
        }

        private void UpdateProgressbarTransform(float zCoordinate)
        {
            progressIndicatorTransform.localScale = new Vector3(
                progressIndicatorTransform.transform.localScale.x,
                progressIndicatorTransform.transform.localScale.y,
                zCoordinate);
        }
    }
}
