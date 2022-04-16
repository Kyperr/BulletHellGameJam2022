using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterArenaShaderUpdater : MonoBehaviour
{
    [SerializeField]
    private Material material;

    void Update()
    {
        material.SetVector("_PlayerPosition", this.transform.position);
    }
}
