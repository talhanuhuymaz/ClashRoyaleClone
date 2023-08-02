using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AssetSize : MonoBehaviour
{
    public AssetReference assetLabel;
    public Text assetSizeText;

    private void Start()
    {
        // Add a listener to the button click event.
        Button downloadButton = GetComponent<Button>();
        downloadButton.onClick.AddListener(OnDownloadButtonClicked);
    }

    private async void OnDownloadButtonClicked()
    {
        await PrintAssetSize(assetLabel);
    }

    private async Task PrintAssetSize(AssetReference label)
    {
        AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(label);

        await sizeHandle.Task;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            long bytes = sizeHandle.Result;
            float megabytes = bytes / (1024f * 1024f);
            string formattedSize = megabytes.ToString("F2") + " MB";
            Debug.Log("Asset size of '" + label + "' is: " + formattedSize);

            // Update the UI text with the asset size.
            assetSizeText.text = "Download Size: " + formattedSize;
        }
        else
        {
            Debug.LogError("Failed to get the asset size for label: " + label);
        }
    }
}