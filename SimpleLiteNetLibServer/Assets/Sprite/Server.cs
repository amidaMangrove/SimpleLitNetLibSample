using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

namespace HxCx {
    public class Server : MonoBehaviour, INetEventListener {

        NetManager _server;

        [SerializeField]
        int _port;

        [SerializeField]
        int _updateTime;

        [SerializeField]
        Button _leftButon;

        [SerializeField]
        Button _centerButton;

        [SerializeField]
        Button _rightButon;


        NetDataWriter _dataWriter = new NetDataWriter();

        private NetPeer _ourPeer;

        float _velocityX;

        // Start is called before the first frame update
        void Start()
        {
            _server = new NetManager(this);
            _server.Start(_port);
            _server.UpdateTime = _updateTime;

            _leftButon.onClick.AddListener(() =>
            {
                _velocityX = -1;
            });

            _centerButton.onClick.AddListener(() =>
            {
                _velocityX = 0;
            });

            _rightButon.onClick.AddListener(() =>
            {
                _velocityX = 1;
            });
        }

        // Update is called once per frame
        void Update()
        {
            _server.PollEvents();

        }
        void FixedUpdate()
        {
            if(_ourPeer != null) {

                _dataWriter.Reset();
                _dataWriter.Put(_velocityX);
                _ourPeer.Send(_dataWriter, DeliveryMethod.Sequenced);
            }
        }
        /// <summary>
        /// 接続リクエストの確認
        /// </summary>
        /// <param name="request"></param>
        public void OnConnectionRequest(ConnectionRequest request)
        {
            if(_server.ConnectedPeersCount < 5) {
                request.AcceptIfKey("SampleKey");
            }
            else {
                request.Reject();
            }
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
            Debug.Log($"{this.name} : OnPeerConnected");
            _ourPeer = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log($"{this.name} : OnPeerDisconnected");
        }

        
    }
}
