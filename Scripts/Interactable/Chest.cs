using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropLoot))]
public class Chest : Interactable
{
    private DropLoot loot;
    private Animator anim;

    protected override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        loot = GetComponent<DropLoot>();
    }

    public override void Interact(GameObject obj)
    {
        anim.SetTrigger("Open");
        Destroy(this);
    }
}
