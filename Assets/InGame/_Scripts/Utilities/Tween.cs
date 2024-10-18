
using UnityEngine;
using DG.Tweening;
using TMPro;

public static class Tween
{
    public static void MoveAndScale(GameObject item, float duration = 1f, float moveDistance = 500f, float scaleFactor = 0.5f)
    {
        // Ensure the GameObject starts with its original scale
        Vector3 originalScale = item.transform.localScale;

        // Move the GameObject from right to left (negative X-axis) and scale it down during the movement
        item.transform.DOMoveX(item.transform.position.x - moveDistance, duration).SetEase(Ease.InBack);

        // Scale the object down during the movement
        item.transform.DOScale(originalScale * scaleFactor, duration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                // Reset the GameObject's scale to the original after the animation completes
                item.transform.DOScale(originalScale, 0.2f);
                item.SetActive(false);  // Optionally deactivate the item after animation
            });
    }
    public static void CreateHealthFloatingText(Vector3 startPosition, int Amount, GameObject floatingTextPrefab, Transform healthBarTransform, Transform parentTransform)
    {
        // Instantiate the floating text above the cube
        GameObject floatingText = Object.Instantiate(floatingTextPrefab, startPosition, Quaternion.identity, parentTransform);

        // Set the position to the screen point if necessary, depending on your setup
        floatingText.transform.position = Camera.main.WorldToScreenPoint(startPosition);

        // Animate the floating text to the health bar using DOTween
        floatingText.transform.DOMove(healthBarTransform.position, 1.5f).OnComplete(() =>
        {

            // Destroy the text once it reaches the health bar
            Object.Destroy(floatingText);
            Health.Instance.IncreaseHealth(Amount);

        });
    }


    public static void StretchAndSquish(GameObject textObject, float stretchAmount = 1.2f, float duration = 0.3f)
    {
        // Ensure the text object has a RectTransform component (which is required for UI elements)
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // Sequence to apply the stretch and squish effect
            Sequence stretchSquishSequence = DOTween.Sequence();

            // Stretch the text (scaling up)
            stretchSquishSequence.Append(rectTransform.DOScaleX(stretchAmount, duration / 2).SetEase(Ease.OutQuad));
            stretchSquishSequence.Join(rectTransform.DOScaleY(1 / stretchAmount, duration / 2).SetEase(Ease.OutQuad));

            // Squish the text back to its original size (scaling down)
            stretchSquishSequence.Append(rectTransform.DOScaleX(1f, duration / 2).SetEase(Ease.InQuad));
            stretchSquishSequence.Join(rectTransform.DOScaleY(1f, duration / 2).SetEase(Ease.InQuad));

            // Start the sequence
            stretchSquishSequence.Play();
        }
        else
        {
            Debug.LogError("StretchAndSquish: The GameObject does not have a RectTransform component.");
        }
    }


    public static void ScaleText(GameObject textObject, float duration = 1f)
    {
        // Ensure the text object has a RectTransform component (which is required for UI elements)
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // Scale the text object from 1 to 5 over the specified duration
            rectTransform.localScale = Vector3.one; // Reset scale to 1
            rectTransform.DOScale(5f, duration).SetEase(Ease.OutBack).SetUpdate(true); // Scale to 5
        }
        else
        {
            Debug.LogError("ScaleText: The GameObject does not have a RectTransform component.");
        }
    }
}
