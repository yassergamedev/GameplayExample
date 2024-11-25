using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{

    public AudioClip audioClip;
    private WeaponController weaponController;
    private GameObject player;
    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        weaponController = player.GetComponent<WeaponController>();
    }

    public void PickWeapon()
    {

        weaponController.SwitchWeapon(1,true);
        weaponController.GetComponent<AudioSource>().PlayOneShot(audioClip);


    }
}
