using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuChunk : MonoBehaviour
{
    public string versionTxt = "Version: ";
    public float version = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Version_Txt").GetComponent<Text>().text = versionTxt + version.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
        }
    }
}
