using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBulletPattern : ScriptableObject
{
    [SerializeField]
    [TextArea(3, 5)]
    private string description;

    public abstract IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null);

    protected Vector3 GetBaseDirection(GameObject source)
    {
        Vector3 baseDirection = new Vector3();

        Target target = source.GetComponent<Target>();
        TargetDirection targetDirection = source.GetComponent<TargetDirection>();

        if (target)
        {
            baseDirection = target.GetTarget().transform.position - source.transform.position;
        }
        else if (targetDirection)
        {
            baseDirection = targetDirection.Direction;
        }
        else
        {
            baseDirection = source.transform.rotation.eulerAngles - source.transform.position;
        }

        return baseDirection;
    }
}
