using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Sprite icon;
    public GameObject interactionVFX;
    public bool countable = false;
    public int amount;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public abstract void Interact(GameObject obj);

    public void AplayVFX()
    {
        if(interactionVFX != null)
        {
            GameObject vfx = Instantiate(interactionVFX, transform.position, transform.rotation);
            Destroy(vfx, 10);
        }
    }
}
