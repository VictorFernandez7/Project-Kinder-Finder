using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_LoadingScreen : MonoBehaviour
{
    [Header("Paramaters")]
    [SerializeField] private float timeToLoad;

    private bool done;

    private void Update()
    {
        timeToLoad -= Time.deltaTime;

        if (timeToLoad <= 0 && !done)
        {
            Load();
            done = true;
        }
    }

    private void Load()
    {
        if (Scr_Levels.currentlyLoading == Scr_Levels.CurrentlyLoading.LoadingMenu)
        {
            switch (Scr_Levels.menuToLoad)
            {
                case Scr_Levels.MenuToLoad.MainMenu:
                    SceneManager.LoadSceneAsync(MenuStorage.mainMenu, LoadSceneMode.Additive);
                    break;
                case Scr_Levels.MenuToLoad.SystemSelection:
                    SceneManager.LoadSceneAsync(MenuStorage.systemSelection, LoadSceneMode.Additive);
                    break;
                case Scr_Levels.MenuToLoad.NarrativeScene:
                    SceneManager.LoadSceneAsync(MenuStorage.narrativeScene, LoadSceneMode.Additive);
                    break;
            }
        }

        else if (Scr_Levels.currentlyLoading == Scr_Levels.CurrentlyLoading.LoadingLevel)
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
        }

        SceneManager.sceneLoaded += FinishLoading;
    }

    private void FinishLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= FinishLoading;
        Destroy(this.gameObject);
    }
}