using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncesDestroy : MonoBehaviour
{
    public delegate void OnDestroyDelegate();

    public event OnDestroyDelegate OnRegisterableDestroy = delegate { };

    void OnDestroy()
    {
        OnRegisterableDestroy();
    }

}
