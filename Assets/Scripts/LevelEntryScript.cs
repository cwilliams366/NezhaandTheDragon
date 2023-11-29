using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntryScript : MonoBehaviour
{
    [Header("Scene Data Info")]
    public int sceneIndex;
    public SceneAsset loadedScene;
    public GameObject transitionAnim;
    public string spawnPointName;
    private Canvas canvas;
    private Animator animator;
    private void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();
        if (!loadedScene)
        {
            throw new MissingReferenceException(name + " has no reference to the variable \"loadedScene\"");
        }

        if (!transitionAnim)
        {
            throw new MissingReferenceException(name + " has no reference to the variable \"transitionAnim\"");
        }

    }

    private void Update()
    {
        if (transitionAnim)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                LevelEventsScript.levelExit.Invoke(loadedScene, spawnPointName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision);
          

            animator = Instantiate(transitionAnim, canvas.transform).GetComponent<Animator>();
        }
    }
}
