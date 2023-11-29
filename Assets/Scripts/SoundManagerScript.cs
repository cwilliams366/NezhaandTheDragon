using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    //Singleton
    public static SoundManagerScript S;
    //Audio Source
    private AudioSource audio;
    private AudioSource bgmAudio;
    //Booleans
    public bool atMainMenu = true;
    public bool isAtLevel = false;
    public bool isPaused = false;
    //AudioComponents
    [Header("SFX Clips")]
    public AudioClip hitconfirm;
    public AudioClip coinSound;
    public AudioClip potionSound;
    public AudioClip healthSound;
    public AudioClip talismanSound;
    public AudioClip spiritSound;

    [Header("Main Themes")]
    public AudioClip title;
    public AudioClip level1;
    public void Awake()
    {
        if (S)
        {
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        //Get the audio source
        audio = GetComponent<AudioSource>();
        bgmAudio = GetComponent<AudioSource>();
    }

    //Play the respective sound effact
    public void PlayRespectiveSound(string soundTag)
    {
        switch(soundTag)
        {
            case "Coin":
                audio.PlayOneShot(coinSound);
                break;
            case "Potion":
                audio.PlayOneShot(potionSound);
                break;
            case "Health":
                audio.PlayOneShot(healthSound);
                break;
            case "Talisman":
                audio.PlayOneShot(talismanSound);
                break;
            case "Spirit":
                audio.PlayOneShot(spiritSound);
                break;
            default:
                Debug.Log("No sound was played");
                break;
        }
    }
   
    public void PlayBackgroundMusic(GameState state)
    {
        //Get current state to play respective music
        if(state == GameState.MainMenu && atMainMenu)
        {
            bgmAudio.PlayOneShot(title);
        }
        else if(state == GameState.Playing)
        {
            if (bgmAudio.isPlaying && atMainMenu)
            {
                //Stop the title music and play the new one
                bgmAudio.Stop();
                bgmAudio.PlayOneShot(level1);
            }
            else if(isPaused)
            {
                //Pause the music 
                bgmAudio.UnPause();
                isPaused = false;
            }
           
        }
        else if(state == GameState.Paused)
        {
            //Stop the current music and play the current music
            isPaused = true;
            bgmAudio.Pause();
        }
        else if(state == GameState.GameOver)
        {
            //Stop the current music and play the current music
            bgmAudio.Stop();
        }
    }

}
