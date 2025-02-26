using UnityEngine;

namespace TheSentinel.Cores
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("Instance + " + typeof(T)).AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}
