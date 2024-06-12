


public class GameModel
{
    public int MovementLimit { get; set; }
    public bool IsGameStarted { get; set; }
    public bool IsGameFinished { get; set; }
    public bool IsVibrationOn { get; set; }
    public bool IsSFXOn { get; set; }
    public int CurrentLevel { get; set; }
    public CameraSettings cameraSettings { get; set; }

    public GameModel()
    {
        MovementLimit = 6;
        IsGameStarted = false;
        IsGameFinished = false;
        IsVibrationOn = true;
        IsSFXOn = true;
        CurrentLevel = 1;
    }
}