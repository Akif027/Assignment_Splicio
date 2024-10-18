
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
}
