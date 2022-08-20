using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Player player;

    private void Awake()
    {
        player.noMpRevert = true;
    }
    private void Update()
    {
        if (player.noMpRevert && player.mp != 0)
        {
            player.mp = 0;
        }
    }
}
