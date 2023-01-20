using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTowerButton : MonoBehaviour
{
    public GameObject playerControllerGO;
    PlayerController playerController;

    private void Start()
    {
        playerController = playerControllerGO.GetComponent<PlayerController>();
    }

    public void BuyTower()
    {
        if (!playerController.placingTower)
        {
            if (playerController.resources >= playerController.towerCost)
            {
                playerController.placingTower = true;

            }
        }

    }
}
