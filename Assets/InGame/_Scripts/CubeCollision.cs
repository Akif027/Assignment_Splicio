using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class CubeCollision : MonoBehaviour
{
    public MMFeedbacks collisionFeedback; // Assign in the Inspector
    public MMFeedbacks collisionFeedback2; // Assign in the Inspector
    public float Force = 20f;
    public GameObject ActionCamera2;



    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On screen tap
        {
            ApplyForce();
            GameManager.Instance.PlayDustParticle(transform);
        }


    }

    public void ApplyForce() // Detect tap
    {
        // Move cubes towards each other and apply physics for collision
        GetComponent<Rigidbody>().AddForce(-Vector3.forward * Force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the other object is the second cube
        if (collision.gameObject.CompareTag("Cube"))
        {
            GameManager.Instance.PlayHitParticle(transform);
            GameManager.Instance.FloatingHealth(transform);
            collisionFeedback2.PlayFeedbacks();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Activate secondary feedback on trigger enter
        if (collision.gameObject.CompareTag("Cube"))
        {

            ActionCamera2.SetActive(true);
            collisionFeedback.PlayFeedbacks();
        }
    }


}
