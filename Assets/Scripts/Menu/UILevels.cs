using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class UILevels : MonoBehaviour
{
    [SerializeField]
    private List<Button> _levelButtons;

    private int _levelsAccessible;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateButtons();
    }

    public void SaveLevel(int levelIndex)
    {
        if (levelIndex > _levelsAccessible)
        {
            PlayerPrefs.SetInt("LevelIndex", levelIndex);
            PlayerPrefs.Save();

            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        _levelsAccessible = PlayerPrefs.GetInt("LevelIndex", 1);
        int i = 0;
        for (i = 0; i < _levelsAccessible; i++)
        {
            _levelButtons[i].interactable = true;
        }
        for (int j = i; j < _levelButtons.Count; j++)
        {
            _levelButtons[j].interactable = false;
        }
    }
}
