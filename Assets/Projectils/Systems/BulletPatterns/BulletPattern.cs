using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BulletPattern : ScriptableObject
{
    [SerializeField]
    [TextArea(3, 5)]
    private string description;

    public abstract IEnumerator TriggerBulletPattern(GameObject source, Action whenDone = null);
}
