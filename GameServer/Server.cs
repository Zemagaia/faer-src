using System.Net;
using System.Net.Sockets;
using GameServer.realm;
using NLog;

namespace GameServer; 

public class Server {
    private static Logger Log = LogManager.GetCurrentClassLogger();

    private Socket _listenSocket;

    private RealmManager _manager;

    private Queue<Client> _clientPool;

    public Server(RealmManager manager, int port)
    {
        Log.Info("Starting server...");
        _manager = manager;
        _clientPool = new Queue<Client>(512);
        for (int i = 0; i < 512; i++)
        {
            _clientPool.Enqueue(new Client(this, _manager));
        }
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
        _listenSocket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _listenSocket.Bind(endpoint);
        _listenSocket.Listen(128);
        Log.Info("Listening on port {0}...", port);
        Accept();
    }

    private async void Accept()
    {
        while (true)
        {
            Socket socket2;
            Socket socket = (socket2 = await _listenSocket.AcceptAsync(CancellationToken.None));
            if (socket2 != null)
            {
                _clientPool.Dequeue().Reset(socket);
            }
        }
    }

    public void Disconnect(Client client)
    {
        try
        {
            if (client.Socket.Connected)
            {
                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Close();
            }
            _clientPool.Enqueue(client);
        }
        catch (Exception e)
        {
            if (!(e is SocketException se) || (se.SocketErrorCode != SocketError.NotConnected && se.SocketErrorCode != SocketError.Shutdown))
            {
                Log.Error<Exception>(e);
            }
        }
    }

    public void Stop()
    {
        Log.Info("Stoping server...");
        try
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
        }
        catch (Exception e)
        {
            if (!(e is SocketException se) || se.SocketErrorCode != SocketError.NotConnected)
            {
                Log.Error<Exception>(e);
            }
        }
        _listenSocket.Close();
        Client[] array = _manager.Clients.Keys.ToArray();
        foreach (Client i in array)
        {
            i.Disconnect();
        }
    }
}