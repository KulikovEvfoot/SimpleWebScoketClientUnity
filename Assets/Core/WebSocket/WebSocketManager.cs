using System.Collections;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;

namespace WebSocket
{
    public class WebSocketManager : IWebSocketManager
    {
        public const string EmptyValidJson = "{}";
        
        public async void Connection(
            string url,
            WebSocketOpenEventHandler onOpen, 
            WebSocketErrorEventHandler onError, 
            WebSocketCloseEventHandler onClose,
            WebSocketMessageEventHandler onMessage)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError($"{nameof(WebSocketManager)} >>> url is null or empty");
                return;
            }
            
            m_Websocket = new NativeWebSocket.WebSocket(url);
            Subscribe(onOpen, onError, onClose, onMessage);
            await m_Websocket.Connect();
        }
        
        public async Task SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError($"{nameof(WebSocketManager)} >>> {nameof(message)} is null or empty!");
                return;
            }
            
            await m_Websocket.SendText(message);
            
            m_DispatchMessageQueueRoutine = DontDestroyCoroutineProvider.DoCoroutine(DispatchMessageQueueRoutine());
        }

        public void Close()
        {
            if (m_DispatchMessageQueueRoutine != null)
            {
                DontDestroyCoroutineProvider.Stop(m_DispatchMessageQueueRoutine);
            }
            
            m_Websocket?.Close();
        }
        
        private void PongOnMessage(byte[] bytes)
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            if (message == EmptyValidJson)
            {
                m_Websocket.SendText(EmptyValidJson);
            }
        }

        private void OnClose(WebSocketCloseCode closeCode)
        {
            m_OnClose?.Invoke(closeCode);
            Unsubscribe();
        }
        
        private IEnumerator DispatchMessageQueueRoutine()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            while (m_Websocket.State == WebSocketState.Open)
            {
                m_Websocket.DispatchMessageQueue();
                yield return new WaitForEndOfFrame();
            }
#endif
        }

        private void Subscribe(
            WebSocketOpenEventHandler onOpen,
            WebSocketErrorEventHandler onError, 
            WebSocketCloseEventHandler onClose,
            WebSocketMessageEventHandler onMessage)
        {
            Unsubscribe();
            m_OnOpen = onOpen;
            m_OnError = onError;
            m_OnClose = onClose;
            m_OnMessage = onMessage;

            m_Websocket.OnOpen += m_OnOpen;
            m_Websocket.OnError += m_OnError;
            m_Websocket.OnClose += OnClose;
            m_Websocket.OnMessage += m_OnMessage;
            m_Websocket.OnMessage += PongOnMessage;
        }
        

        private void Unsubscribe()
        {
            m_Websocket.OnOpen -= m_OnOpen;
            m_Websocket.OnError -= m_OnError;
            m_Websocket.OnClose -= OnClose;
            m_Websocket.OnMessage -= m_OnMessage;
            m_Websocket.OnMessage -= PongOnMessage;
        }
        
        private NativeWebSocket.WebSocket m_Websocket;
        private Coroutine m_DispatchMessageQueueRoutine;
        private WebSocketOpenEventHandler m_OnOpen;
        private WebSocketErrorEventHandler m_OnError;
        private WebSocketCloseEventHandler m_OnClose;
        private WebSocketMessageEventHandler m_OnMessage;
    }
}