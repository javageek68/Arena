using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class FragmasLobbyHook : LobbyHook
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="lobbyPlayer"></param>
    /// <param name="gamePlayer"></param>
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //this gets called when we are loading our game scene.
        //copy lobby player components to game player.
        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        Player gPlayer = gamePlayer.GetComponent<Player>();

        gPlayer.playerName = lPlayer.playerName;
        gPlayer.playerColor = lPlayer.playerColor;
    }
}