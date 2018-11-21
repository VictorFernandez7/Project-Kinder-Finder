using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_DevTools : MonoBehaviour
{
    [Header("BulletTime")]
    [Range(0, 1)] [SerializeField] private float slowAmount;
    [SerializeField] private KeyCode BT_key;

    [Header("ReloadScene")]
    [SerializeField] private KeyCode RS_key;

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Input.GetKey(BT_key))
            BulletTime(true);

        else
            BulletTime(false);

        if (Input.GetKeyDown(RS_key))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BulletTime(bool active)
    {
        if (active)
        {
            Time.timeScale = slowAmount;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        else
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += 0.25f * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
}