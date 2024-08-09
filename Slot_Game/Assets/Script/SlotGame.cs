using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For button interaction

public class SlotGame : MonoBehaviour
{
    public ReelStrip reelStrip;
    public float moveDuration = 3f; // Duration for the entire movement
    public float bouncyIntensity = 10f; // Intensity of the bouncy effect
    public Button rotateButton; // Button to trigger rotation

    public List<float> customStopPositions; // Define custom stop positions for reels

    private bool isMoving = false;

    private void Start()
    {
        // Set the number of symbols to create on each reel
        reelStrip.symbolsToCreate = 100;

        // Initialize the reels
        reelStrip.InitializeReels();

        // Add listener to the button
        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(OnRotateButtonClicked);
        }
        else
        {
            Debug.LogError("Rotate Button is not assigned.");
        }
    }

    private void OnRotateButtonClicked()
    {
        if (!isMoving)
        {
            StartCoroutine(RotateReels());
        }
    }

    private IEnumerator RotateReels()
    {
        // Reset reel positions before starting rotation
        ResetReelPositions();

        // Start moving the symbols
        yield return StartCoroutine(MoveSymbols());
    }

    private void ResetReelPositions()
    {
        foreach (var reel in reelStrip.reels)
        {
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Reset the position of each reel to its starting position
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, rectTransform.localPosition.z);
            }
        }
    }

    private IEnumerator MoveSymbols()
    {
        isMoving = true;

        // Create an array of LeanTween tweens for each reel
        List<LTDescr> reelTweens = new List<LTDescr>();

        // Start a LeanTween tween for each reel
        foreach (var reel in reelStrip.reels)
        {
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                Debug.LogError("Reel does not have a RectTransform component.");
                continue;
            }

            Vector3 startPosition = rectTransform.localPosition;
            float reelHeight = reelStrip.distanceBetweenSymbols * reelStrip.symbolsToCreate;
            float endPositionY = startPosition.y - reelHeight;

            // Calculate the stop position
            float stopPositionY = customStopPositions.Count > 0 ? GetCustomStopPosition(rectTransform) : endPositionY;

            // Move each reel using LeanTween
            LTDescr tween = LeanTween.moveLocalY(rectTransform.gameObject, endPositionY, moveDuration)
                .setEase(LeanTweenType.easeInOutQuad) // Smooth easing function
                .setOnComplete(() =>
                {
                    // Correct the final position to the custom stop position (or end position if no custom stop)
                    LeanTween.moveLocalY(rectTransform.gameObject, stopPositionY, 0.2f)
                        .setEase(LeanTweenType.easeInOutQuad)
                        .setOnComplete(() => OnReelStop(rectTransform.gameObject)); // Apply bouncy effect
                });

            reelTweens.Add(tween);
        }

        // Wait for all tweens to complete
        foreach (var tween in reelTweens)
        {
            yield return tween;
        }

        isMoving = false;
    }

    private void OnReelStop(GameObject reel)
    {
        RectTransform rectTransform = reel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector3 originalPosition = rectTransform.localPosition;

            // Apply a bouncy effect when the reel stops
            LeanTween.moveLocalY(rectTransform.gameObject, originalPosition.y + bouncyIntensity, 0.2f)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() =>
                {
                    LeanTween.moveLocalY(rectTransform.gameObject, originalPosition.y, 0.3f)
                        .setEase(LeanTweenType.easeInQuad);
                });
        }
    }

    private float GetCustomStopPosition(RectTransform rectTransform)
    {
        // Custom logic to determine the stop position
        // For simplicity, use a predefined stop position from the list
        if (customStopPositions.Count > 0)
        {
            // Example: return a random custom stop position
            int index = Random.Range(0, customStopPositions.Count);
            return customStopPositions[index];
        }
        else
        {
            // Default behavior if no custom positions are defined
            return rectTransform.localPosition.y;
        }
    }

    // Call this method to stop moving the symbols
    public void StopMoving()
    {
        isMoving = false;
        // Optional: Stop all LeanTween animations if needed
        LeanTween.cancelAll();
    }
}
