using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WeatherOverlay : MonoBehaviour
{
    public Image lightningPanel;
    public float flashInDuration = 0.1f;
    public float fadeOutDuration = 0.5f;
    public float minCooldown = 1f;
    public float maxCooldown = 3f;
    public float maxOpacity = 0.3f;

    private bool isFlashing = false;

    [SerializeField] List<AudioClip> thunderSounds;
    AudioSource sounds;

    void Start()
    {
        sounds = GetComponent<AudioSource>();  
        lightningPanel.color = new Color(1, 1, 1, 0);
        StartCoroutine(RandomFlashCoroutine());
    }

    private IEnumerator RandomFlashCoroutine()
    {
        while (true)
        {
            // Lightning flash random intervals betweenmin and max cooldown
            float cooldown = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(cooldown);
            TriggerFlash();
        }
    }

    public void TriggerFlash()
    {
        int numberOfFlashes = Random.Range(0, 5);

        
            if (!isFlashing)
            {
                StartCoroutine(FlashCoroutine());
            }
        
        
       
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        // Flash fade in
        float elapsedTime = 0f;
        while (elapsedTime < flashInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, maxOpacity, elapsedTime / flashInDuration);
            lightningPanel.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Flash fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(maxOpacity, 0, elapsedTime / fadeOutDuration);
            lightningPanel.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Ensure the panel is fully invisible after fade out
        lightningPanel.color = new Color(1, 1, 1, 0);

        sounds.PlayOneShot(thunderSounds[Random.Range(0, thunderSounds.Count)]);

        isFlashing = false;
    }
}
