using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TerrainManager : MonoBehaviour
{
    private float xIni;
    private float yIni;
    private float scale = 0.2f;
    private Transform mainBase;
    private Transform enemyBase;
    private float clearDist = 3f;

    public bool pathExist = false;

    [SerializeField]
    GameObject rockPrefab;

    private void Awake()
    {
        mainBase = GameObject.Find("MainBase").transform;
        enemyBase = GameObject.Find("EnemyBase").transform;

        
        while (!pathExist)
        {
            GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
            foreach(GameObject rock in rocks)
            {
                Destroy(rock);
            }

            xIni = Random.Range(-10000f, 10000f);
            yIni = Random.Range(-10000f, 10000f);

            CreateTerrain();

            AstarPath.active.Scan();

            GraphNode node1 = AstarPath.active.GetNearest(enemyBase.position, NNConstraint.Default).node;
            GraphNode node2 = AstarPath.active.GetNearest(mainBase.position, NNConstraint.Default).node;

            pathExist = PathUtilities.IsPathPossible(node1, node2);
        }

        
        

    }

    void CreateTerrain()
    {
        for(float x = -31.5f; x < 32f; x++)
        {
            for(float y = -13.5f; y < 14f; y++)
            {
                float distMainBase = Vector3.Distance(new Vector3(x, y, 0), mainBase.position);
                float distEnemyBase = Vector3.Distance(new Vector3(x, y, 0), enemyBase.position);
                if(distEnemyBase > clearDist && distMainBase > clearDist)
                {
                    float xCoord = xIni + x * scale;
                    float yCoord = yIni + y * scale;
                    float v = Mathf.PerlinNoise(xCoord, yCoord);

                    if (v > 0.55f)
                    {
                        Instantiate(rockPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    
                }

            }
        }
    }


}
