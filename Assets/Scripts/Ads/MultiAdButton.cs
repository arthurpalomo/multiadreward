using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Advertisements;

public class MultiAdButton : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] string _placementId = "rewardedVideo";
    [SerializeField] int _adsToWatchForReward = 2;
    [SerializeField] Button _button = default;
    [SerializeField] Text _buttonText = default;
    [SerializeField] UnityEvent _onAdsCompleted = default;
    [SerializeField] UnityEvent _onAdError = default;
    int _currentAdsWatched;
    float _timeout = 30;

    void OnEnable()
    {
        Advertisement.AddListener(this);
        _button.onClick.AddListener(OnButtonClicked);
        _button.gameObject.SetActive(false);
        _button.interactable = false;
        _buttonText.text = "";
    }

    void OnDisable()
    {
        Advertisement.RemoveListener(this);
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    [ContextMenu("Enable Button")]
    public void EnableButton()
    {
        _button.gameObject.SetActive(true);
        _button.interactable = true;
        _buttonText.text = "Watch Ads for Reward";
    }

    void DisableButton()
    {
        _button.gameObject.SetActive(false);
        _button.interactable = false;
        _buttonText.text = "";
    }

    void OnButtonClicked()
    {
        _currentAdsWatched = 0;
        _button.interactable = false;
        _buttonText.text = _currentAdsWatched + " of " + _adsToWatchForReward + " Complete";
        TryShowAd();
    }

    void TryShowAd()
    {
        if (Advertisement.IsReady(_placementId))
        {
            Advertisement.Show(_placementId);
        }
        else
        {
            StartCoroutine(ShowAdWhenReady());
        }
    }

    IEnumerator ShowAdWhenReady()
    {
        bool isTimedOut = false;
        float startTime = Time.realtimeSinceStartup;

        while (!isTimedOut && !Advertisement.IsReady(_placementId))
        {
            isTimedOut = (Time.realtimeSinceStartup > startTime + _timeout) ? true : false;
            yield return new WaitForSeconds(1f);
        }

        if (isTimedOut)
        {
            Debug.Log("Timed out - Placement: " + _placementId + " State: " + Advertisement.GetPlacementState(_placementId));
            _onAdError?.Invoke();
            DisableButton();
        }
        else
        {
            Advertisement.Show(_placementId);
        }
    }

    void OnAdCompleted()
    {
        _currentAdsWatched++;
        string status = _currentAdsWatched + " of " + _adsToWatchForReward + " Complete";
        _buttonText.text = status;
        Debug.Log(status);
        if(_currentAdsWatched == _adsToWatchForReward)
        {
            _onAdsCompleted?.Invoke();
            DisableButton();
            return;
        }
        TryShowAd();
    }

    public void OnUnityAdsReady(string placementId)
    {
        //if (!string.Equals(placementId, _placementId)) return;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //if (!string.Equals(placementId, _placementId)) return;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (!string.Equals(placementId, _placementId)) return;
        switch (showResult)
        {
            case ShowResult.Failed:
            case ShowResult.Finished:
            case ShowResult.Skipped:
            default:
                OnAdCompleted();
                break;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("OnUnityAdsDidError: " + message);
        _onAdError?.Invoke();
        DisableButton();
    }
}