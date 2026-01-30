using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


namespace Logic
{
    public class MyAudioManager : MonoBehaviour
    {
        public static MyAudioManager Instance { get; private set; }
        
        [SerializeField] private AudioSource _effectsSource;
        [SerializeField] private AudioSource _musicSource;
        
        [SerializeField] private AudioClip _cellClickSound;
        [SerializeField] private AudioClip _maskClickSound;
        
        [SerializeField] private AudioClip _mainMenuMusic;
        [SerializeField] private List<AudioClip> _gameMusic;

        [SerializeField] private int fadeDuration;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            
                Instance = this;
                _effectsSource = GetComponent<AudioSource>();
            }
            
   
        }

        private void Start()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            
            if (sceneName != "MainMenu")
            {
                StartCoroutine(FadeOutIn(_mainMenuMusic));
            }
            else
            {
                StartCoroutine(FadeOutIn(_gameMusic[Random.Range(0, _gameMusic.Count)]));
            }
        }

        public void PlayCellClickSound()
        {
            _effectsSource.PlayOneShot(_cellClickSound);
        }

        public void PlayMaskClickSound()
        {
            _effectsSource.PlayOneShot(_maskClickSound);
        }

        public void Mute()
        {
            _musicSource.mute = true;
        }

        public void Unmute()
        {
            _musicSource.mute = false;
        }
        
        private IEnumerator FadeOutIn(AudioClip newClip)
        {
            // Fade Out
            float startVolume = _musicSource.volume;

            while (_musicSource.volume > 0)
            {
                _musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            _musicSource.Stop();
            _musicSource.volume = 0;

            // Cambia clip
            _musicSource.clip = newClip;
            _musicSource.Play();

            // Fade In
            while (_musicSource.volume < startVolume)
            {
                _musicSource.volume += startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            _musicSource.volume = startVolume;
        }
        
        
    }
}