using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Scr_MenuManager : MonoBehaviour
{
    [Header("Menu Parameters")]
    [SerializeField] private float cameraSpeed;

    private GameObject mainCamera;

    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
    }

    private void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        mainCamera.transform.position += new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0) * Time.deltaTime * cameraSpeed;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}