using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class ChangeShaderValueDuringPlayer : MonoBehaviour
{
    [SerializeField]
    private Material material;

    [SerializeField]
    private List<BooleanValue> booleanValues;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BooleanValue val in booleanValues)
        {
            if (Application.isPlaying)
            {
                material.SetInt(val.valueName, val.valueInPlay ? 1 : 0);
            }
            else
            {
                material.SetInt(val.valueName, val.valueInEditor ? 1 : 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Serializable]
    public class BooleanValue
    {
        [SerializeField]
        public string valueName;

        [SerializeField]
        public bool valueInPlay;

        [SerializeField]
        public bool valueInEditor;
    }
}
