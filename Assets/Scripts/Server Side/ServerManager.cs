using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Server))]
public class ServerManager : MonoBehaviour {

    Dictionary<int, Player> players = new Dictionary<int, Player>();

    Server server;

    [SerializeField]
    GameObject playerPrefab;

    Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
        server = GetComponent<Server>();

        server.OnNetworkConnectEvent += HandleNetworkConnectEvent;
        server.OnNetworkDisconnectEvent += HandleNetworkDisconnectEvent;

        NetworkUtility.messageDelegates[EMessageType.ClientUpdate] += HandleClientUpdateMessage;
        //NetworkUtility.messageDelegates[EMessageType.Fire] += HandleFireMessage;

        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = new Transform[spawnObjects.Length];
        int i = 0;
        foreach (GameObject obj in spawnObjects)
        {
            spawnPoints[i++] = obj.transform;
        }
	}

    void HandleNetworkConnectEvent(int connectionId)
    {
        // spawn new player
        GameObject newPlayer = Instantiate(playerPrefab); //, spawnPoints[0].position, spawnPoints[0].rotation);
        Player player = newPlayer.GetComponent<Player>();
        if (player != null)
        {
            players.Add(connectionId, newPlayer.GetComponent<Player>());
            player.AddOnDeathHandler(HandlePlayerDeath);
            player.ConnectionId = connectionId;
            RespawnPlayers();
        }

    }

    void RespawnPlayers()
    {
        List<Transform> spawnPointsList = new List<Transform>(spawnPoints);
        foreach (Player player in players.Values)
        {
            int index = (int)Random.Range(0, spawnPointsList.Count);
            player.GetComponent<Transform>().SetPositionAndRotation(spawnPointsList[index].position, spawnPointsList[index].rotation);
            spawnPointsList.RemoveAt(index);
        }
    }

    void HandleNetworkDisconnectEvent(int connectionId)
    {
        Destroy(players[connectionId].gameObject);
        players.Remove(connectionId);
    }

    void HandleClientUpdateMessage(MessageBase msgBase)
    {
        ClientUpdateMessage message = (ClientUpdateMessage)msgBase;
        players[message.connectionId].UpdateFromClient(message);
    }

    void HandlePlayerDeath(int connectionId)
    {
        Debug.Log("player died");
        Invoke("RespawnPlayers", 1.5f);

        DeathMessage msg = new DeathMessage();
        server.SendDeathMessage(msg, connectionId);
    }

    //void HandleFireMessage(MessageBase msgBase)
    //{
    //    Debug.Log("Receiving fire message");
    //    FireMessage message = (FireMessage)msgBase;
    //    players[message.connectionId].Fire();
    //}
}
