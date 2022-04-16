using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackGroundWar : MonoBehaviour
{

    [SerializeField]
    private Vector2 size = new Vector2(100, 100);

    [SerializeField]
    private int numberOfShips = 10;

    [SerializeField]
    private List<GameObject> objectsToRender;

    void Start()
    {
        for (int i = 0; i < numberOfShips; i++)
        {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(0, size.x) - (size.x / 2), 0, UnityEngine.Random.Range(0, size.y) - (size.y / 2));
            Vector3 randomRotation = new Vector3(0, 0, 0);
            int randomShip = UnityEngine.Random.Range(0, objectsToRender.Count);

            GameObject go = Instantiate(objectsToRender[randomShip]);
            go.transform.parent = this.transform;
            go.transform.position = this.transform.position + randomPosition;
            go.transform.rotation = Quaternion.Euler(randomRotation);

        }
    }

    void Update()
    {
    }

    private void RenderBatches()
    {
    }

}
