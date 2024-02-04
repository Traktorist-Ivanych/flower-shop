using UnityEngine;

namespace FlowerShop.Weeds
{
    [CreateAssetMenu(fileName = "Weed", menuName = "Weed", order = 53)]
    public class Weed : ScriptableObject
    {
        [SerializeField] private Mesh[] weedLvlMeshes = new Mesh[3];

        public Mesh GetWeedLvlMesh(int weedLvl)
        {
            return weedLvlMeshes[weedLvl - 1];
        }
    }
}
