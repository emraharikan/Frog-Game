using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Panels")]
    public GameObject settingPanel;
    public GameObject mainMenuPanel;
    public GameObject levelSuccessPanel;
    public GameObject levelFaultPanel;

    [Header("Texts")]
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI currentLevelText;

    [Header("Setting Icons")]
    public GameObject vibrationOptionOpenedImage;
    public GameObject vibrationOptionClosedImage;
    public GameObject sfxOptionOpenedImage;
    public GameObject sfxOptionClosedImage;

    [Header("Bools")]
    bool isGearOpened = false;

    private GameManager gameManager;

    private void Awake()
    {
        MakeSingleton();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        moveText.text = gameManager.GameModel.MovementLimit.ToString();
        currentLevelText.text = gameManager.GameModel.CurrentLevel.ToString();
        mainMenuPanel.SetActive(true);
        levelSuccessPanel.SetActive(false);

        GameManager.OnMovementLimitChanged += SetMovementText;
        GameManager.OnLevelChanged += SetCurrentLevelText;
        UpdateVibrationIcon();
        UpdateSFXIcon();
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

    public void GearButtonPressed()
    {
        isGearOpened = !isGearOpened;
        settingPanel.SetActive(isGearOpened);
    }

    public void SFXOptionButtonPressed()
    {
        gameManager.ToggleSFX();
        UpdateSFXIcon();
    }

    public void VibrationOptionButtonPressed()
    {
        gameManager.ToggleVibration();
        UpdateVibrationIcon();
    }

    public void ContinueButtonPressed()
    {
        gameManager.NextButtonPressed();
    }

    public void SetMovementText(int movementLimit)
    {
        moveText.text = movementLimit.ToString();
    }

    public void SetCurrentLevelText(int level)
    {
        currentLevelText.text = "Level " + level.ToString();
    }

    void UpdateVibrationIcon()
    {
        bool isVibrationOn = gameManager.GameModel.IsVibrationOn;
        vibrationOptionOpenedImage.SetActive(isVibrationOn);
        vibrationOptionClosedImage.SetActive(!isVibrationOn);
    }

    void UpdateSFXIcon()
    {
        bool isSFXOn = gameManager.GameModel.IsSFXOn;
        sfxOptionOpenedImage.SetActive(isSFXOn);
        sfxOptionClosedImage.SetActive(!isSFXOn);
    }

    public void ShowLevelFailPanel()
    {
        levelFaultPanel.SetActive(true);
    }

    public void ShowLevelSuccessPanel()
    {
        levelSuccessPanel.SetActive(true);
    }
}