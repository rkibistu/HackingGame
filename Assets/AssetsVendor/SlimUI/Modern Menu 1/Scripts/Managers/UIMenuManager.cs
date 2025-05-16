using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;

namespace SlimUI.ModernMenu {
    public class UIMenuManager : MonoBehaviour {
        private Animator CameraObject;

        // campaign button sub menu
        [Header("MENUS")]
        [Tooltip("The Menu for when the MAIN menu buttons")]
        public GameObject mainRoot;
        [Tooltip("THe first menu displayed. Should be the LOGIN")]
        public GameObject _loginMenu;
        [Tooltip("The Menu for when the PLAY button is clicked")]
        public GameObject playMenu;
        [Tooltip("The Menu for when the EXIT button is clicked")]
        public GameObject exitMenu;
        [Tooltip("The Menu for LOGIN. The game starts with this one.")]
        public GameObject _mainMenu;
        [Tooltip("The Menu for REGISTER form.")]
        public GameObject _registerMenu;


        public enum Theme { custom1, custom2, custom3 };
        [Header("THEME SETTINGS")]
        public Theme theme;
        private int themeIndex;
        public ThemedUIData themeController;

        [Header("PANELS")]
        [Tooltip("The UI Panel parenting all sub menus")]
        public GameObject mainCanvas;
        [Tooltip("The UI Panel that holds the CONTROLS window tab")]
        public GameObject PanelControls;
        [Tooltip("The UI Panel that holds the VIDEO window tab")]
        public GameObject PanelVideo;
        [Tooltip("The UI Panel that holds the GAME window tab")]
        public GameObject PanelGame;
        [Tooltip("The UI Panel that holds the KEY BINDINGS window tab")]
        public GameObject PanelKeyBindings;
        [Tooltip("The UI Sub-Panel under KEY BINDINGS for MOVEMENT")]
        public GameObject PanelMovement;
        [Tooltip("The UI Sub-Panel under KEY BINDINGS for COMBAT")]
        public GameObject PanelCombat;
        [Tooltip("The UI Sub-Panel under KEY BINDINGS for GENERAL")]
        public GameObject PanelGeneral;


        // highlights in settings screen
        [Header("SETTINGS SCREEN")]
        [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
        public GameObject lineGame;
        [Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
        public GameObject lineVideo;
        [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
        public GameObject lineControls;
        [Tooltip("Highlight Image for when KEY BINDINGS Tab is selected in Settings")]
        public GameObject lineKeyBindings;
        [Tooltip("Highlight Image for when MOVEMENT Sub-Tab is selected in KEY BINDINGS")]
        public GameObject lineMovement;
        [Tooltip("Highlight Image for when COMBAT Sub-Tab is selected in KEY BINDINGS")]
        public GameObject lineCombat;
        [Tooltip("Highlight Image for when GENERAL Sub-Tab is selected in KEY BINDINGS")]
        public GameObject lineGeneral;

        [Header("LOADING SCREEN")]
        [Tooltip("If this is true, the loaded scene won't load until receiving user input")]
        public bool waitForInput = true;
        public GameObject loadingMenu;
        [Tooltip("The loading bar Slider UI element in the Loading Screen")]
        public Slider loadingBar;
        public TMP_Text loadPromptText;
        public KeyCode userPromptKey;

        [Header("SFX")]
        [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
        public AudioSource hoverSound;
        [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
        public AudioSource sliderSound;
        [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
        public AudioSource swooshSound;

        private List<GameObject> _allMenues = new List<GameObject>();

        void Start() {
            CameraObject = transform.GetComponent<Animator>();

            mainRoot.SetActive(true);
            _loginMenu.SetActive(true);
            playMenu.SetActive(false);
            exitMenu.SetActive(false);
            _mainMenu.SetActive(false);
            _registerMenu.SetActive(false);

            SetThemeColors();

            //---
            RegisterAllMenues();
        }

        // ---INSERT MINE HRE---

        public void SwitchToRegister() {
            DisableAllMenues();
            _registerMenu.SetActive(true);
        }
        public void SwitchToLogin() {
            DisableAllMenues();
            _loginMenu.SetActive(true);
        }
        public void SwitchToMainMenu() {
            DisableAllMenues();
            _mainMenu.SetActive(true);
        }

        private void RegisterAllMenues() {
            _allMenues.Clear();
            if (_loginMenu)
                _allMenues.Add(_loginMenu);
            if (playMenu)
                _allMenues.Add(playMenu);
            if (exitMenu)
                _allMenues.Add(exitMenu);
            if (_mainMenu)
                _allMenues.Add(_mainMenu);
            if (_registerMenu)
                _allMenues.Add(_registerMenu);
        }
        private void DisableAllMenues() {
            foreach(var menu in _allMenues) {
                menu.SetActive(false);
            }
        }

        // -----------

        void SetThemeColors() {
            switch (theme) {
                case Theme.custom1:
                    themeController.currentColor = themeController.custom1.graphic1;
                    themeController.textColor = themeController.custom1.text1;
                    themeIndex = 0;
                    break;
                case Theme.custom2:
                    themeController.currentColor = themeController.custom2.graphic2;
                    themeController.textColor = themeController.custom2.text2;
                    themeIndex = 1;
                    break;
                case Theme.custom3:
                    themeController.currentColor = themeController.custom3.graphic3;
                    themeController.textColor = themeController.custom3.text3;
                    themeIndex = 2;
                    break;
                default:
                    Debug.Log("Invalid theme selected.");
                    break;
            }
        }

        public void PlayCampaign() {
            exitMenu.SetActive(false);
            playMenu.SetActive(true);
        }

        public void PlayCampaignMobile() {
            exitMenu.SetActive(false);
            playMenu.SetActive(true);
            mainRoot.SetActive(false);
        }

        public void ReturnMenu() {
            playMenu.SetActive(false);
            exitMenu.SetActive(false);
            mainRoot.SetActive(true);
        }

        public void LoadScene(string scene) {
            if (scene != "") {
                StartCoroutine(LoadAsynchronously(scene));
            }
        }

        public void DisablePlayCampaign() {
            playMenu.SetActive(false);
        }

        public void Position2() {
            DisablePlayCampaign();
            CameraObject.SetFloat("Animate", 1);
        }

        public void Position1() {
            CameraObject.SetFloat("Animate", 0);
        }

        void DisablePanels() {
            PanelControls.SetActive(false);
            PanelVideo.SetActive(false);
            PanelGame.SetActive(false);
            PanelKeyBindings.SetActive(false);

            lineGame.SetActive(false);
            lineControls.SetActive(false);
            lineVideo.SetActive(false);
            lineKeyBindings.SetActive(false);

            PanelMovement.SetActive(false);
            lineMovement.SetActive(false);
            PanelCombat.SetActive(false);
            lineCombat.SetActive(false);
            PanelGeneral.SetActive(false);
            lineGeneral.SetActive(false);
        }

        public void GamePanel() {
            DisablePanels();
            PanelGame.SetActive(true);
            lineGame.SetActive(true);
        }

        public void VideoPanel() {
            DisablePanels();
            PanelVideo.SetActive(true);
            lineVideo.SetActive(true);
        }

        public void ControlsPanel() {
            DisablePanels();
            PanelControls.SetActive(true);
            lineControls.SetActive(true);
        }

        public void KeyBindingsPanel() {
            DisablePanels();
            MovementPanel();
            PanelKeyBindings.SetActive(true);
            lineKeyBindings.SetActive(true);
        }

        public void MovementPanel() {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelMovement.SetActive(true);
            lineMovement.SetActive(true);
        }

        public void CombatPanel() {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelCombat.SetActive(true);
            lineCombat.SetActive(true);
        }

        public void GeneralPanel() {
            DisablePanels();
            PanelKeyBindings.SetActive(true);
            PanelGeneral.SetActive(true);
            lineGeneral.SetActive(true);
        }

        public void PlayHover() {
            hoverSound.Play();
        }

        public void PlaySFXHover() {
            sliderSound.Play();
        }

        public void PlaySwoosh() {
            swooshSound.Play();
        }

        // Are You Sure - Quit Panel Pop Up
        public void AreYouSure() {
            exitMenu.SetActive(true);
            DisablePlayCampaign();
        }

        public void AreYouSureMobile() {
            exitMenu.SetActive(true);
            mainRoot.SetActive(false);
            DisablePlayCampaign();
        }

        public void ExtrasMenu() {
            playMenu.SetActive(false);
            exitMenu.SetActive(false);
        }

        public void QuitGame() {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // Load Bar synching animation
        IEnumerator LoadAsynchronously(string sceneName) { // scene name is just the name of the current scene being loaded
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            mainCanvas.SetActive(false);
            loadingMenu.SetActive(true);

            while (!operation.isDone) {
                float progress = Mathf.Clamp01(operation.progress / .95f);
                loadingBar.value = progress;

                if (operation.progress >= 0.9f && waitForInput) {
                    loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
                    loadingBar.value = 1;

                    if (Input.GetKeyDown(userPromptKey)) {
                        operation.allowSceneActivation = true;
                    }
                }
                else if (operation.progress >= 0.9f && !waitForInput) {
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        public void LoadSceneByNameDirectly(string sceneName) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SceneManager.LoadScene(sceneName);
        }
    }


}