using System.Collections;
using UnityEngine;

public class DontDestroyCoroutineProvider : MonoBehaviour
{
    public static Coroutine DoCoroutine(IEnumerator coroutine)
    {
        if (m_DontDestroyCoroutineProvider == null)
        {
            var obj = new GameObject($"{nameof(DontDestroyCoroutineProvider)}");    
            m_DontDestroyCoroutineProvider = obj.AddComponent<DontDestroyCoroutineProvider>();
            DontDestroyOnLoad(m_DontDestroyCoroutineProvider);
        }

        return m_DontDestroyCoroutineProvider.StartCoroutine(coroutine);
    }

    public static void Stop(Coroutine coroutine)
    {
        if (m_DontDestroyCoroutineProvider != null)
        {
            if (coroutine != null)
            {
                m_DontDestroyCoroutineProvider.StopCoroutine(coroutine);
            }
            else
            {
                Debug.LogError($"{nameof(DontDestroyCoroutineProvider)} >>> {nameof(coroutine)} doesnt exist");
            }
        }
        else
        {
            Debug.LogError($"{nameof(DontDestroyCoroutineProvider)} >>> {nameof(DontDestroyCoroutineProvider)} doesnt exist");
        }
    }

    public static void Destroy()
    {
        m_DontDestroyCoroutineProvider.StopAllCoroutines();
        Destroy(m_DontDestroyCoroutineProvider.gameObject);
        m_DontDestroyCoroutineProvider = null;
    }

    private void OnDestroy()
    {
        if (m_DontDestroyCoroutineProvider != null)
        {
            m_DontDestroyCoroutineProvider.StopAllCoroutines();
        }
    }

    private static DontDestroyCoroutineProvider m_DontDestroyCoroutineProvider;
}