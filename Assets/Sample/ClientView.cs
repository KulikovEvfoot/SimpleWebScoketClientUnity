using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class ClientView : MonoBehaviour
    {
        public void AddTextToChatBox(string message)
        {
            m_ChatBox.text += message;
        }

        public string GetInputText()
        {
            return m_InputBox.text;
        }

        public void ClearInputText()
        {
            m_InputBox.text = string.Empty;
        }
        
        public void SetSendMessageCallback(Action callback)
        {
            m_SendMessageCallback = callback;
            m_SendMessage.onClick.RemoveListener(InvokeSendMessageCallback);
            m_SendMessage.onClick.AddListener(InvokeSendMessageCallback);
        }

        private void InvokeSendMessageCallback()
        {
            m_SendMessageCallback?.Invoke();
        }

        [SerializeField] private Text m_ChatBox;
        [SerializeField] private InputField m_InputBox;
        [SerializeField] private Button m_SendMessage;

        private Action m_SendMessageCallback;
    }
}