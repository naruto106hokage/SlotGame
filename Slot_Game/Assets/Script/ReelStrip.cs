using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelStrip : MonoBehaviour
{
    public GameObject[] reels;
    public float distanceBetweenSymbols = -85f;
    public int symbolsToCreate = 1;

    public void InitializeReels()
    {
        foreach (var reel in reels)
        {
            RectTransform rectTransform = reel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                for (int i = 0; i < symbolsToCreate; i++)
                {
                    GameObject symbol = Instantiate(reel, rectTransform);
                    symbol.transform.SetParent(reel.transform, false);
                    symbol.transform.localPosition = new Vector3(0, -distanceBetweenSymbols * i, 0);
                }
            }
        }
    }

    private GameObject Instantiate(GameObject prefab, RectTransform parent)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(parent, false);
        return instance;
    }
}
