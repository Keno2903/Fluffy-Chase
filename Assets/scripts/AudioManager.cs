using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.Collections;
using System.Collections;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);



        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;


        }
    }
    private void Start()
    {
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        yield return new WaitForSecondsRealtime(1);
    }
    public void Play(string name)
    {
        if (!CurrentData.instance.sound)
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Stop(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
