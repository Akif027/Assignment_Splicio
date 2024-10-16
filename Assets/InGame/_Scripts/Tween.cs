
using UnityEngine;
using DG.Tweening;

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
}
