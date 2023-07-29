using System;
using NativeWebSocket;
using UnityEngine;
using WebSocket;

namespace Sample
{
    public class WebSocketClient : MonoBehaviour
    {
        public event Action OnConnectionError;
        public event Action<string> OnGetMessage;

        public void SendMessageToSocket(string message)
        {
            m_WebSocketManager.SendMessage(message);
        }
        
        private void Start()
        {
            OnConnectionError = ConnectionError;
            m_WebSocketManager.Connection(m_Url, OnOpenWebSocket, OnErrorWebSocket, OnCloseWebSocket, OnMessageWebSocket);
        }

        private void OnApplicationQuit()
        {
            m_WebSocketManager.Close();
        }

        private void OnOpenWebSocket()
        {
            Log("WebSocket open connection");
        }
        
        private void OnErrorWebSocket(string e)
        {
            LogError($"WebSocket error\nError: {e}");
            OnConnectionError?.Invoke();
        }

        private void OnCloseWebSocket(WebSocketCloseCode closeCode)
        {
            Log($"WebSocket close\nCode: {closeCode.ToString()}");
            if (closeCode != WebSocketCloseCode.Normal)
            {
                OnConnectionError?.Invoke();
            }
        }

        private void OnMessageWebSocket(byte[] bytes)
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            if (message == WebSocketManager.EmptyValidJson)
            {
                Log("PING");
                return;
            }
            
            OnGetMessage?.Invoke(message); 
            Log($"WebSocket message\nMessage: {message}");
            
            // LogError($"{nameof(WebSocketClient)} message is null!");
        }
        
        private void ConnectionError()
        {
            LogError("Connection error");
        }

        private void Log(string log)
        {
            Debug.Log($"<color=yellow>{nameof(WebSocketClient)} >>></color> {log}");
        }

        private void LogError(string log)
        {
            Debug.LogError($"{nameof(WebSocketClient)} >>> {log}");
        }
        
        private const string m_Url = "ws://127.0.0.1:7890/Echo";
        
        private IWebSocketManager m_WebSocketManager = new WebSocketManager();
    }
}