using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelStrip : MonoBehaviour
{
    public GameObject[] reels;                // Array of reel GameObjects
    public GameObject[] symbolPrefabs;        // Array of different symbol prefabs to choose from
    public float distanceBetweenSymbols = -85f; // Distance between each symbol on the reel
    public int symbolsToCreate = 3;           // Number of symbols to create after the last one

    private List<GameObject[]> slots = new List<GameObject[]>(); // List of arrays to store symbols for each reel

    // Start method is now private and is handled internally by Unity
    private void Start()
    {
        InitializeReels();
    }

    // Public method to initialize the reels
    public void InitializeReels()
    {
        foreach (var reel in reels)
        {
            CreateReelStrip(reel);
        }
    }

    private void CreateReelStrip(GameObject reel)
    {
        int symbolCount = reel.transform.childCount;
        GameObject[] reelSlots = new GameObject[symbolCount + symbolsToCreate]; // Adjust array size to accommodate new symbols

        // Copy existing symbols in the reel
        for (int i = 0; i < symbolCount; i++)
        {
            reelSlots[i] = reel.transform.GetChild(i).gameObject;
        }

        // Instantiate the specified number of symbols after the last existing symbol
        Vector3 lastPosition = reelSlots[symbolCount - 1].transform.localPosition;
        for (int i = 0; i < symbolsToCreate; i++)
        {
            // Randomly select a symbol prefab from the array
            GameObject randomSymbolPrefab = symbolPrefabs[Random.Range(0, symbolPrefabs.Length)];

            // Calculate the new position for the symbol
            Vector3 newPosition = lastPosition;
            newPosition.y += distanceBetweenSymbols * (i + 1); // Apply the distance for each new symbol

            // Instantiate the symbol and add it to the reelSlots array
            reelSlots[symbolCount + i] = Instantiate(randomSymbolPrefab, reel.transform);
            reelSlots[symbolCount + i].transform.localPosition = newPosition;
        }

        // Add the reelSlots to the slots list for future reference
        slots.Add(reelSlots);
    }
}
