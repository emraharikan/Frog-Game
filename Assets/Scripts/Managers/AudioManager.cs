using System.Collections;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource rightSound, wrongSound, collectedSound;


    private void Awake()
    {
        MakeSingleton();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        rightSound = audioSources[0];
        wrongSound = audioSources[1];
        collectedSound = audioSources[2];
    }

    private void MakeSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void RightSoundPlay()
    {
        if (GameManager.instance.GameModel.IsSFXOn)
        {
            if (!rightSound.isPlaying)
            {
                StartCoroutine(RightSoundCourutine());
            }
        }
    }

    private IEnumerator RightSoundCourutine()
    {
        rightSound.Play();
        yield return new WaitForSeconds(1);
        rightSound.Stop();
    }

    public void CollectedSoundPlay()
    {
        if (GameManager.instance.GameModel.IsSFXOn)
        {
            if (!collectedSound.isPlaying)
            {
                StartCoroutine(CollectedSoundCourutine());
            }
        }
    }

    private IEnumerator CollectedSoundCourutine()
    {
        collectedSound.Play();
        yield return new WaitForSeconds(1);
        collectedSound.Stop();
    }

    public void WrongSoundPlay()
    {
        if (GameManager.instance.GameModel.IsSFXOn)
        {
            if (!wrongSound.isPlaying)
            {
                StartCoroutine(WrongSoundCourutine());
            }
        }
    }

    private IEnumerator WrongSoundCourutine()
    {
        wrongSound.Play();
        yield return new WaitForSeconds(1f);
        wrongSound.Stop();
    }
}