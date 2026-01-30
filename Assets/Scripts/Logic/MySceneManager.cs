using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class MySceneManager : MonoBehaviour
    {
        public static MySceneManager Instance {get; private set;}
        
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void GoToGame()
        {
            SceneManager.LoadScene("MainGame");
        }

        public void QuitGame()
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }
}