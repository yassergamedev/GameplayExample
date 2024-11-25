using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest: RewardDispenser
{

    public GameObject pickUp;

    public bool isOpened = false; 
    private PlayerInteract playerInteract; 
    private Animator boxAnimator; 

    void Start()
    {
        playerInteract = FindObjectOfType<PlayerInteract>(); 
        boxAnimator = GetComponentInParent<Animator>(); 

    }

    protected override void Open()
    {
        if (isOpened)
        {
            return;
        }

        if (playerInteract != null )
        {
            
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger("Open");
            }
            pickUp.SetActive(true);
            isOpened = true;
        }
       
    }


}
