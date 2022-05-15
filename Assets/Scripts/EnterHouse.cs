using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{   

    // Update is called once per frame
    public string nextsence;
    void Update()
    {
        enterScence(nextsence);
    }

    void enterScence(string nextsence)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(nextsence);
    }
    }

}
