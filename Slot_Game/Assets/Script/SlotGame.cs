using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanTween; // Make sure LeanTween is properly included

public class SlotGame : MonoBehaviour
{
    public ReelStrip reelStrip;
    public float moveSpeed = 50f;  // Speed at which the symbols move
    public float moveDuration = 3f; // Duration for the entire movement
    public float bouncyIntensity = 1f; // Intensity of the bouncy effect
    private bool isMoving = false;

    private void Start()
    {
        // Set the number of symbols to create on each reel
        reelStrip.symbolsToCreate = 100;

        // Initialize the reels by calling the public method
        reelStrip.InitializeReels();

        // Start moving the symbols
        StartCoroutine(MoveSymbols());
    }

    private IEnumerator MoveSymbols()
    {
        isMoving = true;

        // Create an array of LeanTween tweens for each reel
        List<LTDescr> reelTweens = new List<LTDescr>();

        // Start a LeanTween tween for each reel
        foreach (var reel in reelStrip.reels)
        {
            // Use LeanTween to move each reel
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                Debug.LogError("Reel does not have a RectTransform component.");
                continue;
            }

            Vector3 startPosition = rectTransform.localPosition;
            Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - (reelStrip.distanceBetweenSymbols * reelStrip.symbolsToCreate), startPosition.z);

            LTDescr tween = LeanTween.moveLocalY(rectTransform.gameObject, endPosition.y, moveDuration)
                .setEase(LeanTweenType.easeInOutQuad) // Smooth easing function
                .setOnComplete(() => OnReelStop(rectTransform.gameObject)); // Trigger bouncy effect on completion

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
        // Apply bouncy effect when the reel stops
        RectTransform rectTransform = reel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            LeanTween.moveLocalY(rectTransform.gameObject, rectTransform.localPosition.y + bouncyIntensity, 0.5f)
                .setEase(LeanTweenType.easeBounceOut)
                .setOnComplete(() =>
                {
                    // Optional: Further actions after bouncy effect completion
                });
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
