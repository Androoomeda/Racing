using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static bool pause = true;
    [SerializeField] private GameObject player;

    public void Pause()
    {
        pause = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pause = false;
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        playerStats.timer = 0f;
        playerRb.velocity = Vector3.zero;
        player.transform.position = playerController.start;

        Resume();
    }
}
