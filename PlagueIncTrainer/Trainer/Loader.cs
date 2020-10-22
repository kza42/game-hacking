using UnityEngine;

namespace Trainer
{
    public static class Loader
    {
        private static GameObject _load;
        
        public static void Init()
        {
            _load = new GameObject();
            _load.AddComponent<Main>();
            Object.DontDestroyOnLoad(_load);
        }
        
        public static void Unload()
        {
            _Unload();
        }
        
        private static void _Unload()
        {
            Object.Destroy(_load);
        }
    }
}