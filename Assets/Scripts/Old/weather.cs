using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weather : MonoBehaviour
{
    [SerializeField] List<GameObject> weatherEffects;
    
    void Start()
    {    
        if ( weatherEffects.Count > 0)
        {
            foreach (var weatherEffect in weatherEffects)
            {
                weatherEffect.SetActive(false);
            }

            int selection = Random.Range(0, weatherEffects.Count);
            weatherEffects[selection].SetActive(true);
        }
    }
}