using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour, INetEventListener
{
    NetManager _client;

    [SerializeField]
    string _address;

    [SerializeField]
    int _port;

    [SerializeField]
    string _key;

    [SerializeField]
    GameObject _playerPrefab;

    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _client = new NetManager(this);
        _client.Start();
        _client.Connect(_address, _port, _key);
    }

    // Update is called once per frame
    void Update()
    {
        _client.PollEvents();
    }
    public void OnConnectionRequest(ConnectionRequest request)
    {
        Debug.Log($"{this.name} : OnConnectionRequest");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.Log($"{this.name} : OnNetworkError");
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="peer"></param>
    /// <param name="latency"></param>
    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        Debug.Log($"{this.name} : OnNetworkLatencyUpdate");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        var pos = _player.transform.position;
        pos.x = reader.GetFloat();
        _player.transform.position = pos;
        Debug.Log($"{this.name} : OnNetworkReceive");
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        Debug.Log($"{this.name} : OnNetworkReceiveUnconnected");
    }

    /// <summary>
    /// 接続
    /// </summary>
    /// <param name="peer"></param>
    public void OnPeerConnected(NetPeer peer)
    {
        _player = Instantiate(_playerPrefab);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log($"{this.name} : OnPeerDisconnected");
    }

}
