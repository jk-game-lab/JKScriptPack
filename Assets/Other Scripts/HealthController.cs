using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthController : MonoBehaviour
{
    public int currentHealth;
    public int MaxHealth;

    public CharacterController thePlayer;

    private bool isRespawning;
    public Vector3 respawnPoint;

    void Start()
    {
        thePlayer = FindObjectOfType<CharacterController>();
        respawnPoint = thePlayer.transform.position;
        currentHealth = MaxHealth;
    }

    void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            DamagePlayer(5);
        }
    }

    public void DamagePlayer(int damageamount)
    {
        currentHealth -= damageamount;
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Debug.Log("Should respawn");
        thePlayer.transform.position = respawnPoint;
        Physics.SyncTransforms();
        currentHealth = MaxHealth;
    }
}