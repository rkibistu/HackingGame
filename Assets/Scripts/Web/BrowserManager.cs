using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private Site _activeSite;
    private Stack<Site> _backSites = new Stack<Site>();
    private Stack<Site> _forwardSites = new Stack<Site>();

    private void Start() {
        SetMainSite();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Return)) {
            ChangeSite(_headerInput.text);
        }
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

    private void ChangeSite(string url) {
        foreach (var site in _sites) {
            if (site.URL == url) {
                _backSites.Push(_activeSite);
                _forwardSites.Clear();
                _activeSite.Obj.SetActive(false);
                _activeSite = site;
                site.Obj.SetActive(true);

                return;
            }
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
}
