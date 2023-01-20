using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    public float turnSpeed = 10f;
    private bool cooldown = false;
    private bool targetOn = false;
    public float cooldownTime = 0.5f;
    public float shootRange = 10f;
    public int exp = 0;
    public int level = 1;
    public int nextLvlExp = 5;


    public TextMeshPro levelText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LockTarget();
        
        Fire();
    }

    void Fire()
    {
        if (!targetOn)
        {
            return;
        }
        if (!cooldown)
        {
            GameObject bullet = BulletPool.instance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation * Quaternion.Euler(0,0,90f);
                bullet.GetComponent<Bullet>().parentTower = gameObject;
                
                bullet.SetActive(true);
                StartCoroutine(StartCooldown());
            }

        }
        
    }

    IEnumerator StartCooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }


    void LockTarget()
    {
        Vector3 target = FindTarget();
        Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, (target-transform.position).normalized);
        float step = turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, step);
    }


    Vector3 FindTarget()
    {
        Vector3 defaultTarget = gameObject.transform.position + new Vector3(1, 0, 0);
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length == 0)
        {
            targetOn = false;
            return defaultTarget;
        }
        GameObject target = enemies[0];
        float minDist = Vector2.Distance(transform.position, target.transform.position);

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if(dist < minDist)
            {
                target = enemy;
                minDist = dist;
            }
        }

        if(minDist > shootRange)
        {
            targetOn = false;
            return defaultTarget;
        }

        targetOn = true;
        return target.transform.position;

    }

    public void AddExperience(int gain)
    {
        exp += gain;
        if(exp >= nextLvlExp && level < 25)
        {
            LevelUp();
            nextLvlExp += 5 + level * 1;
        }
    }

    void LevelUp()
    {
        level += 1;

        turnSpeed = 40 + 10 * (level - 1);
        cooldownTime = Mathf.Max( 0.35f - 0.01f * (level - 1) , 0.1f);
        
        shootRange =10f + 0.4f* (level - 1);
        levelText.text = level.ToString();
    }
}
