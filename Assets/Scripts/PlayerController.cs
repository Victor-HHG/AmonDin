using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool placingTower = false;
    public bool placingWall = false;
    private bool delayClick = false;

    public int hp = 100;
    public int resources = 40;
    public int towerCost = 50;
    public int wallCost = 20;

    [SerializeField] GameObject towerShadowPrefab;
    GameObject towerShadow;
    [SerializeField] GameObject wallShadowPrefab;
    GameObject wallShadow;
    [SerializeField] GameObject towerPrefab;
    GameObject tower;
    [SerializeField] GameObject wallPrefab;
    GameObject wall;
    [SerializeField] GameObject mainBase;
    [SerializeField] GameObject enemyBase;

    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI resourcesText;
    [SerializeField] TextMeshProUGUI towerCostText;
    [SerializeField] TextMeshProUGUI wallCostText;


    // Start is called before the first frame update
    void Start()
    {

        towerCostText.text = towerCost.ToString();
        wallCostText.text = wallCost.ToString();
        resourcesText.text = resources.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        //Metodo para mostrar donde se va a poner una torre
        if (placingTower)
        {
            PickTowerLocation();
        }

        //Método para mostrar donde se va a poner una pared
        if (placingWall)
        {
            PickWallLocation();
        }

        if (Input.GetMouseButtonDown(0))
        {
            CheckClickOnDrop();
        }
            
    }

    public void BuyTower()
    {
        if (!placingTower && !placingWall)
        {
            if (resources >= towerCost)
            {
                placingTower = true;
                towerShadow = Instantiate(towerShadowPrefab, MouseToGrid(), Quaternion.identity);
                StartCoroutine(DelayClick());
            }
        }

    }

    public void BuyWall()
    {
        if (!placingTower && !placingWall)
        {
            if (resources >= wallCost)
            {
                placingWall = true;
                wallShadow = Instantiate(wallShadowPrefab, MouseToGrid(), Quaternion.identity);
                StartCoroutine(DelayClick());
            }
        }
    }

    void PickTowerLocation()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            placingTower = false;
            GameObject.Destroy(towerShadow);
            
        }

        towerShadow.transform.position = MouseToGrid();
        float yPos = towerShadow.transform.position.y;

        if (Input.GetMouseButtonUp(0) && !delayClick && yPos >= -13.5)
        {
            tower = Instantiate(towerPrefab, MouseToGrid(), Quaternion.identity);
            VerifyPathTower(tower);
        }

    }

    void PickWallLocation()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            placingWall = false;
            GameObject.Destroy(wallShadow);
            
        }

        wallShadow.transform.position = MouseToGrid();

        if (Input.GetMouseButtonUp(0) && !delayClick)
        {
            wall = Instantiate(wallPrefab, MouseToGrid(), Quaternion.identity);
            VerifyPathWall(wall);
        }

    }

    void VerifyPathTower(GameObject tower)
    {
        var guo = new GraphUpdateObject(tower.GetComponentsInChildren<BoxCollider2D>()[0].bounds);
        var spawnPointNode = AstarPath.active.GetNearest(enemyBase.transform.position).node;
        var goalNode = AstarPath.active.GetNearest(mainBase.transform.position).node;
        if (GraphUpdateUtilities.UpdateGraphsNoBlock(guo, spawnPointNode, goalNode, false))
        {
            placingTower = false;
            Destroy(towerShadow);
            UpdateResources(-towerCost);
            IncreaseTowerCost();
        }
        else
        {
            
            Destroy(tower);
        }

    }

    void VerifyPathWall(GameObject wall)
    {
        var guo = new GraphUpdateObject(wall.GetComponent<BoxCollider2D>().bounds);
        var spawnPointNode = AstarPath.active.GetNearest(enemyBase.transform.position).node;
        var goalNode = AstarPath.active.GetNearest(mainBase.transform.position).node;
        if (GraphUpdateUtilities.UpdateGraphsNoBlock(guo, spawnPointNode, goalNode, false))
        {
            placingWall = false;
            Destroy(wallShadow);
            UpdateResources(-wallCost);
            IncreaseWallCost();
        }
        else
        {

            Destroy(wall);
        }

    }

    Vector3 MouseToGrid()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Vector3 gridPos = new Vector3(Mathf.Floor(worldPos.x), Mathf.Floor(worldPos.y), 0);
        gridPos += new Vector3(0.5f, 0.5f, 0);
        return (gridPos);
    }

    IEnumerator DelayClick()
    {
        delayClick = true;
        yield return new WaitForSeconds(0.5f);
        delayClick = false;
    }

    void IncreaseTowerCost()
    {
        towerCost = (int)Mathf.Floor(towerCost * 1.1f);
        towerCostText.text = towerCost.ToString();
    }

    void IncreaseWallCost()
    {
        wallCost = (int)Mathf.Floor(wallCost * 1.2f);
        wallCostText.text = wallCost.ToString();
    }

    public void UpdateResources(int flow)
    {
        resources += flow;
        resourcesText.text = resources.ToString();
    }

    void CheckClickOnDrop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, Vector2.zero, Mathf.Infinity);

        /*
        if (hit.collider != null)
        {
            
            if (hit.collider.tag == "Drop")
            {
                hit.transform.gameObject.GetComponent<DropedResources>().ClickedOn();
            }
            
        }
        */
        foreach(RaycastHit2D hit in hits)
        {
            
            if (hit.collider.tag == "Drop")
            {
                hit.transform.gameObject.GetComponent<DropedResources>().ClickedOn();
            }

        }

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpText.text = hp.ToString();
        if(hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

}
