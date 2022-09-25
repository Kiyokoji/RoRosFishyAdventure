using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator transition;

    public Level SceneToLoad = Level.Menu;

    public enum Level
    {
        Menu,
        Game,
        Level1,
        Level2
    };

    private void Start()
    {
        transition = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    public virtual void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneToLoad));
    }

    IEnumerator LoadLevel(Level scene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(scene.ToString());
    }
}
