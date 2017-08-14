using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    int maxHealth = 3;

    [SyncVar(hook = "OnHealthChanged")]
    int health;

    Player player;


    void Awake()
    {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// ServerCallback means that this method only exists on the server
    /// </summary>
    [ServerCallback]
    void OnEnable()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Server means that this method can only run on the server
    /// </summary>
    /// <returns></returns>
    [Server]
    public bool TakeDamage()
    {
        bool died = false;

        //check to see if we are already dead when we were shot
        //if so return false, so that the player that took the 
        //shot doesn't get credit 
        if (health <= 0)
            return died;

        health--;
        died = health <= 0;

        //play the died logic on each client
        RpcTakeDamage(died);

        return died;
    }

    /// <summary>
    /// ClientRpc will run on each client
    /// </summary>
    /// <param name="died"></param>
    [ClientRpc]
    void RpcTakeDamage(bool died)
    {
        //if this is the local player, then we need to have the UI
        //play the flash effect
        if (isLocalPlayer)
            PlayerCanvas.canvas.FlashDamageEffect();

        //all clients need to play the die animation for this player.
        if (died)
            player.Die();
    }

    void OnHealthChanged(int value)
    {
        health = value;
        if (isLocalPlayer)
            PlayerCanvas.canvas.SetHealth(value);
    }
}