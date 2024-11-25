using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private TextMeshProUGUI ammoCountText;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform ammoPanel;
    [SerializeField] private Color bulletColorFull = Color.white;
    [SerializeField] private Color bulletColorEmpty = Color.gray;


    [SerializeField] private TextMeshProUGUI SpeedUpTimerText; 

    private List<Image> bulletImages = new List<Image>();

    private Coroutine speedUpCoroutine;
    float[] originalDamages;

    [SerializeField] private float speedUpTimeRemaining;
    private WeaponController weaponController;


    void Start()
    {

        promptText.text = "";
        ammoCountText.text = "";

 
        if (SpeedUpTimerText != null)
        {
            SpeedUpTimerText.gameObject.SetActive(false); 
        }


        weaponController = GetComponent<WeaponController>();
 
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

  

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        while (bulletImages.Count < currentAmmo)
        {
            GameObject newBullet = Instantiate(bulletPrefab, ammoPanel);
            bulletImages.Add(newBullet.GetComponent<Image>());
        }

        while (bulletImages.Count > currentAmmo)
        {
            Destroy(bulletImages[bulletImages.Count - 1].gameObject);
            bulletImages.RemoveAt(bulletImages.Count - 1);
        }

        for (int i = 0; i < bulletImages.Count; i++)
        {
            bulletImages[i].color = i < currentAmmo ? bulletColorFull : bulletColorEmpty;
        }

        ammoCountText.text = $"{currentAmmo} / {maxAmmo}";
    }

 


    public void StartSpeedUpTimer(float duration)
    {
        if (speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
        }

        speedUpTimeRemaining = duration;
        speedUpCoroutine = StartCoroutine(SpeedUpCountdown());
        StartTimer();
    }

    private IEnumerator SpeedUpCountdown()
    {
        GetComponent<PlayerMovement>().walkSpeed *= 2;
        while (speedUpTimeRemaining > 0)
        {
            speedUpTimeRemaining -= Time.deltaTime;
            yield return null;
        }
        GetComponent<PlayerMovement>().walkSpeed /= 2;
        /*//  UnPause the spawning in WaveSystem
        waveSystem.SetTimeStop(false);
        UnfreezeZombies();*/
    }


    public void StartTimer()
    {
     SpeedUpTimerText.gameObject.SetActive(true);
     
    }

    public IEnumerator UpdateTimer(float duration, string timer)
    {
        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            SpeedUpTimerText.text = timer + " : " + speedUpTimeRemaining.ToString("F1") + "s";
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
      
        SpeedUpTimerText.gameObject.SetActive(false);
    }


      

    

}
