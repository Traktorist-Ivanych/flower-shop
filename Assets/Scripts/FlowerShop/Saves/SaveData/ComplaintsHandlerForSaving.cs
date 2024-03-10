using System;
using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [Serializable]
    public struct ComplaintsHandlerForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public bool IsComplaintActive { get; private set; }
        [field: SerializeField] public float CurrentComplaintTime { get; private set; }
        [field: SerializeField] public float CurrentComplaintHandleTime { get; private set; }
        [field: SerializeField] public int ComplaintDescriptionIndex { get; private set; }
        [field: SerializeField] public string ComplaintFlowerInfoUniqueKey { get; private set; }

        public ComplaintsHandlerForSaving(float currentComplaintTime, float currentComplaintHandleTime,
            bool isComplaintActive, int complaintDescriptionIndex, string complaintFlowerInfoUniqueKey)
        {
            IsValuesSaved = true;
            CurrentComplaintTime = currentComplaintTime;
            CurrentComplaintHandleTime = currentComplaintHandleTime;
            IsComplaintActive = isComplaintActive;
            ComplaintDescriptionIndex = complaintDescriptionIndex;
            ComplaintFlowerInfoUniqueKey = complaintFlowerInfoUniqueKey;
        }
    }
}