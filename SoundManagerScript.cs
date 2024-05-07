using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerSlash1, playerSlash2, playerSlash3;
    static AudioSource audioSrc;

    void Start()
    {
        playerSlash1 = Resources.Load<AudioClip>("playerSlash1");
        playerSlash2 = Resources.Load<AudioClip>("playerSlash2");
        playerSlash3 = Resources.Load<AudioClip>("playerSlash3");

        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "playerSlash1":
                audioSrc.PlayOneShot(playerSlash1);
                break;
            case "playerSlash2":
                audioSrc.PlayOneShot(playerSlash2);
                break;
            case "playerSlash3":
                audioSrc.PlayOneShot(playerSlash3);
                break;


        }
    }
}
