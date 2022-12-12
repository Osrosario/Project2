using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//added to load scenes
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    //a new variable that you can change in the inspector
    public string nextLevel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //a public method that will run when you click the button
    public void ButtonToChangeLevel()
    {

        //loads the scene written in the inspector
        SceneManager.LoadScene(nextLevel);

    }
}
