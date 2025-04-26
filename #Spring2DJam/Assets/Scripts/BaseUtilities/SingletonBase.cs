using UnityEngine;

namespace BaseUtilities
{
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;
        public static T Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(this as T);
            }
            else Destroy(this as T);
        }	
    }
}