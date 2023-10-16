using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioClip[] BGMs;
    [SerializeField] [Range(0, 1)] float volume;
    [SerializeField] [Range(0, 1)] float volume_ui;
    AudioSource audioSource;
    public AudioSource AudioSource { get { return audioSource; } set { audioSource = value; } }
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMs[1];
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void ChangeBgm(string stage)
    {
        switch (stage)
        {
            case "EnterStage1":
                PlayBGM(1);
                break;

            case "EnterStage2":
                PlayBGM(2);
                break;

            case "EnterStage3":
                PlayBGM(3);
                break;
        }
        
    }

    void PlayBGM(int idx)
    {
        //StartCoroutine(MusicFadeout());
        audioSource.clip = BGMs[idx];
        audioSource.Play();
        //StartCoroutine(MusicFadein());
    }

    public void TurnOnUI(bool turn)
    {
        if (turn)
        {
            audioSource.volume = volume_ui;
        }
        else
        {
            audioSource.volume = volume;
        }
    }

    IEnumerator MusicFadein()
    {
        while (audioSource.volume < volume)
        {
            Debug.Log("in");
            audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.2f);
        }
        StopCoroutine(MusicFadein());
    }

    IEnumerator MusicFadeout()
    {
        while (audioSource.volume > 0)
        {
            Debug.Log("out");
            audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.2f);
        }
        StopCoroutine(MusicFadeout());
    }
}
