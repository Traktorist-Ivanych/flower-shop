using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [System.Serializable]
    public class PotsRackForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public int TableLvl { get; private set; }
        
        public PotsRackForSaving() {}

        public PotsRackForSaving(int tableLvl)
        {
            IsValuesSaved = true;
            TableLvl = tableLvl;
        }
    }
}