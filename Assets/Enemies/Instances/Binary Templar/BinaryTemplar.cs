using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTemplar : MonoBehaviour
{

    [SerializeField]
    private GameObject target;

    // Update is called once per frame
    void Update()
    {
        // 1) Enemy should circle the player for N seconds
        // 2) After N seconds, stop, turn and face player.
        // 3) Shoot bullets towards player and wait for X amount of seconds (configurable)
        // 4) Continue circling the player, randomly choose clockwise or counter clockwise.
    }

}
