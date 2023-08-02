using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;

namespace TowerRoyale
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private GameObject loadPanel;
        [SerializeField] private Slider slider;
        [SerializeField] private Text loadingPercentage;
        [SerializeField] private string addrasableName;

        [SerializeField] private GameObject downloadSizePanel;
        [SerializeField] private Text downloadSizeText;

        private AsyncOperationHandle _sceneHandle;
        private bool _isWaiting;


        private void Start()
        {
            ShowDownloadSize();
        }

        private void OnDisable()
        {
            if (_sceneHandle.IsValid())
            {
                _sceneHandle.Completed -= OnSceneLoaded;
            }
        }


        private void GoToNextLevel()
        {
            Addressables.LoadSceneAsync(addrasableName, UnityEngine.SceneManagement.LoadSceneMode.Single, true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CacheRemove();
            }

            if (!_isWaiting) return;

            loadingPercentage.text = $"{_sceneHandle.PercentComplete * 100}%";
            slider.value = _sceneHandle.PercentComplete;
        }

        public void SceneAsyncLoad()
        {
            downloadSizePanel.SetActive(false);
            loadPanel.transform.localScale = Vector3.one;

            _sceneHandle = Addressables.DownloadDependenciesAsync(addrasableName);
            _sceneHandle.Completed += OnSceneLoaded;

            _isWaiting = true;

        }

        private void OnSceneLoaded(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GoToNextLevel();

            }
        }

        private void CacheRemove()
        {
            Caching.ClearCache();
        }

        public void ShowDownloadSize()
        {
            StartCoroutine(DownloadSize());
        }

        IEnumerator DownloadSize()
        {
            var downloadSize = Addressables.GetDownloadSizeAsync(addrasableName);
            yield return downloadSize;
            if (downloadSize.Result > 0)
            {
                downloadSizeText.text = $"Download {downloadSize.Result / 1024f / 1024f:0.00} MB ?";
                downloadSizePanel.SetActive(true);
            }
            else
            {
                SceneAsyncLoad();
            }
        }
    }
}