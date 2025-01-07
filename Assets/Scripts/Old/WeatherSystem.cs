using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    // Enum for weather types
    public enum WeatherType
    {
        Clear,
        Fog
    }
    // Stores the current weather type
    public WeatherType currentWeather;
    // Reference for the fog particle system
    public ParticleSystem fogParticleSystem;

    private void Start()
    {
        // Makes the initial weather type clear
        SetWeather(WeatherType.Clear);
        DisableFog();
    }

    // Method to change the weather
    public void SetWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;

        if (currentWeather == WeatherType.Clear)
        {
            DisableFog();
        }
        else if (currentWeather == WeatherType.Fog)
        {
            EnableFog();
        }
    }

    private void Update()
    {
        //Used to change weather in other scripts

        //if (Input.GetKeyDown(KeyCode.F)) ; // Press F to toggle fog
        //{
        //    WeatherSystem.SetWeather(WeatherSystem.WeatherType.Fog);
        //}


        //Used to change weather in this script
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentWeather = WeatherType.Fog;
            // Call SetWeather to apply the change
            SetWeather(currentWeather);
        }
    }


    private void EnableFog()
    {
        if (fogParticleSystem != null)
        {
            fogParticleSystem.Play();
        }
    }

    private void DisableFog()
    {
        if (fogParticleSystem != null)
        {
            fogParticleSystem.Stop();
        }
    }
}
