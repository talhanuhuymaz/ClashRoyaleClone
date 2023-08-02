using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadScreen;
    public Image barFill;

    private AsyncOperationHandle<SceneInstance> _sceneHandle;

    public void LoadSceneAsync()
    {
        LoadScreen.SetActive(true); // Set the loading screen active at the start of loading

        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        // Start loading the scene
        _sceneHandle = Addressables.LoadSceneAsync("Assets/ClashRoyale/Scenes/SampleScene.unity", LoadSceneMode.Single, activateOnLoad: true);

        // Wait until the loading is complete
        while (!_sceneHandle.IsDone)
        {
            float progressValue = Mathf.Clamp01(_sceneHandle.PercentComplete / 0.9f);
            barFill.fillAmount = progressValue;
            yield return null;
        }


        // Disable the loading screen after loading is complete
        LoadScreen.SetActive(false);
    }
}