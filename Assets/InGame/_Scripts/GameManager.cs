using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private GameObject enemyCube;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject gameOverTextObj;
    [SerializeField] private Vector3 groundBounds;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore;

    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float gameTime = 30f;
    private bool isGameOver;

    [Header("Particles")]
    [SerializeField] private GameObject dustParticle;
    [SerializeField] private GameObject hitParticle;

    [Header("Health Settings")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform healthbarTarget;
    [SerializeField] private Transform parent;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ObjectPooler.Instance.CreatePool(enemyCube, 10);
        SpawnEnemyAtRandomDirection(5f);
        gameOverTextObj.SetActive(false);
        StartCoroutine(GameTimer());
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
        Tween.StretchAndSquish(scoreText.gameObject);  // Apply animation
    }

    private IEnumerator GameTimer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < gameTime && !isGameOver)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = gameTime - elapsedTime;
            timeText.text = "Time: " + Mathf.CeilToInt(remainingTime);
            yield return null;
        }

        GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverTextObj.SetActive(true);
        Tween.ScaleText(gameOverTextObj.transform.GetChild(0).gameObject);  // Apply animation
        Debug.Log($"Game Over! Final Score: {currentScore}");
        timeText.text = "Game Over!";
        Time.timeScale = 0;
    }

    public void SpawnEnemyAtRandomDirection(float distance)
    {
        Vector3 playerPos = player.position;
        Vector3[] directions =
        {
            new Vector3(0, 0, distance),   // Forward
            new Vector3(0, 0, -distance),  // Backward
            new Vector3(-distance, 0, 0),  // Left
            new Vector3(distance, 0, 0)    // Right
        };

        GameObject pooledObject = null;
        bool spawnSuccessful = false;

        for (int attempt = 0; attempt < directions.Length; attempt++)
        {
            Vector3 randomDirection = directions[Random.Range(0, directions.Length)];
            Vector3 spawnPos = playerPos + randomDirection;

            spawnPos.x = Mathf.Clamp(spawnPos.x, -groundBounds.x / 2, groundBounds.x / 2);
            spawnPos.z = Mathf.Clamp(spawnPos.z, -groundBounds.z / 2, groundBounds.z / 2);
            spawnPos.y = playerPos.y;

            if (Vector3.Distance(spawnPos, playerPos) >= distance * 0.9f)
            {
                pooledObject = ObjectPooler.Instance.GetPooledObject(enemyCube);
                if (pooledObject != null)
                {
                    pooledObject.transform.position = spawnPos;
                    pooledObject.SetActive(true);
                    pooledObject.transform.LookAt(new Vector3(playerPos.x, pooledObject.transform.position.y, playerPos.z));
                    spawnSuccessful = true;
                    break;
                }
            }
        }

        if (!spawnSuccessful && pooledObject != null)
        {
            ObjectPooler.Instance.ReturnToPool(pooledObject);
        }

        if (GameObject.FindGameObjectsWithTag("EnemyCube").Length == 0)
        {
            SpawnEnemyAtRandomDirection(5f);
        }
    }

    public void PlayHitParticle(Vector3 pos)
    {
        PlaySound(hitClip);
        GameObject particle = Instantiate(hitParticle, pos, Quaternion.identity);
        Destroy(particle, 1);
    }

    public void PlayDustParticle(Transform pos)
    {
        GameObject particle = Instantiate(dustParticle, pos.position, Quaternion.identity);
        Destroy(particle, 1);
    }

    public void FloatingHealth(Transform startPos)
    {
        Tween.CreateHealthFloatingText(startPos.position, 10, floatingTextPrefab, healthbarTarget, parent);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GamePlay");
        Time.timeScale = 1;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
