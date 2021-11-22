using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Code.Cinematics
{
    [Serializable]
    internal struct SceneClip
    {
        public GameObject sceneRoot;
        public float sceneTime;
    }


    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private Image _fadePanel;
        [Space]
        [SerializeField] private SceneClip[] clips;
        private int _currentIndex = 0;
        private bool _fadedIn;
        private bool _fadedOut;
        private SceneClip _currentClip;
        
        void Awake()
        {
            Assert.AreNotEqual(clips.Length, 0, "At least one clip was expected");
        }

        private void Start()
        {
            var c = _fadePanel.color;
            _fadePanel.color = new Color(c.r, c.g, c.b, 1);

            foreach (var clip in clips)
                clip.sceneRoot.SetActive(false);
            
            _currentClip = clips[_currentIndex];
            
            if (!_currentClip.sceneRoot.activeInHierarchy)
                _currentClip.sceneRoot.SetActive(true);

            _fadePanel.DOFade(0, 1).onComplete += () =>
                StartCoroutine(SceneChange(_currentClip.sceneTime, 1, 1));
        }

        private bool LoadNextClip()
        {
            if (_currentIndex < clips.Length - 1)
            {
                _currentClip.sceneRoot.SetActive(false);
                _currentIndex++;
                _currentClip = clips[_currentIndex];
                _currentClip.sceneRoot.SetActive(true);
                return true;
            }
            
            return false;
        }

        private IEnumerator SceneChange(float waitTime, float inTime, float outTime)
        {
            // Wait for clip to finish
            yield return new WaitForSeconds(waitTime);
            
            // Fade in
            _fadePanel.DOFade(1, inTime).onComplete += () => _fadedIn = true;
            yield return new WaitUntil(() => _fadedIn);
            
            // When screen is black load next scene
            var shouldContinue = LoadNextClip();
            
            if(!shouldContinue)
            {
                SceneManager.LoadScene("Level 1");
                yield break;
            }
            
            // Fade out
            _fadePanel.DOFade(0, outTime).onComplete += () => _fadedOut = true;
            yield return new WaitUntil(() => _fadedOut);
            
            // Reset and wait for next clip
            _fadedIn = false;
            _fadedOut = false;
            
            StartCoroutine(SceneChange(_currentClip.sceneTime, 1, 1));
        }
    }
}