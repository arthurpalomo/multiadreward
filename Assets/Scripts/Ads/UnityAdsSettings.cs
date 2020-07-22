using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnityAdsSettings : ScriptableObject
{
    public string GameId
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return _iOSGameId;
                case RuntimePlatform.Android:
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    return _androidGameId;
                default:
                    return "";
            }
        }
    }

    public bool TestMode
    {
        get
        {
            return _testMode;
        }
    }

    public bool PerPlacementLoad
    {
        get
        {
            return _perPlacementLoad;
        }
    }

    [SerializeField] string _androidGameId = default;
    [SerializeField] string _iOSGameId = default;
    [SerializeField] bool _testMode = default;
    [SerializeField] bool _perPlacementLoad = default;
}