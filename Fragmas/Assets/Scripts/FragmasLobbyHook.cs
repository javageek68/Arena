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
        Debug.Log("entered OnLobbyServerSceneLoadedForPlayer");
        //this gets called when we are loading our game scene.
        //copy lobby player components to game player.
        Debug.Log("get ref to lPlayer");
        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        Debug.Log("get ref to gPlayer");
        Player gPlayer = gamePlayer.GetComponent<Player>();

        Debug.Log("set playerName " + lPlayer.playerName);
        gPlayer.playerName = lPlayer.playerName;

        Debug.Log("set playerColor " + lPlayer.playerColor.ToString());
        gPlayer.playerColor = lPlayer.playerColor;
        Debug.Log("leaving OnLobbyServerSceneLoadedForPlayer");
    }
}