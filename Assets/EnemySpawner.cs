using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] public Vector3 spawnTriggerOffset;
    [Range(0, 100)] [SerializeField] float spawnRadius = 10;
    [Range(0, 100)] [SerializeField] float spawnTimeDrainRadius = 0;
    [Range(0, 100)] [SerializeField] float spawnChance = 100;
    [Range(1, 5)] [SerializeField] int maxSpawnAmount = 1;

    [Header("Waves / Timer")]
    [SerializeField] bool infiniteWaves;
    [SerializeField] float waveCount;
    [SerializeField] Vector2 spawnGapSeconds;

    float playerRange, spawnTimer, wave;
    bool active;

    internal bool init;
    internal EnemyManager enemyManager;
    internal Transform player;

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere indicating the spawn range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + spawnTriggerOffset, spawnRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + spawnTriggerOffset, spawnTimeDrainRadius);
    }

    void Update()
    {
        if (init)
        {
            playerRange = ((transform.position + spawnTriggerOffset) - player.transform.position).magnitude;

            if (playerRange < spawnRadius && !active)
                Spawn(); // Init spawn

            if (active)
            {
                if (playerRange < spawnRadius)
                {
                    spawnTimer -= Time.deltaTime;
                }
                else if (playerRange < spawnTimeDrainRadius)
                {
                    spawnTimer -= Time.deltaTime / 2;
                }

                if (spawnTimer < 0 && (wave < waveCount || infiniteWaves))
                {
                    Spawn(); // Wave spawn
                }
            }
        }
        else
        {
            /*
            Debug.LogWarning("Spawner not added to Enemy Manager list!");
            player = FindObjectOfType<Player>();
            enemyManager = FindObjectOfType<EnemyManager>();
            */
            init = true;
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < maxSpawnAmount; i++)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, transform.rotation);

                /*
                Enemy enemy = Instantiate(m_spawns[Random.Range(0, m_spawns.Length)], transform.position,
                    m_facePlayer ? Quaternion.Euler(transform.position - player.transform.position) : transform.rotation).GetComponent<Enemy>();

                enemyManager.InitEnemy(enemy);
                */
            }
        }

        active = true;
        wave++;
        spawnTimer = Random.Range(spawnGapSeconds.x, spawnGapSeconds.y);
    }
}