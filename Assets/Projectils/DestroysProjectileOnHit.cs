using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroysProjectileOnHit : MonoBehaviour
{
    public delegate void OnProjectileDestroyedDelegate();
    public event OnProjectileDestroyedDelegate OnProjectileDestroyed = delegate { };

    [Header("On Any Collision")]
    [SerializeField]
    private bool onAny = false;
    public bool OnAny { get => onAny; set => onAny = value; }

    [SerializeField]
    private string onAnyText;

    [SerializeField]
    private Color onAnyTextColor = new Color(0, 222, 255);

    [Header("On Damage Taken")]
    [SerializeField]
    private bool onDamageTaken = false;
    public bool OnDamageTaken { get => onDamageTaken; set => onDamageTaken = value; }

    void Start()
    {
        if (onDamageTaken)
        {
            TakesDamage takesDamage = GetComponent<TakesDamage>();
            if (takesDamage)
            {
                takesDamage.OnDamageTaken += doesDamage =>
                {
                    Destroy(doesDamage.gameObject);
                    OnProjectileDestroyed();
                };
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile && onAny)
        {
            UnityEngine.Debug.Log("Destroying because " + gameObject.name + " destroys any.");
            Destroy(projectile.gameObject);
            DisplayOnAnyTextIfSet();
            OnProjectileDestroyed();
        }
    }

    private void DisplayOnAnyTextIfSet()
    {
        if (!string.IsNullOrEmpty(onAnyText))
        {
            PopupManager.Instance.createPopupText(transform.position, onAnyText, onAnyTextColor);
        }
    }
}
