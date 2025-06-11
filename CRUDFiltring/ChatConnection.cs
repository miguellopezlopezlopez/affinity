using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FiltringApp
{
    public class ChatConnection
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly string serverIp = "127.0.0.1";
        private readonly int serverPort = 2000;
        private bool isConnected = false;
        private readonly int userId;
        private readonly string username;

        public event EventHandler<ChatMessageEventArgs> MessageReceived;
        public event EventHandler<string> ConnectionStatusChanged;

        public ChatConnection(int userId, string username)
        {
            this.userId = userId;
            this.username = username;
        }

        public async Task ConnectAsync()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(serverIp, serverPort);
                stream = client.GetStream();
                isConnected = true;

                // Enviar datos de autenticación
                var authData = new
                {
                    user_id = userId,
                    username = username
                };
                
                string authJson = JsonConvert.SerializeObject(authData);
                byte[] authBytes = Encoding.UTF8.GetBytes(authJson);
                await stream.WriteAsync(authBytes, 0, authBytes.Length);

                // Iniciar recepción de mensajes
                Task.Run(ReceiveMessages);

                ConnectionStatusChanged?.Invoke(this, "Conectado al servidor de chat");
            }
            catch (Exception ex)
            {
                isConnected = false;
                ConnectionStatusChanged?.Invoke(this, $"Error al conectar: {ex.Message}");
                throw;
            }
        }

        public async Task SendMessageAsync(int receptorId, string content)
        {
            if (!isConnected)
            {
                throw new InvalidOperationException("No hay conexión con el servidor");
            }

            try
            {
                var messageData = new
                {
                    receptor_id = receptorId,
                    contenido = content
                };

                string messageJson = JsonConvert.SerializeObject(messageData);
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageJson);
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                isConnected = false;
                ConnectionStatusChanged?.Invoke(this, $"Error al enviar mensaje: {ex.Message}");
                throw;
            }
        }

        private async Task ReceiveMessages()
        {
            byte[] buffer = new byte[4096];

            while (isConnected)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // Conexión cerrada por el servidor
                        break;
                    }

                    string messageJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var messageData = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageJson);

                    var chatMessage = new ChatMessageEventArgs
                    {
                        EmisorId = int.Parse(messageData["emisor_id"]),
                        EmisorNombre = messageData["emisor_nombre"],
                        Contenido = messageData["contenido"],
                        Fecha = DateTime.Parse(messageData["fecha"])
                    };

                    MessageReceived?.Invoke(this, chatMessage);
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    ConnectionStatusChanged?.Invoke(this, $"Error en la conexión: {ex.Message}");
                    break;
                }
            }
        }

        public void Disconnect()
        {
            isConnected = false;
            stream?.Close();
            client?.Close();
            ConnectionStatusChanged?.Invoke(this, "Desconectado del servidor");
        }
    }

    public class ChatMessageEventArgs : EventArgs
    {
        public int EmisorId { get; set; }
        public string EmisorNombre { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }
    }
} 