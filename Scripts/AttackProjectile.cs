using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public LayerMask targetMask;
    [HideInInspector] public CharacterStats myStats;
    public GameObject dieVFX;

    public float livingTime;

    void Start()
    {
        Die(livingTime);
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (targetMask == (targetMask | (1 << other.gameObject.layer)))
        {
            if (other.gameObject.GetComponent<CharacterStats>())
            {
                myStats.DealDamage(other.gameObject.GetComponent<CharacterStats>());
            }
        }
    }

    public virtual void Die(float timeDelay)
    {
        if (dieVFX != null)
        {
            Instantiate(dieVFX, transform.position, transform.rotation);
        }
        Destroy(gameObject, timeDelay);
    }
}
