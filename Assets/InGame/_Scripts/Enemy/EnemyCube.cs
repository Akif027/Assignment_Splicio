using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyCube : MonoBehaviour
{
    [SerializeField] private MMFeedbacks collisionFeedback;  // Serialized for Inspector assignment
    [SerializeField] private float returnDelay = 0.4f;       // Adjustable delay before returning to pool

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _boxCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            collisionFeedback?.PlayFeedbacks();  // Play feedbacks safely if assigned
            _boxCollider.enabled = false;
            StartCoroutine(DelayedReturnToPool());
        }
    }

    private IEnumerator DelayedReturnToPool()
    {
        yield return new WaitForSeconds(returnDelay);

        // Return this enemy cube to the object pool
        ObjectPooler.Instance.ReturnToPool(gameObject);

        // Spawn a new enemy at a random direction
        GameManager.Instance.SpawnEnemyAtRandomDirection(5f);
    }
}
