using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefab : MonoBehaviour
{
    public GameObject AttackVFX;
    public Transform WeapongEdge;

    void Start()
    {
        if (AttackVFX != null)
            AttackVFX.SetActive(false);
    }

    void Update()
    {

    }
}
