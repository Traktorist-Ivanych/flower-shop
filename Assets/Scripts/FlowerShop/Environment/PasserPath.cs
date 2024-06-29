using UnityEngine;

namespace FlowerShop.Environment
{
    public class PasserPath : MonoBehaviour
    {
        [SerializeField] private Transform[] pathTransforms;

        public Transform GetSpawnTransform()
        {
            return pathTransforms[0];
        }

        public Transform GetNextPathTransform(int pathIndex)
        {
            return pathTransforms[pathIndex];
        }

        public bool IsPasserOnEndTransform(int pathIndex)
        {
            return pathIndex >= pathTransforms.Length - 1;
        }

        private void OnDrawGizmos()
        {
            if (pathTransforms.Length > 1)
            {
                Gizmos.color = Color.yellow;
                Vector3[] pathPoints = new Vector3[pathTransforms.Length];
                for (int i = 0; i < pathTransforms.Length; i++)
                {
                    pathPoints[i] = pathTransforms[i].position;
                }
                Gizmos.DrawLineStrip(pathPoints, false);
            }
        }
    }
}
