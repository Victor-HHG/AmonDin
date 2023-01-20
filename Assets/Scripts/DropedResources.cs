using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedResources : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        StartCoroutine(DespawnTime());
    }

    public void ClickedOn()
    {
        playerController.GetComponent<PlayerController>().UpdateResources(20);
        Destroy(gameObject);
    }

    IEnumerator DespawnTime()
    {
        
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
