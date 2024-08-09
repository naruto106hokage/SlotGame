using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotGameManager : MonoBehaviour
{
    public ReelStrip reelStrip;
    public GameObject winIndicator;
    public Button spinButton;
    public int winningLineCount = 3;

    private void Start()
    {
        if (winIndicator != null)
        {
            winIndicator.SetActive(false);
        }
        else
        {
            Debug.LogError("WinIndicator GameObject is not assigned.");
        }

        if (spinButton != null)
        {
            spinButton.onClick.AddListener(OnSpinButtonClicked);
        }
        else
        {
            Debug.LogError("Spin Button is not assigned.");
        }
    }

    private void OnSpinButtonClicked()
    {
        HideWinIndicator();
        StartCoroutine(StartSpinning());
    }

    private IEnumerator StartSpinning()
    {
        yield return new WaitForSeconds(3f);
        CheckForWinningSymbols();
    }

    public void CheckForWinningSymbols()
    {
        for (int line = 0; line < winningLineCount; line++)
        {
            if (IsWinningLine(line))
            {
                DisplayWinIndicator();
                return;
            }
        }
        HideWinIndicator();
    }

    private bool IsWinningLine(int lineIndex)
    {
        string firstSymbol = GetSymbolAtLine(lineIndex, 0);
        if (firstSymbol == null) return false;

        for (int reelIndex = 1; reelIndex < reelStrip.reels.Length; reelIndex++)
        {
            string currentSymbol = GetSymbolAtLine(lineIndex, reelIndex);
            if (currentSymbol != firstSymbol)
            {
                return false;
            }
        }

        return true;
    }

    private string GetSymbolAtLine(int lineIndex, int reelIndex)
    {
        if (reelIndex >= 0 && reelIndex < reelStrip.reels.Length)
        {
            GameObject reel = reelStrip.reels[reelIndex];
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                return reel.transform.GetChild(lineIndex).name;
            }
        }

        return null;
    }

    private void DisplayWinIndicator()
    {
        if (winIndicator != null)
        {
            winIndicator.SetActive(true);
        }
    }

    private void HideWinIndicator()
    {
        if (winIndicator != null)
        {
            winIndicator.SetActive(false);
        }
    }
}
