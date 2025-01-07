using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] List<GameObject> healthIcons;
    
    void Start()
    {
        SetHealthIcons(3);
    }

    public void SetHealthIcons(int health)
    {
        foreach (var icon in healthIcons)
        {
            icon.SetActive(false);
        }
        
        for (int i = 0; i < health; i++)
        {
            healthIcons[i].SetActive(true);
        }
    }
}