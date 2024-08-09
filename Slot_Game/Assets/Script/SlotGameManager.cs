using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For button interaction

public class SlotGameManager : MonoBehaviour
{
    public ReelStrip reelStrip;
    public GameObject winIndicator; // Reference to the GameObject that indicates a win
    public Button spinButton; // Reference to the button that triggers the spin
    public int winningLineCount = 3; // Number of lines to check for winning

    private void Start()
    {
        // Initialize winIndicator
        if (winIndicator != null)
        {
            winIndicator.SetActive(false); // Ensure it is hidden at the start
        }
        else
        {
            Debug.LogError("WinIndicator GameObject is not assigned.");
        }

        // Add listener to the spin button
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
        // Hide win indicator when spin is clicked
        HideWinIndicator();

        // Start the spinning process
        // Assuming you have a method to start the spin
        StartCoroutine(StartSpinning());
    }

    private IEnumerator StartSpinning()
    {
        // Your spinning logic here
        // Example:
        yield return new WaitForSeconds(3f); // Simulate spinning time

        // After spinning is done, check for winning
        CheckForWinningSymbols();
    }

    public void CheckForWinningSymbols()
    {
        // Check for winning symbols on all lines
        for (int line = 0; line < winningLineCount; line++)
        {
            if (IsWinningLine(line))
            {
                DisplayWinIndicator();
                return;
            }
        }
        // No win, hide the indicator
        HideWinIndicator();
    }

    private bool IsWinningLine(int lineIndex)
    {
        // Iterate over each reel and check if the symbols match
        string firstSymbol = GetSymbolAtLine(lineIndex, 0);
        if (firstSymbol == null) return false;

        for (int reelIndex = 1; reelIndex < reelStrip.reels.Length; reelIndex++)
        {
            string currentSymbol = GetSymbolAtLine(lineIndex, reelIndex);
            if (currentSymbol != firstSymbol)
            {
                return false; // Symbols do not match
            }
        }

        return true; // All symbols match
    }

    private string GetSymbolAtLine(int lineIndex, int reelIndex)
    {
        // Assuming you have a way to get the symbol at a specific line and reel
        // Modify this based on how you manage symbols in your ReelStrip class
        if (reelIndex >= 0 && reelIndex < reelStrip.reels.Length)
        {
            GameObject reel = reelStrip.reels[reelIndex];
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Get the symbol at the specified line
                // Modify this based on your symbol management
                // For example:
                // return reel.transform.GetChild(lineIndex).GetComponent<Symbol>().name;
                return reel.transform.GetChild(lineIndex).name; // Replace with your method to get the symbol name
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
