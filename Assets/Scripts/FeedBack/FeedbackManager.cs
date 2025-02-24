using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// <summary>
/// Purpose: Manages the feedback system, dynamically assigning platform-specific URLs and handling user interactions.
/// Responsibilities: Sets feedback links based on platform, opens URLs, and controls panel visibility.
/// Usage: Works with `ReleaseHandler` to enable/disable feedback and is triggered by UI button actions.
/// </summary>

public class FeedbackManager : MonoBehaviour
{
    [Header("Panel GameObject")]
    [SerializeField] private GameObject feedbackPanel;

    private string playtestFormURL;
    private string bugReportURL;
    private string featureSuggestURL;
    private string communityJoinURL;

    private void Awake()
    {
        SetPlatformURLs();
    }

    private void SetPlatformURLs()
    {
        // Set URLs based on platform
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            playtestFormURL = Global.PlaytestFormURL_Web;
            bugReportURL = Global.BugReportURL_Web;
            featureSuggestURL = Global.FeatureSuggestURL_Web;
            communityJoinURL = Global.CommunityJoinURL_Web;
        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            playtestFormURL = Global.PlaytestFormURL_Mobile;
            bugReportURL = Global.BugReportURL_Mobile;
            featureSuggestURL = Global.FeatureSuggestURL_Mobile;
            communityJoinURL = Global.CommunityJoinURL_Mobile;
        }
        else // Default to PC
        {
            playtestFormURL = Global.PlaytestFormURL_PC;
            bugReportURL = Global.BugReportURL_PC;
            featureSuggestURL = Global.FeatureSuggestURL_PC;
            communityJoinURL = Global.CommunityJoinURL_PC;
        }
    }

    public void ShowFeedbackPanel()
    {
        SetActiveState(feedbackPanel, true);
    }

    public void HideFeedbackPanel()
    {
        SetActiveState(feedbackPanel, false);
    }

    private void SetActiveState(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }

    private void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    // Public methods to trigger feedback actions
    public void RatePlaytest() => OpenURL(playtestFormURL);
    public void ReportBug() => OpenURL(bugReportURL);
    public void SuggestFeature() => OpenURL(featureSuggestURL);
    public void JoinCommunity() => OpenURL(communityJoinURL);
}
