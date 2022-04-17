using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPObject : MonoBehaviour
{
    [SerializeField]
    float maxHP;
    float currentHP;
    bool isDead;
    HPBar hpBar;
    public void getDamage(float d)
    {
        if (isDead)
        {
            return;
        }
        currentHP -= d;
        Mathf.Clamp(currentHP, 0, maxHP);
        if (currentHP <= 0)
        {
            Die();
        }
        //update HPBar
        hpBar.update(currentHP);
    }
    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        //explosion

        Destroy(gameObject.transform.parent.gameObject, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (maxHP <= 0)
        {
            Debug.LogError("max hp should not be 0");
        }
        currentHP = maxHP;
        hpBar = GetComponentInChildren<HPBar>();
        if (!hpBar)
        {
            Debug.LogError("hp bar component does not existed as a child");
        }
        else
        {
            hpBar.init(maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for test only
        if (Input.GetKeyDown(KeyCode.P))
        {
            //getDamage(50);
            getDamage(Random.Range(1, 100));
        }
    }
}
