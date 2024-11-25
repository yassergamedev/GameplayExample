using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int ammoAmount = 30; 
     

    public AudioClip audioClip;
    private WeaponController weaponController;
    private GameObject player;
    private void Start()
    {
       
        player = GameObject.FindGameObjectWithTag("Player");
        weaponController = player.GetComponent<WeaponController>();
    }

    public void PickAmmo()
    {

       
        weaponController.currentWeapon.ammoReserve += ammoAmount;
        player.GetComponent<PlayerUI>().UpdateAmmoText(
                weaponController.currentWeapon.currentAmmo,
                weaponController.currentWeapon.ammoReserve);

        weaponController.GetComponent<AudioSource>().PlayOneShot(audioClip);


    }
}
