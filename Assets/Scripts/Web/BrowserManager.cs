using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BrowserManager;

public class BrowserManager : MonoBehaviour {
    [Serializable]
    public struct Site {
        public string URL;
        public GameObject Obj;
    }

    [SerializeField]
    private TMP_InputField _headerInput;

    [SerializeField]
    private List<Site> _sites;
    [SerializeField]
    private string _mainSiteUrl;
    [SerializeField]
    private Site _notFoundSite;

    //[SerializeField]
    //private string _loginUniWebsiteUrl;
    //[SerializeField]
    //private string _storyLoginId;
    //[SerializeField]
    //private string _taskBeforeLoginPageId;
    //[SerializeField]
    //private string _taskAfterLoginPageId;

    private Site _activeSite;
    private Stack<Site> _backSites = new Stack<Site>();
    private Stack<Site> _forwardSites = new Stack<Site>();


    private void Start() {
        SetMainSite();

        _headerInput.onEndEdit.AddListener((value) => {
            if (Input.GetKeyDown(KeyCode.Return)) {
                ChangeSite(value);
            }
        });
    }

    private void Update() {

    }

    private void SetMainSite() {
        bool setActiveSite = false;
        foreach (var site in _sites) {
            if (site.URL == _mainSiteUrl) {
                _activeSite = site;
                setActiveSite = true;
            }
        }
        if (!setActiveSite) {
            Debug.LogWarning("Main site url doesn t exist in the list of sites!");
        }
    }

    //private void EnableWebsiteStoryBasedOfTask(string baseURL, string websiteUrl, 
    //                                                        string storyId, string taskId, string nextTaskId)
    //{
    //    if (baseURL == websiteUrl)
    //    {
    //        if (TasksController.Instance.CheckCurrentTask(taskId))
    //        {
    //            TasksController.Instance.ActivateTask(nextTaskId);
    //            DialogueController.Instance.PlayStory(storyId);
    //        }
    //    }
    //}

    private void ChangeSite(string url) {

        //base url without params
        //we use this to navigate to other page
        // and params are just passed to the page
        string baseUrl = url.Split('?')[0];

        foreach (var site in _sites) {
            if (site.URL == baseUrl) {
                //enable story and next task after user gets to the login page of the  university website
                //EnableWebsiteStoryBasedOfTask(baseUrl, _loginUniWebsiteUrl, _storyLoginId,
                //    _taskBeforeLoginPageId, _taskAfterLoginPageId);

                _backSites.Push(_activeSite);
                _forwardSites.Clear();
                _activeSite.Obj.SetActive(false);
                _activeSite = site;
                site.Obj.SetActive(true);

                return;
            }
        }


        if (_notFoundSite.Obj != null) {
            ShowNotFoundPage();
        }
    }

    public void GoBack() {
        if (_backSites.Count <= 0)
            return;

        var prevSite = _backSites.Pop();
        _forwardSites.Push(_activeSite);
        _activeSite.Obj.SetActive(false);
        _activeSite = prevSite;
        prevSite.Obj.SetActive(true);
    }

    public void GoForward() {
        if (_forwardSites.Count <= 0)
            return;

        var forwardSite = _forwardSites.Pop();
        _backSites.Push(_activeSite);
        _activeSite.Obj.SetActive(false);
        _activeSite = forwardSite;
        forwardSite.Obj.SetActive(true);
    }


    private void ShowNotFoundPage() {
        if (_activeSite.URL == _notFoundSite.URL)
            return;

        _backSites.Push(_activeSite);
        _forwardSites.Clear();
        _activeSite.Obj.SetActive(false);
        _activeSite = _notFoundSite;
        _notFoundSite.Obj.SetActive(true);
    }
}
