using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100f;
    public float damage = 10f;
    public float fireRate = 0.1f;
    public int maxAmmo = 30;
    public int ammoReserve = 90;
    public int normalAmmoReserve = 90;
    public float reloadTime = 2f;
    public int pointsOnHit = 15;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip headshotSFX;
    public GameObject bulletHolePrefab;
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePoint;
    public Light muzzleFlashLight;
    public GameObject bloodEffectPrefab;
    public Animator weaponAnimator;
    public AudioSource audioSource;
    public int currentAmmo;


    public PlayerUI playerUI;

    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public MouseLook mouseLook;
    public Vector3 RecoilPush ;
    public Vector3 RecoilRotate;

    [Header ("Aim")]
    public Vector3 rotationOffset;
    public Vector3 positionOffset;

    [Header("Sprint")]
    public Vector3 sprintRotationOffset;
    public Vector3 sprintPositionOffset;
    public bool isSprintable;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        muzzleFlashLight.enabled = false;

        currentAmmo = maxAmmo;
        playerUI = FindObjectOfType<PlayerUI>();

     

        playerUI.UpdateAmmoText(currentAmmo, ammoReserve);

        mouseLook = FindObjectOfType<MouseLook>();
    }

    public virtual void Shoot(Camera playerCamera, Vector3 rotationOffset, float shakeAmount, WeaponController controller)
    {
        if (currentAmmo > 0)
        {
            audioSource.PlayOneShot(shootSound);

            if (weaponAnimator != null)
            {
                weaponAnimator.Play("Shoot");
            }

            mouseLook.AddRecoil(recoilX, recoilY);

            controller.StartCoroutine(controller.MuzzleFlash(muzzleFlashPrefab, muzzlePoint, muzzleFlashLight));

            StartCoroutine(ApplyWeaponRecoil());

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Transform hitTransform = hit.transform;
                while (hitTransform.parent != null)
                {
                    hitTransform = hitTransform.parent;
                }

                if (hitTransform.CompareTag("Zombie"))
                {
                    Zombie zombie = hitTransform.GetComponent<Zombie>();
                    if (zombie != null)
                    {
                        PlayerInteract playerInteract = playerCamera.GetComponentInParent<PlayerInteract>();
                        float finalDamage = damage;

                        if (bloodEffectPrefab != null)
                        {
                            Transform parentTransform = hit.collider.transform.parent;
                            GameObject bloodEffect = Instantiate(bloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal), parentTransform);
                            Destroy(bloodEffect, 0.5f);
                        }

                        zombie.TakeDamage(finalDamage);

                    }
                }
                else if (hit.collider != null && !hit.collider.isTrigger ) 
                {
                    Quaternion rotation = Quaternion.LookRotation(hit.normal);
                    rotation *= Quaternion.Euler(rotationOffset);
                    GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, rotation);

                    Destroy(bulletHole, 5f);
                }
            }

            currentAmmo--;
            playerUI.UpdateAmmoText(currentAmmo, ammoReserve);
        }
    }

    IEnumerator ApplyWeaponRecoil()
    {
        Vector3 originalPosition = transform.localPosition;
        Quaternion originalRotation = transform.localRotation;

        Vector3 recoilPosition = originalPosition + RecoilPush; 
        Quaternion recoilRotation = originalRotation * Quaternion.Euler(RecoilRotate); 

        float elapsedTime = 0f;
        float recoilDuration = 0.1f;
        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / recoilDuration);
            transform.localRotation = Quaternion.Lerp(originalRotation, recoilRotation, elapsedTime / recoilDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(recoilPosition, originalPosition, elapsedTime / recoilDuration);
            transform.localRotation = Quaternion.Lerp(recoilRotation, originalRotation, elapsedTime / recoilDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }


    public void Reload(WeaponController controller)
    {
        int ammoToReload = maxAmmo - currentAmmo;

        if (ammoReserve > 0 && ammoToReload > 0)
        {
            ammoToReload = Mathf.Min(ammoToReload, ammoReserve);

            currentAmmo += ammoToReload;
            ammoReserve -= ammoToReload;

            audioSource.PlayOneShot(reloadSound);
            if (weaponAnimator != null)
            {
                weaponAnimator.Play("Reload");
            }

            playerUI.UpdateAmmoText(currentAmmo, ammoReserve);
        }
       
    }
}
