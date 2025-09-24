using UnityEngine;
using System.Collections;

public class AudioFader : MonoBehaviour
{
    private static AudioFader _instance;
    public static AudioFader Instance
    {
        get
        {
            if (_instance == null)
            {
                // Create a hidden GameObject if it doesn’t exist yet
                var obj = new GameObject("AudioFader (Singleton)");
                obj.hideFlags = HideFlags.HideAndDontSave; // keep it hidden in hierarchy
                _instance = obj.AddComponent<AudioFader>();
                DontDestroyOnLoad(obj); // survive scene loads
            }
            return _instance;
        }
    }

    /// <summary>
    /// Public entry to fade an AudioSource.
    /// </summary>
    public void Fade(AudioSource source, float targetVolume, float duration)
    {
        StartCoroutine(FadeAudio(source, targetVolume, duration));
    }

    private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        if (source != null)
        {
            while (time < duration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
                yield return null;
            }

            if (targetVolume <= 0f)
                source.Stop();
        }
    }
}
