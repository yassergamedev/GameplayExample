using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3.0f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private PlayerMovement playerMovement;
    public int points;

    void Start()
    {
        cam = Camera.main;
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);

    
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; 
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<RewardDispenser>() != null)
            {
                RewardDispenser interactable = hitInfo.collider.GetComponent<RewardDispenser>();
                playerUI.UpdateText(interactable.promptMessage);

                if (Input.GetButtonDown("Interact") && playerMovement.isGrounded)
                {
                    interactable.BaseOpen();
                }
            }
        }
    }

   
}
