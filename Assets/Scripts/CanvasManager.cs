using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject loading_scene;
    public float timer;
    public GameObject statusSet;

    // Test
    public Text positionText;

    // Floor
    public GameObject villageCollier1F;
    public GameObject villageCollier2F;

    public void changeColliderOnOff(Map map, int layerIndex)
    {
        switch (map)
        {
            case Map.VILLAGE:
                switch (layerIndex)
                {
                    case 1:
                        villageCollier1F.SetActive(true);
                        villageCollier2F.SetActive(false);
                        break;
                    case 3:
                        villageCollier1F.SetActive(false);
                        villageCollier2F.SetActive(true);
                        break;
                }
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //positionText.text = ("터치한좌표는 \n" + "x = " + Input.GetTouch(0).position.x + ", y = " + Input.GetTouch(0).position.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        loading_scene.SetActive(true);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (loading_scene.activeSelf && timer > 3f)
        {
            loading_scene.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        statusRefresh();
    }

    private void statusRefresh()
    {
        statusSet.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "" + GameManager.instance.playerData.name;
        statusSet.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = "" + GameManager.instance.playerData.getJobName();
        statusSet.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "" + GameManager.instance.playerData.level;

        statusSet.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = 
            GameManager.instance.playerData.healthPoint + " / " + GameManager.instance.playerData.healthPointMax;
        statusSet.transform.GetChild(4).GetChild(0).GetComponent<Text>().text =
            GameManager.instance.playerData.manaPoint + " / " + GameManager.instance.playerData.manaPointMax;
        statusSet.transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            GameManager.instance.playerData.exp + " / " + GameManager.instance.playerData.nextExp;

        statusSet.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = 
            GameManager.instance.playerData.healthPoint / GameManager.instance.playerData.healthPointMax;
        statusSet.transform.GetChild(4).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount =
            GameManager.instance.playerData.manaPoint / GameManager.instance.playerData.manaPointMax;
        statusSet.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount =
            (float)GameManager.instance.playerData.exp / (float)GameManager.instance.playerData.nextExp;
    }

    public void offUI()
    {
        MouseMovement.instance.isMoving = true;
        SoundManager.instance.playButtonEffectSound();

        if (GetComponent<StatUI>().statSet.activeSelf)
        {
            GetComponent<StatUI>().statSet.SetActive(false);
        }
        if (GetComponent<InventoryUI>().inventorySet.activeSelf)
        {
            GetComponent<InventoryUI>().inventorySet.SetActive(false);
        }
        if (GetComponent<QuestUI>().questSet.activeSelf)
        {
            GetComponent<QuestUI>().questSet.SetActive(false);
        }
        if (GetComponent<SkillUI>().skillSet.activeSelf)
        {
            GetComponent<SkillUI>().skillSet.SetActive(false);
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
