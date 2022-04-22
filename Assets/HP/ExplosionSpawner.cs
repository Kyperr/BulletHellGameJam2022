using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HPObject))]
public class ExplosionSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> explosionList;
    public void spawn()
    {
        var go = Instantiate(explosionList[Random.Range(0,  explosionList.Count - 1)],transform.position,Quaternion.identity);
        Destroy(go, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<HPObject>().OnDeath += () => {
            spawn();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
