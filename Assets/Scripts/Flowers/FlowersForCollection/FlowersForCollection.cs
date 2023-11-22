using System.Collections.Generic;
using UnityEngine;

public class FlowersForCollection : MonoBehaviour
{
    private readonly List<Flower> flowerCollection = new();

    public void AddFlowerToCollectionList(Flower addedFlower)
    {
        flowerCollection.Add(addedFlower);
    }

    public bool IsFlowerForCollectionUnique(Flower verifiableFlower)
    {
        return !flowerCollection.Contains(verifiableFlower);
    }
}
