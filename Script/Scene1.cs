using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("MoriagariDemo");
    }
}
