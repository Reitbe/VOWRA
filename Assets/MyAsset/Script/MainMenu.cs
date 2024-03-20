using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenu, MVMenu, CfMenu, EdMenu, Settings;
    List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameObjects.Add(mainMenu);
        gameObjects.Add(MVMenu);
        gameObjects.Add(CfMenu);
        gameObjects.Add(EdMenu);
        gameObjects.Add(Settings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show(GameObject obj)
    {
        foreach (GameObject scene in gameObjects)
            scene.SetActive(false);
        obj.SetActive(true);

    }

    public void showMain(GameObject obj)
    {
        obj.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void enterViewerMode(int index)
    {
        //JsonUtility
        //PlayerPrefs
        PlayerPrefs.SetInt("Model", index);
        SceneManager.LoadScene("ObjectViewer");

    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    
}
