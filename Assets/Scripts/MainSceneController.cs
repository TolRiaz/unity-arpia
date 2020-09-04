using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public GameObject main_scene;
    public GameObject book_scene;

    private Button _game_start_button;

    public int type;
    public GameObject[] character_type;

    // Start is called before the first frame update
    void Start()
    {
        main_scene.SetActive(true);
        book_scene.SetActive(false);

        _game_start_button = main_scene.transform.GetChild(0).gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoStartGame()
    {
        main_scene.SetActive(false);
        book_scene.SetActive(true);
    }

    public void MovePreviousBook()
    {
        main_scene.SetActive(true);
        book_scene.SetActive(false);
    }

    public void MoveNextBook()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectType(int num)
    {
        OffType();

        switch (num)
        {
            case 0:
                type = 0;
                character_type[0].SetActive(true);
                break;
            case 1:
                type = 1;
                character_type[1].SetActive(true);
                break;
            case 2:
                type = 2;
                character_type[2].SetActive(true);
                break;
            case 3:
                type = 0;
                character_type[3].SetActive(true);
                break;
            case 4:
                type = 4;
                character_type[4].SetActive(true);
                break;
            case 5:
                type = 5;
                character_type[5].SetActive(true);
                break;
        }
    }

    private void OffType()
    {
        for (int i = 0; i < character_type.Length; i++)
        {
            character_type[i].SetActive(false);
        }
    }
}
