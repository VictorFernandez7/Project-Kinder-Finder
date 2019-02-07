using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_LoadingScreen : MonoBehaviour
{
    private void OnEnable()
    {
        switch (Scr_Levels.levelToLoad)
        {
            case Scr_Levels.LevelToLoad.PlanetSystem1:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem1, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem2:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem2, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem3:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem3, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem4:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem4, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem5:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem5, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem6:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem6, LoadSceneMode.Additive);
                break;
            case Scr_Levels.LevelToLoad.PlanetSystem7:
                SceneManager.LoadSceneAsync(LevelStorage.planetSystem7, LoadSceneMode.Additive);
                break;
        }

        SceneManager.sceneLoaded += FinishLoading;
    }

    private void FinishLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= FinishLoading;
        Destroy(this.gameObject);
    }
}