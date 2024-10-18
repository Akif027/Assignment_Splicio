using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    [Header("Player Settings")]
    public float force = 20f;
    public float forceCooldown = 1f;

    [Header("Feedback")]
    public MMFeedbacks collisionFeedback; // Assign in the Inspector

    private Rigidbody rb;
    private Camera mainCamera;
    private float lastForceTime = 0f; // Time when the last force was applied
    private Vector3 tapWorldPosition; // Store the world position of the tap

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Cache the Rigidbody component
        mainCamera = Camera.main; // Cache the main camera
    }

    void Update()
    {
        HandleMovement();
        HandleKeyboardMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && CanApplyForce())
        {
            Vector3 tapPosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(tapPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                tapWorldPosition = hit.point; // Store the tap position in the world
                ApplyForceToDirection((tapWorldPosition - transform.position).normalized);
            }
        }
    }

    private void HandleKeyboardMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) movement += Vector3.back;
        if (Input.GetKey(KeyCode.A)) movement += Vector3.left;
        if (Input.GetKey(KeyCode.D)) movement += Vector3.right;

        if (movement != Vector3.zero && CanApplyForce())
        {
            ApplyForceToDirection(movement.normalized);
        }
    }

    private bool CanApplyForce()
    {
        return Time.time - lastForceTime >= forceCooldown;
    }

    private void ApplyForceToDirection(Vector3 direction)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
        lastForceTime = Time.time; // Update cooldown timer
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyCube"))
        {
            GameManager.Instance.IncreaseScore(10);
            GameManager.Instance.PlayHitParticle(collision.contacts[0].point);
            GameManager.Instance.FloatingHealth(transform);

            collisionFeedback.PlayFeedbacks();
        }
    }
}
