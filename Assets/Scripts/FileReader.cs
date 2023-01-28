using UnityEngine;

public class FileReader : MonoBehaviour
{
    [SerializeField] private TextAsset _levelSettings;
    
    public LevelSettings GetLevelSettings()
    {
        var levelSettings = new LevelSettings();
        levelSettings = JsonUtility.FromJson<LevelSettings>(_levelSettings.text);
        
        return levelSettings;
    }
}