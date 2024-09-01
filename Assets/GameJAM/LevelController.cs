using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Player player;
    private bool isGameStart = false;
    private void Awake()
    {
        player.noMpRevert = true;
    }
    private void Update()
    {
        if (!isGameStart && player.noMpRevert && player.mp != 0)
        {
            player.mp = 0;
            isGameStart = true;
        }
    }
}
