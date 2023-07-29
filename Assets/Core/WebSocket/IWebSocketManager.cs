using System.Threading.Tasks;
using NativeWebSocket;

namespace WebSocket
{
    public interface IWebSocketManager
    { 
        void Connection(
            string url,
            WebSocketOpenEventHandler onOpen, 
            WebSocketErrorEventHandler onError, 
            WebSocketCloseEventHandler onClose,
            WebSocketMessageEventHandler onMessage);
        
        Task SendMessage(string message);
        void Close();
    }
}