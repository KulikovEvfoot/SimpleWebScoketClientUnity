using System.Text;
using UnityEngine;

namespace Sample
{
    public class ClientController : MonoBehaviour
    {
        private void Start()
        {
            Init();
        }
        
        private void Init()
        { 
            m_ClientView.SetSendMessageCallback(SendMessageToSocket);
            m_WebSocketClient.OnGetMessage -= UpdateChatBox;
            m_WebSocketClient.OnGetMessage += UpdateChatBox;
        }

        private void UpdateChatBox(string newMessage)
        {
            var newStr = $"\n{newMessage}";
            m_ClientView.AddTextToChatBox(newStr);
        }

        private void SendMessageToSocket()
        {
            var inputText = m_ClientView.GetInputText();
            if (string.IsNullOrEmpty(inputText))
            {
                Debug.LogError($"{nameof(ClientController)} >>> input text is empty");
                return;
            }

            var playerNickname = m_RegistrationController.GetNickName();
            m_MessageBuilder.Clear();
            m_MessageBuilder.Append(playerNickname);
            m_MessageBuilder.Append(": ");
            m_MessageBuilder.Append(inputText);
            m_WebSocketClient.SendMessageToSocket(m_MessageBuilder.ToString());
            //m_ClientView.ClearInputText();
        }
        
        [SerializeField] private WebSocketClient m_WebSocketClient;
        [SerializeField] private RegistrationController m_RegistrationController;
        [SerializeField] private ClientView m_ClientView;

        private StringBuilder m_MessageBuilder = new StringBuilder();
    }
}