using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotGame : MonoBehaviour
{
    public ReelStrip reelStrip;
    public float moveDuration = 3f;
    public float bouncyIntensity = 10f;
    public Button rotateButton;

    public List<float> customStopPositions;

    private bool isMoving = false;

    private void Start()
    {
        reelStrip.symbolsToCreate = 100;
        reelStrip.InitializeReels();

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
        ResetReelPositions();
        yield return StartCoroutine(MoveSymbols());
    }

    private void ResetReelPositions()
    {
        foreach (var reel in reelStrip.reels)
        {
            RectTransform rectTransform = reel.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, rectTransform.localPosition.z);
            }
        }
    }

    private IEnumerator MoveSymbols()
    {
        isMoving = true;

        List<LTDescr> reelTweens = new List<LTDescr>();

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

            float stopPositionY = GetCustomStopPosition(rectTransform);

            LTDescr tween = LeanTween.moveLocalY(rectTransform.gameObject, endPositionY, moveDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    LeanTween.moveLocalY(rectTransform.gameObject, stopPositionY, 0.2f)
                        .setEase(LeanTweenType.easeInOutQuad)
                        .setOnComplete(() => OnReelStop(rectTransform.gameObject));
                });

            reelTweens.Add(tween);
        }

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
        if (customStopPositions.Count > 0)
        {
            int index = Random.Range(0, customStopPositions.Count);
            return customStopPositions[index];
        }
        else
        {
            return rectTransform.localPosition.y;
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        LeanTween.cancelAll();
    }
}
