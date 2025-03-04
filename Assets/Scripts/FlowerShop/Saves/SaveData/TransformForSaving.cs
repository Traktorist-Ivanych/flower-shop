﻿using UnityEngine;

namespace FlowerShop.Saves.SaveData
{
    [System.Serializable]
    public struct TransformForSaving
    {
        [field: SerializeField] public bool IsValuesSaved { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public Quaternion Rotation { get; private set; }
            
        public TransformForSaving(Transform transmittedTransform)
        {
            Position = transmittedTransform.position;
            Rotation = transmittedTransform.rotation;
            IsValuesSaved = true;
        }
    }
}