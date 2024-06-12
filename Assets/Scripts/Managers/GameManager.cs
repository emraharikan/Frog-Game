using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lofelt.NiceVibrations;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameModel GameModel { get; private set; }

    private Camera mainCamera;
    public CameraSettings cameraSettings;
    public bool isInGame;
    public static event Action<int> OnMovementLimitChanged;
    public static event Action<int> OnLevelChanged;

    private void Awake()
    {
        GameModel = new GameModel();
        GameModel.cameraSettings = cameraSettings;
        LoadPreferences();
        MakeSingleton();
        UpdateCameraSize();
    }

    public void MakeSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void LoadPreferences()
    {
        if (PlayerPrefs.HasKey("VibrationOff"))
        {
            GameModel.IsVibrationOn = PlayerPrefs.GetInt("VibrationOff") == 0;
        }

        if (PlayerPrefs.HasKey("SFXOff"))
        {
            GameModel.IsSFXOn = PlayerPrefs.GetInt("SFXOff") == 0;
        }

        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            GameModel.CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            GameModel.CurrentLevel = 1;
        }

        UpdateMovementLimit();
    }

    private void SavePreferences()
    {
        PlayerPrefs.SetInt("VibrationOff", GameModel.IsVibrationOn ? 0 : 1);
        PlayerPrefs.SetInt("SFXOff", GameModel.IsSFXOn ? 0 : 1);
        PlayerPrefs.SetInt("CurrentLevel", GameModel.CurrentLevel);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SavePreferences();
    }

    public void DecreaseMovement(int amount)
    {
        if (GameModel.MovementLimit > 0)
        {
            GameModel.MovementLimit -= amount;
            OnMovementLimitChanged?.Invoke(GameModel.MovementLimit);
        }
        else
        {
            TriggerLevelFailed();
        }
    }

    public void IncreaseLevel()
    {
        GameModel.CurrentLevel++;
        UpdateMovementLimit();
        OnLevelChanged?.Invoke(GameModel.CurrentLevel);
        SavePreferences();
    }

    private void UpdateMovementLimit()
    {
        GameModel.MovementLimit = 6 + GameModel.CurrentLevel * 1; // Seviye başına 1 movement artırılıyor
    }

    public void TriggerHaptic()
    {
        if (GameModel.IsVibrationOn)
        {
            HapticController.fallbackPreset = HapticPatterns.PresetType.Warning;
            HapticPatterns.PlayEmphasis(0.85f, 0.05f);
        }
    }

    public void TriggerLevelFailed()
    {
        GameModel.IsGameFinished = true;
        UIManager.instance.ShowLevelFailPanel();
    }

    public void TriggerLevelStart()
    {
        isInGame = true;
    }

    public void TriggerLevelSuccess()
    {
        GameModel.IsGameFinished = true;
        UIManager.instance.ShowLevelSuccessPanel();
        TriggerHaptic();
    }

    public void NextButtonPressed()
    {
        IncreaseLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetryButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleVibration()
    {
        GameModel.IsVibrationOn = !GameModel.IsVibrationOn;
        PlayerPrefs.SetInt("VibrationOff", GameModel.IsVibrationOn ? 0 : 1);
    }

    public void ToggleSFX()
    {
        GameModel.IsSFXOn = !GameModel.IsSFXOn;
        PlayerPrefs.SetInt("SFXOff", GameModel.IsSFXOn ? 0 : 1);
    }

    public void UpdateCameraSize()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        foreach (var setting in cameraSettings.levelCameraSettings)
        {
            if (setting.level == GameModel.CurrentLevel)
            {
                mainCamera.orthographicSize = setting.orthographicSize;
                break;
            }
        }
    }
}