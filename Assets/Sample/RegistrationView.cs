using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class RegistrationView : MonoBehaviour
    {
        public void SetConfirmNicknameCallback(Action callback)
        {
            m_ConfirmNicknameCallback = callback;
            m_ConfirmNickname.onClick.RemoveListener(InvokeConfirmNicknameCallback);
            m_ConfirmNickname.onClick.AddListener(InvokeConfirmNicknameCallback);
        }
        
        public void EnableRegistrationPanel()
        {
            m_RegistrationPanel.gameObject.SetActive(true);
        }
        
        public void DisableRegistrationPanel()
        {
            m_RegistrationPanel.gameObject.SetActive(false);
        }

        public string GetInputNickname()
        {
            return m_NicknameBox.text;
        }

        private void InvokeConfirmNicknameCallback()
        {
            m_ConfirmNicknameCallback?.Invoke();
        }
        
        [SerializeField] private GameObject m_RegistrationPanel;
        [SerializeField] private InputField m_NicknameBox;
        [SerializeField] private Button m_ConfirmNickname;

        private Action m_ConfirmNicknameCallback;
    }
}