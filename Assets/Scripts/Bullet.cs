using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    public GameObject parentTower;

    // Update is called once per frame
    void Update()
    {
        OutOfBounds();
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
        
        
    }

    void OutOfBounds()
    {
        float distToTower = Vector2.Distance(parentTower.transform.position, transform.position);
        if(distToTower > parentTower.GetComponent<Tower>().shootRange)
        {
            gameObject.SetActive(false);
        }
    }
}
