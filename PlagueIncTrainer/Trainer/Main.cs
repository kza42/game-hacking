using System.Linq;
using UnityEngine;

namespace Trainer
{
    internal class Main : MonoBehaviour
    {
        private bool _showHelp;
        
        public void Start()
        {
            _showHelp = true;
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                CGameManager.game.WorldInstance.diseases.First().evoPoints += 100;
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                _showHelp = !_showHelp;
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Loader.Unload();
            }
        }
        
        public void OnGUI()
        {
            if (!_showHelp) return;
            
            GUI.Label(new Rect(0f, 0f, 250f, 25f), "F1 - Add 100 evolution points");
            GUI.Label(new Rect(0f, 25f, 250f, 25f), "F11 - Toggle help");
            GUI.Label(new Rect(0f, 50f, 250f, 25f), "DELETE - Unload trainer");
        }
    }
}