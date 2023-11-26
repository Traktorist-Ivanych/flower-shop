using System.Collections.Generic;
using UnityEngine;

public class FlowersForCollection : MonoBehaviour
{
    private readonly List<FlowerInfo> flowerCollection = new();

    public void AddFlowerToCollectionList(FlowerInfo addedFlowerInfo)
    {
        flowerCollection.Add(addedFlowerInfo);
    }

    public bool IsFlowerForCollectionUnique(FlowerInfo verifiableFlowerInfo)
    {
        return !flowerCollection.Contains(verifiableFlowerInfo);
    }
}
