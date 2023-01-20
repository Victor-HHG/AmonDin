using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public int hp = 5;
    public int maxHP = 5;
    public int level = 1;

    public Transform target;
    public float nextWaypointDistance = 0.1f;
    private float turnSpeed = 600f;
    public int expOnKill = 1;

    private Path path;
    public int currentWaypoint = 0;
    bool reachEndOfPath = false;

    private Seeker seeker;
    
    [SerializeField] GameObject dropResPrefab;
    PlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        target = GameObject.Find("MainBase").transform;

        playerController = FindObjectOfType<PlayerController>();

        InvokeRepeating("UpdatePath", 0f, 1f);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
        {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            return;
        }
        else
        {
            reachEndOfPath = false;
        }
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Move(direction);
        //ResetPosition();
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // Mover al enemigo
    void Move(Vector2 direction)
    {

        Quaternion moveRot = Quaternion.LookRotation(Vector3.forward, direction);
        float step = turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRot, step);
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

    void ResetPosition()
    {
        if(transform.position.y < -7f)
        {
            transform.position = new Vector2(Random.Range(-8.5f, 8.5f), 7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            hp -= 1;
            if (hp < 1)
            {
                GameObject tower = collision.gameObject.GetComponent<Bullet>().parentTower;
                tower.GetComponent<Tower>().AddExperience(1);
                DropResources();

                gameObject.SetActive(false);
                hp = maxHP;

            }
        }

        if(collision.name == "MainBase")
        {
            playerController.TakeDamage(10);
            gameObject.SetActive(false);
            hp = maxHP;
        }
        
    }

    void DropResources()
    {
        int amount = 1+ (int)Mathf.Floor(level / 5f);
        playerController.UpdateResources(amount);
        if (Random.Range(0f,100f) < 20f)
        {
            Instantiate(dropResPrefab, transform.position, Quaternion.identity);
        }
    }
}
