using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public List<LevelCameraSetting> levelCameraSettings;
}

[System.Serializable]
public class LevelCameraSetting
{
    public int level;
    public float orthographicSize;
}