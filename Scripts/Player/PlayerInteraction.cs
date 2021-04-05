using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange;

    public DitzeGames.MobileJoystick.Button interactBtn;
    public TextMeshProUGUI btnInteractableName;
    public TextMeshProUGUI btnInteractableCount;
    public Image btnInteractionIcon;

    void Start()
    {
        interactBtn.onButtonDown += Interact;
        interactBtn.gameObject.SetActive(false);
    }

    void Update()
    {
        Interactable closestInteractable = GetClosestInteractableInRange();
        if (closestInteractable != null)
        {
            interactBtn.gameObject.SetActive(true);
            btnInteractionIcon.sprite = closestInteractable.icon;
            if (closestInteractable.countable)
            {
                btnInteractableCount.text = closestInteractable.amount.ToString();
            }
            else
            {
                btnInteractableCount.text = "";
            }
            btnInteractableName.text = closestInteractable.name;

        }
        else
        {
            interactBtn.gameObject.SetActive(false);
        }
    }

    public Interactable GetClosestInteractableInRange()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (var col in cols)
        {
            if (col.GetComponent<Interactable>())
            {
                return col.GetComponent<Interactable>();
            }
        }
        Interactable _inter = null;
        float curDistance = float.MaxValue;
        for (int i = 0; i < cols.Length; i++)
        {
            if (Vector3.Distance(transform.position, cols[i].transform.position) < curDistance)
            {
                _inter = cols[i].GetComponent<Interactable>();
                curDistance = Vector3.Distance(transform.position, cols[i].transform.position);
            }
        }

        return _inter;
    }

    public void Interact()
    {
        Interactable interactable = GetClosestInteractableInRange();
        if (interactable != null)
        {
            interactable.Interact(gameObject);
        }
    }
}
