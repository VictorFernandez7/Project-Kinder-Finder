using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_DevTools : MonoBehaviour
{
    [Header("Bullet Time")]
    [Range(0, 1)] [SerializeField] private float slowAmount;
    [SerializeField] private KeyCode BT_key;

    [Header("Reload Scene")]
    [SerializeField] private KeyCode RS_key;

    [Header("Active Solid Tool")]
    [SerializeField] private KeyCode ST_key;

    [Header("Active Liquid Tool")]
    [SerializeField] private KeyCode LT_key;

    [Header("Active Gas Tool")]
    [SerializeField] private KeyCode GT_key;

    [Header("References")]
    [SerializeField] public GameObject solidTool;
    [SerializeField] public GameObject liquidTool;
    [SerializeField] public GameObject gasTool;

    private GameObject playerShip;
    private bool devSlow; 

    private void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

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

        if (Input.GetKeyDown(ST_key))
            solidTool.SetActive(!solidTool.activeInHierarchy);

        if (Input.GetKeyDown(LT_key))
            liquidTool.SetActive(!liquidTool.activeInHierarchy);

        if (Input.GetKeyDown(GT_key))
            gasTool.SetActive(!gasTool.activeInHierarchy);
    }

    private void BulletTime(bool active)
    {
        if (active)
        {
            Time.timeScale = slowAmount;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = false;
            devSlow = true;
        }

        else if(devSlow == true)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += 0.25f * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 1;
                playerShip.GetComponent<Scr_PlayerShipPrediction>().enabled = true;
                devSlow = false;
            }
        }
    }
}