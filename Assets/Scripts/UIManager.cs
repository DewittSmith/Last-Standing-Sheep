using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText, statisticsText;

    [SerializeField] private Slider music, sound;
    [SerializeField] private Toggle vibration;

    [SerializeField] private CanvasGroup[] canvasGroups;
    [SerializeField] private int victoryGroup, looseGroup;
    [SerializeField] private ButtonActionBind[] buttonBinds;

    private void OnValidate()
    {
                                       
        if (buttonBinds.Length > 0)
            foreach (var b in buttonBinds)
            {
                if (b.Button)
                    b.Name = b.Button.transform.parent.name + " : " + b.Button.name;
            }
    }

    private void Awake()
    {
        if (versionText) versionText.text = "v" + Application.version;
        foreach (var b in buttonBinds)
            b.Button.onClick.AddListener(new UnityAction(b.ClickEvent.Invoke));

        SettingsData settingsData = SaveSystem.Load("settings") as SettingsData;
        if (settingsData != null)
        {
            music.value = settingsData.Music;
            sound.value = settingsData.Sound;
            vibration.isOn = settingsData.Vibration;
        }

        if (statisticsText)
        {
            GameData gameData = SaveSystem.Load("gameData") as GameData;
            if (gameData != null)
            {
                statisticsText.text = $"Wins: {gameData.Wins}\n" +
                                      $"Loses: {gameData.Loses}";
            }
        }
    }

    private void Start()
    {
        Messenger.AddListener("Win", Win);
        Messenger.AddListener("Loose", Loose);
    }

    public void Print(string s) => Debug.Log(s);
    public void NextLevel(string scene) => SceneManager.LoadSceneAsync(scene);

    public void SwitchCanvas(int index) => StartCoroutine(Fade(index));
    private IEnumerator Fade(int index)
    {
        for (float t = 0; t < 1; t += 10 * Time.deltaTime)
        {
            foreach (var cg in canvasGroups)
            {
                if (index >= 0)
                    if (cg != canvasGroups[index])
                    {
                        if (cg.alpha >= 1 - t)
                            cg.alpha = 1 - t;
                    }
                    else cg.alpha = t;
                else if (cg.alpha >= 1 - t) cg.alpha = 1 - t;

                yield return null;
            }
        }

        foreach (var cg in canvasGroups)
        {
            if (index >= 0)
                if (cg != canvasGroups[index])
                {
                    cg.alpha = 0;
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
                else
                {
                    cg.alpha = 1;
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                }
            else
            {
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }
    }

    public void SaveSettings()
    {
        SettingsData data = new SettingsData(music.value, sound.value, vibration.isOn);
        SaveSystem.Save(data, "settings");
    }

    private void Win()
    {
        SwitchCanvas(victoryGroup);
        GameData gd = SaveSystem.Load("gameData") as GameData;
        if (gd == null) gd = new GameData();
        ++gd.Wins;
        SaveSystem.Save(gd, "gameData");
    }

    private void Loose()
    {
        SwitchCanvas(looseGroup);
        GameData gd = SaveSystem.Load("gameData") as GameData;
        if (gd == null) gd = new GameData();
        ++gd.Loses;
        SaveSystem.Save(gd, "gameData");
    }
}

[System.Serializable]
public class ButtonActionBind
{
    [HideInInspector] public string Name;
    public Button Button;
    public UnityEvent ClickEvent;
}