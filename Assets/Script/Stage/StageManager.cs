using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// No good usage atm, but instantiates NetworkManager
public class StageManager : MonoBehaviour
{
    public GameObject NetworkManagerPrefab;

    public void Start()
    {
        Instantiate(NetworkManagerPrefab);
    }
}
