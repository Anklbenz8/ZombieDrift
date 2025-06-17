
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ProjectSettings", fileName = "ProjectSettings", order = 0)]
public class ProjectConfig : ScriptableObject {
    [SerializeField] private bool _isDebugEnable;
    [SerializeField] private bool _isFpsEnabled;
    [SerializeField] private int _targetFramerate = 60;
    [SerializeField] private RootCanvas _rootCanvasPrefab;
    [SerializeField] private FPSCounter _fpsCounterPrefab;
    [SerializeField] private GUILog _guiLogPrefab;

    public bool isDebugEnable => _isDebugEnable;

    public bool isFpsEnabled => _isFpsEnabled;

    public int targetFramerate => _targetFramerate;
    public RootCanvas rootCanvasPrefab => _rootCanvasPrefab;
    public FPSCounter fpsCounterPrefab => _fpsCounterPrefab;
    public GUILog guiLogPrefab => _guiLogPrefab;
}