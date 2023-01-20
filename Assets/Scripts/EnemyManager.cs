using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    private bool spawnCooldown = false;
    public float spawnCooldownTime = 0.5f;
    public GameObject enemyBase;
    
    public int wave = 0;
    private bool isWaveActive;
    public int spawnEnemies;


    public TextMeshProUGUI waveNumbText;
    [SerializeField] GameObject playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(activeEnemies == 0)
        {
            wave++;
            spawnEnemies = 0;
            spawnCooldownTime = Mathf.Max(0.5f - 0.01f * wave, 0f);
            waveNumbText.text = wave.ToString();

            playerController.GetComponent<PlayerController>().UpdateResources(60);
        }
        PlayWave(wave);
    }

    void SpawnEnemy(int wave)
    {
        if (!spawnCooldown)
        {
            GameObject enemy = EnemyPool.instance.GetPooledObject();
            if (enemy != null)
            {
                enemy.transform.position = enemyBase.transform.position;
                enemy.GetComponent<Enemy>().level = wave;
                enemy.GetComponent<Enemy>().maxHP = 5 + wave;
                enemy.GetComponent<Enemy>().hp = 5 + wave;
                enemy.GetComponent<Enemy>().speed = 3f + wave/10f;
                enemy.GetComponent<Enemy>().expOnKill = 1 + (int)Mathf.Floor(wave / 10f);
                enemy.SetActive(true);
                StartCoroutine(SpawnCooldown());
            }

        }

    }

    IEnumerator SpawnCooldown()
    {
        spawnCooldown = true;
        spawnEnemies++;
        yield return new WaitForSeconds(spawnCooldownTime);
        spawnCooldown = false;
    }

    void PlayWave(int wave)
    {
        int waveEnemies = (int)Mathf.Floor(0.25f * wave * wave + 0.5f * wave + 0.5f);
        
        if(spawnEnemies < waveEnemies)
        {
            SpawnEnemy(wave);
            
        }
    }
}
