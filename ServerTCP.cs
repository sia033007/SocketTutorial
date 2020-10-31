using System;
using System.Net;
using System.Net.Sockets;

namespace SocketTutorial
{
    class ServerTCP
    {
        private static Socket _serversocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];
        public static Client[] _clients = new Client[Constants._maxPlayer];
        public static void SetupServer(){
            _serversocket.Bind(new IPEndPoint(IPAddress.Any,5555));
            _serversocket.Listen(10);
            _serversocket.BeginAccept(new AsyncCallback(AcceptCallback),null);
        }
        private static void AcceptCallback(IAsyncResult ar){
            Socket socket = _serversocket.EndAccept(ar);
            _serversocket.BeginAccept(new AsyncCallback(AcceptCallback),null);
            for(int i=0; i<Constants._maxPlayer; i++){
                if(_clients[i].socket == null){
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    _clients[i].StartClient();
                    Console.WriteLine("connection from '{0}' received", _clients[i].ip);
                    return;
                }
            }

        }
        public static void SendDataTo(int index , byte[] data){
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >>8);
            sizeinfo[2] = (byte)(data.Length >>16);
            sizeinfo[3] = (byte)(data.Length >>24);

            _clients[index].socket.Send(sizeinfo);
            _clients[index].socket.Send(data);

        }
        public static void SendConnectionOK(int index){
            
        }
    }
    class Client
    {
        public int index;
        public string ip;
        public Socket socket;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];

        public void StartClient(){
            socket.BeginReceive(_buffer,0,_buffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),socket);
            closing =false;
        }
        private void ReceiveCallback(IAsyncResult ar){
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);
                if(received <=0){
                    CloseClient(index);
                }
                else
                {
                    byte[] databuffer = new byte[received];
                    Array.Copy(_buffer,databuffer,received);
                    //handle netwrok information
                    socket.BeginReceive(_buffer,0,_buffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),socket);

                }

            }
            catch
            {
                CloseClient(index);
            }
        }
        private void CloseClient(int index){
            closing = true;
            Console.WriteLine("connection from {0} has been terminated.",ip);
            //player left game
            socket.Close();
        }
    }
}