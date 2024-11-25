using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUp: MonoBehaviour
{
    public float SpeedUpDuration = 10f;
    public AudioClip SpeedUpSound;

    private AudioSource audioSource;
    private PlayerUI playerUI;
    

    void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
       

        if (playerUI == null)
        {
            Debug.LogError("PlayerUI not found in the scene.");
        }

       

        audioSource = playerUI.gameObject.GetComponent<AudioSource>();
    }

    public void ActivateSpeedUp()
    {
        if (playerUI != null )
        {
            audioSource.clip = SpeedUpSound;
            audioSource.Play();

            playerUI.StartSpeedUpTimer(SpeedUpDuration);

            playerUI.StartCoroutine(playerUI.UpdateTimer(SpeedUpDuration, "Speed Up"));
        }
    }

}
