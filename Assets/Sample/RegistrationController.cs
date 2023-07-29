using UnityEngine;

namespace Sample
{
    public class RegistrationController : MonoBehaviour
    {
        public string GetNickName()
        {
            if (string.IsNullOrEmpty(m_PlayerNickname))
            {
                Debug.LogError($"{nameof(RegistrationController)} >>> player nickname is empty");
                return string.Empty;
            }

            return m_PlayerNickname;
        }
        private void Start()
        {
            m_RegistrationView.SetConfirmNicknameCallback(ConfirmNicknameCallback);
            Authorization();
        }

        private void Authorization()
        {
            var isPlayerRegistered = IsPlayerRegistered();
            if (isPlayerRegistered)
            {
                m_RegistrationView.DisableRegistrationPanel();
                return;
            }
            
            m_RegistrationView.EnableRegistrationPanel();
        }

        private void ConfirmNicknameCallback()
        {
            var inputNickname = m_RegistrationView.GetInputNickname();
            if (string.IsNullOrEmpty(inputNickname))
            {
                Debug.LogError($"{nameof(RegistrationController)} >>> inputNickname is empty");
                return;
            }

            m_PlayerNickname = inputNickname;
            SavePlayerNickname();
            m_RegistrationView.DisableRegistrationPanel();
        }
        
        private bool IsPlayerRegistered()
        {
            var playerNickname = PlayerPrefs.GetString(m_IsAlreadyRegistered, string.Empty);
            if (string.IsNullOrEmpty(playerNickname))
            {
                return false;
            }

            m_PlayerNickname = playerNickname;
            return true;
        }
        
        private void SavePlayerNickname()
        { 
            PlayerPrefs.SetString(m_IsAlreadyRegistered, m_PlayerNickname);
        }

        [SerializeField] private RegistrationView m_RegistrationView;
        
        private const string m_IsAlreadyRegistered = "IsAlreadyRegistered";

        private string m_PlayerNickname;
    }
}