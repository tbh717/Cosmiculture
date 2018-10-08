using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameStates;

public class AudioDirector : MonoBehaviour {

    public AudioClip mainTheme;
    public AudioClip bgNoise;
    public AudioClip tilePlaceEffect;

    private float tileWaitTime;

    private float bgVolume;
    private float themeVolume;
    private float tileVolume;

    private AudioSource themeSource;
    private AudioSource bgSource;
    private AudioSource effectSource;
    
    // Tile name to audio source
    private Dictionary<string, AudioSource> tileSources;

    private Coroutine tileFocusing;

    void Start() {
        tileSources = new Dictionary<string, AudioSource>();

        tileWaitTime = 2.0f;

        tileVolume = 1f;
        themeVolume = 0.8f;
        bgVolume = 0.8f;

        themeSource = AddSource(mainTheme, true, 1);
        bgSource = AddSource(bgNoise, true, bgVolume);
        effectSource = AddSource(null, false, 1);
    }

    public AudioSource AddSource(AudioClip clip, bool loop, float vol) {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.volume = vol;

        return newAudio;
    }

    public void ChangeState(GameState newState) {
        if(newState == GameState.StartGame) {
            PlayBG(true).volume = 0;
            PlayTheme(true);
            StartCoroutine(TileSounds());
        }
        if(newState == GameState.Gameplay) {
            FadeTheme(false, 1.0f);
            FadeGameplay(true, 1.0f);
        }
        if(newState == GameState.CardSelect) {
            FadeGameplay(false, 1.0f);
            FadeTheme(true, 1.0f);
        }
    }

    private void FadeTheme(bool b, float t) {
        float v;
        // If b, fade in, else fade out
        if(b) v = themeVolume; else v = 0.0f;
        StartCoroutine(FadeTo(themeSource, v, t));
    }

    private void FadeGameplay(bool b, float t) {
        // If b, fade in, else fade out
        float bgv = b ? bgVolume : 0.0f;
        float tv = b ? tileVolume : 0.0f;
        StartCoroutine(FadeTo(bgSource, bgv, t));
        foreach(AudioSource tileSource in tileSources.Values) StartCoroutine(FadeTo(tileSource, tv, t));
    }

    private IEnumerator TileSounds() {
        while(true) {
            float l = PlayRandomSound();
            yield return new WaitForSeconds(l/2f);
        }
    }

    private float PlayRandomSound() {
        Debug.Log(tileSources.Count);
        if(tileSources.Count > 0) {
            int i = Random.Range(0,tileSources.Count-1);
            List<AudioSource> l = new List<AudioSource>(tileSources.Values);
            AudioSource curSource = l[i];
            if(!curSource.isPlaying) {
                curSource.Play();
            }
            return curSource.clip.length;
        }
        else return 1f;
    }

    private IEnumerator FadeTo(AudioSource aus, float value, float atime) {
        float vol = aus.volume;
        // Fading from a low value into a high
        if(value > vol) {
            for(float t = vol; t < value; t += Time.deltaTime / atime) {
                aus.volume = Mathf.Lerp(vol,value,t);
                yield return null;
            }
        }
        // Fading from a high value into a low
        else {
            for(float t = vol; t > value; t -= Time.deltaTime / atime) {
                aus.volume = Mathf.Lerp(value,vol,t);
                yield return null;
            }
        }
        aus.volume = value;
        yield break;
    }

    public AudioSource PlayTheme(bool p) {
        if(p) themeSource.Play();
        else themeSource.Stop();
        return themeSource;
    }

    public AudioSource PlayBG(bool p) {
        if(p) bgSource.Play();
        else bgSource.Stop();
        return bgSource;
    }

    public void UnfocusTileAudio(Tile tile) {
        if(tileFocusing != null) {
            StopCoroutine(tileFocusing);
            tileFocusing = null;
            foreach(AudioSource aSource in tileSources.Values) StartCoroutine(FadeTo(aSource, tileVolume, 0.0f));
        }
    }

    public void FocusTileAudio(Tile tile) {
        string tileName = tile.name;
        foreach(KeyValuePair<string, AudioSource> kv in tileSources) if(kv.Key != tileName) StartCoroutine(FadeTo(kv.Value, tileVolume * 0.15f, 0.0f));
        tileFocusing = StartCoroutine(TileFocusing(tileName));
    }

    private IEnumerator TileFocusing(string tileName) {
        if(tileSources.ContainsKey(tileName)) {
            AudioSource focusAudio = tileSources[tileName];
            while(true) {
                focusAudio.Play();
                yield return new WaitForSeconds(focusAudio.clip.length + 0.5f);
            }
        } 
    }

    public float GetBGVolume() {
        int n = tileSources.Count;
        return Mathf.Exp(-n * 0.25f);
    }

    // Item added
    public void AddItemAudio(Item item, Tile tile) {
        AudioSource tileSource = AddSource(item.Audio, false, 1f);
        tileSources.Add(tile.name, tileSource);
        bgVolume = GetBGVolume();
        StartCoroutine(FadeTo(bgSource, bgVolume, 1.0f));
        effectSource.PlayOneShot(tilePlaceEffect);
    }

    public void RemoveItemAudio(Item item, Tile tile) {
        bool removed = tileSources.Remove(tile.name);
        if(!removed) Debug.Log("Failed to remove tile from AudioDirector: " + tile.name);
    }
}