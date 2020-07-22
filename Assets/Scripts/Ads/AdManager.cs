using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsCallbacks
{
    public System.Action<string> OnReady;
    public System.Action<string> OnStart;
    public System.Action<string, ShowResult> OnFinish;
    public System.Action<string> OnError;
}

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] UnityAdsSettings _adSettings = default;
    UnityAdsCallbacks _callbacks;

    void Awake()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_adSettings.GameId, _adSettings.TestMode, _adSettings.PerPlacementLoad);
    }

    void OnDisable()
    {
        Advertisement.RemoveListener(this);
    }

    public void ShowAd(string placement, System.Action<string, ShowResult> completeCallback)
    {
        if(_callbacks != null)
            _callbacks.OnFinish = completeCallback;
        Advertisement.Show(placement);
    }

    public void ShowAd(string placement, UnityAdsCallbacks callbacks)
    {
        _callbacks = callbacks;
        Advertisement.Show(placement);
    }

    public void OnUnityAdsReady(string placementId)
    {
        _callbacks?.OnReady?.Invoke(placementId);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        _callbacks?.OnStart?.Invoke(placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        _callbacks?.OnFinish?.Invoke(placementId, showResult);
        _callbacks = null;
    }

    public void OnUnityAdsDidError(string message)
    {
        _callbacks.OnError?.Invoke(message);
        _callbacks = null;
    }
}