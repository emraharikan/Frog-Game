using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject[] LevelPrefabs;

    public float activeFrog;

    private void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        int currentLevel = GameManager.instance.GameModel.CurrentLevel;
        if (currentLevel - 1 < LevelPrefabs.Length)
        {
            Instantiate(LevelPrefabs[currentLevel - 1], Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Level index out of range!");
        }
    }

    private void Update()
    {
        activeFrog = GameObject.FindGameObjectsWithTag("Frog").Length;

        if (activeFrog < 1)
        {
            GameManager.instance.TriggerLevelSuccess();
        }
 
    }

    public void ContinueButton()
    {
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        GameManager.instance.IncreaseLevel(); 
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}