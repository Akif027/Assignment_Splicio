using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("particle")]
    public GameObject Dustparticle;
    public GameObject Hitparticle;

    [Header("HealthSetting")]
    public GameObject floatingTextPrefab;
    public Transform HealthbarTarget;
    public Transform Parent;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayHitParticle(Transform pos)
    {
        GameObject p = Instantiate(Hitparticle);
        p.transform.position = pos.position;
        Destroy(p, 1);
    }

    public void PlayDustParticle(Transform pos)
    {
        GameObject p = Instantiate(Dustparticle);
        p.transform.position = pos.position;
        Destroy(p, 1);
    }
    public void FloatingHealth(Transform StartPos)
    {
        // Call the Tween method to instantiate and move the text
        Tween.CreateHealthFloatingText(StartPos.position, 10, floatingTextPrefab, HealthbarTarget, Parent);
    }
}
