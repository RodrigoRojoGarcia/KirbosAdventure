using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;

    private static void InitSingleton(GameManager thisInstance)
    {
        if (instance != null && instance != thisInstance)
        {
            throw new System.Exception("Hay al menos dos instancias de " + thisInstance.GetType().Name);
        }
        else
        {
            instance = thisInstance;
        }
    }
    #endregion


    public PlayerController kirbo;

    public KrackoController kracko;


    private void Awake()
    {
        InitSingleton(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(kirbo.getHealth() <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if(kracko.getHealth() <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
