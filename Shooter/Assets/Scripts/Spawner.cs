using UnityEngine;

public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    float nextSpawnTime;
    int enemiesRemainingToDie;

    private void Start()
    {
        nextWave();
    }

    private void Update()
    {
        if(Time.time > nextSpawnTime && enemiesRemainingToSpawn > 0)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += onEnemyDeath;
        }
    }

    void onEnemyDeath()
    {
        print("an enemy died");
        enemiesRemainingToDie--;

        if(enemiesRemainingToDie == 0)
        {
            nextWave();
        }
    }

    void nextWave()
    {
        currentWaveNumber++;
        print("current wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingToDie = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}
