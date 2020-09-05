using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAlphaController : MonoBehaviour
{
    //private DialogManager dialogManager;
    private Image[] buttonImages;

    // Start is called before the first frame update
    void Start()
    {
        //dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
        buttonImages = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        controlAlphaOnTalk();
    }

    public void controlAlphaOnTalk()
    {
        if (true)
        {
            foreach (Image buttonImage in buttonImages)
            {
                buttonImage.color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            foreach (Image buttonImage in buttonImages)
            {
                buttonImage.color = new Color(1, 1, 1, 0.55f);
            }
        }
    }
}
