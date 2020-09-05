using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public string NEW_LINE = "\n";
    public GameObject statSet;

    public Image[] statPointUp;

    public Text levelText;
    public Text expText;

    public Text statPointText;
    public Text intellectText;
    public Text wisdomText;
    public Text dexterityText;
    public Text concentrationText;

    public Text powerTextText;
    public Text armorTextText;
    public Text accuracyText;
    public Text avoidText;
    public Text critRateText;
    public Text critDamText;

    public Text healthPointText;
    public Text manaPointText;

    public Text toolEffText;
    public Text skillEffText;
    public Text expEffText;

    public Text fameTextText;
    public Text charmTextText;

    public Animator buttonMenuAnimator;

    // 추후 delegate로 변경해야함
    public bool isDataChanged;

    public bool isUIOn;

    // Start is called before the first frame update
    void Start()
    {
        statSet.SetActive(false);
        isDataChanged = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            uiOnOff();
        }
    }

    private void FixedUpdate()
    {
        if (isDataChanged)
        {
            refresh();
        }
    }

    public void uiOnOff()
    {
        if (statSet.activeSelf)
        {
            statSet.SetActive(false);
        }
        else
        {
            if (GetComponent<InventoryUI>().inventorySet.activeSelf)
            {
                GetComponent<InventoryUI>().inventorySet.SetActive(false);
            }
            if (GetComponent<EquipmentUI>().equipmentSet.activeSelf)
            {
                GetComponent<EquipmentUI>().equipmentSet.SetActive(false);
            }

            statSet.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void setPlayerEquipedTool()
    {
        // 빈 아이템일 경우 Null로 처리
        try
        {
            if (GameManager.instance.playerEquipment.items[2] != null && GameManager.instance.playerEquipment.items[2].itemName.Length > 0)
            {
                GameManager.instance.isEquipedTool = true;
            }
            else
            {
                GameManager.instance.isEquipedTool = false;
            }

            int dummy = GameManager.instance.playerEquipment.items[2].itemName.Length;
        }
        catch
        {
            PlayerEquipment.instance.removeItem(EquipmentType.RightHand);
            GameManager.instance.playerEquipment.items[2] = null;
        }
    }

/*    private void updateQuickSlot()
    {
        for (int i = 0; i < GetComponent<ItemMenuSet>().quickSlot.Length; i++)
        {
            if (GetComponent<ItemMenuSet>().quickSlot[i].item != null && GetComponent<ItemMenuSet>().quickSlot[i].item.count > 1)
            {
                GetComponent<ItemMenuSet>().quickSlot[i].isDataChanged = true;
            }
        }
    }*/

    public void refresh()
    {
        isDataChanged = false;
        GameManager.instance.playerEquipment.updateTotalStats();
        GameManager.instance.playerData = Calculator.calcAll(GameManager.instance.playerData);

        setPlayerEquipedTool();

        levelText.text = "레벨 : " + GameManager.instance.playerData.level;
        statPointText.text = "남은 스탯포인트 : " + GameManager.instance.playerData.statPoint;
        expText.text = GameManager.instance.playerData.exp + " / " + GameManager.instance.playerData.nextExp;
        intellectText.text = "" + GameManager.instance.playerData.intellectPoint;
        wisdomText.text = "" + GameManager.instance.playerData.wisdomPoint;
        dexterityText.text = "" + GameManager.instance.playerData.dexterityPoint;
        concentrationText.text = "" + GameManager.instance.playerData.concentrationPoint;

        if (GameManager.instance.playerData.powerEquipment > 0)
        {
            powerTextText.text = "공격력 : " + GameManager.instance.playerData.power + NEW_LINE + " ( + " + GameManager.instance.playerData.powerEquipment + " )";
        }
        else if (GameManager.instance.playerData.powerEquipment < 0)
        {
            powerTextText.text = "공격력 : " + GameManager.instance.playerData.power + NEW_LINE + " ( " + GameManager.instance.playerData.powerEquipment + " )";
        }
        else
        {
            powerTextText.text = "공격력 : " + GameManager.instance.playerData.power;
        }

        if (GameManager.instance.playerData.armorEquipment > 0)
        {
            armorTextText.text = "방어력 : " + GameManager.instance.playerData.armor + NEW_LINE + " ( + " + GameManager.instance.playerData.armorEquipment + " )";
        }
        else if (GameManager.instance.playerData.armorEquipment < 0)
        {
            armorTextText.text = "방어력 : " + GameManager.instance.playerData.armor + NEW_LINE + " ( " + GameManager.instance.playerData.armorEquipment + " )";
        }
        else
        {
            armorTextText.text = "방어력 : " + GameManager.instance.playerData.armor;
        }

        if (GameManager.instance.playerData.accuracyEquipment > 0)
        {
            accuracyText.text = "명중률 : " + GameManager.instance.playerData.accuracy + NEW_LINE + " ( + " + GameManager.instance.playerData.accuracyEquipment + " )";
        }
        else if (GameManager.instance.playerData.accuracyEquipment < 0)
        {
            accuracyText.text = "명중률 : " + GameManager.instance.playerData.accuracy + NEW_LINE + " ( " + GameManager.instance.playerData.accuracyEquipment + " )";
        }
        else
        {
            accuracyText.text = "명중률 : " + GameManager.instance.playerData.accuracy;
        }

        if (GameManager.instance.playerData.avoidEquipment > 0)
        {
            avoidText.text = "회피율 : " + GameManager.instance.playerData.avoid + NEW_LINE + " ( + " + GameManager.instance.playerData.avoidEquipment + " )";
        }
        else if (GameManager.instance.playerData.avoidEquipment < 0)
        {
            avoidText.text = "회피율 : " + GameManager.instance.playerData.avoid + NEW_LINE + " ( " + GameManager.instance.playerData.avoidEquipment + " )";
        }
        else
        {
            avoidText.text = "회피율 : " + GameManager.instance.playerData.avoid;
        }

        if (GameManager.instance.playerData.critRateEquipment > 0)
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(GameManager.instance.playerData.critRate * 10) / 10 + "%"
                + NEW_LINE + " ( + " + Mathf.Round(GameManager.instance.playerData.critRateEquipment * 10) / 10 + "% )";
        }
        else if (GameManager.instance.playerData.critRateEquipment < 0)
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(GameManager.instance.playerData.critRate * 10) / 10 + "%"
                + NEW_LINE + " ( " + Mathf.Round(GameManager.instance.playerData.critRateEquipment * 10) / 10 + "% )";
        }
        else
        {
            critRateText.text = "치명 확률 : " + Mathf.Round(GameManager.instance.playerData.critRate * 10) / 10 + "%";
        }

        if (GameManager.instance.playerData.critDamEquipment > 0)
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(GameManager.instance.playerData.critDam * 10) / 10 + "%"
                + NEW_LINE + " ( + " + Mathf.Round(GameManager.instance.playerData.critDamEquipment * 10) / 10 + "% )";
        }
        else if (GameManager.instance.playerData.critDamEquipment < 0)
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(GameManager.instance.playerData.critDam * 10) / 10 + "%"
                + NEW_LINE + " ( " + Mathf.Round(GameManager.instance.playerData.critDamEquipment * 10) / 10 + "% )";
        }
        else
        {
            critDamText.text = "치명 피해 : " + Mathf.Round(GameManager.instance.playerData.critDam * 10) / 10 + "%";
        }

        healthPointText.text = "체력" + NEW_LINE + (int)GameManager.instance.playerData.healthPoint + " / " + GameManager.instance.playerData.healthPointMax;
        manaPointText.text = "마력" + NEW_LINE + (int)GameManager.instance.playerData.manaPoint + " / " + GameManager.instance.playerData.manaPointMax;

        expEffText.text = "경험치 보너스" + NEW_LINE + Mathf.Round(GameManager.instance.playerData.expEff * 10) / 10 + "%";

        fameTextText.text = "명성 : " + GameManager.instance.playerData.fame;
        charmTextText.text = "호감도 : " + GameManager.instance.playerData.charm;

        for (int i = 0; i < statPointUp.Length; i++)
        {
            if (GameManager.instance.playerData.statPoint > 0)
            {
                statPointUp[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                statPointUp[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    private bool pointUp()
    {
        if (GameManager.instance.playerData.statPoint < 1)
        {
            return false;
        }

        GameManager.instance.playerData.statPoint--;
        isDataChanged = true;
        GameManager.instance.isPointUp = true;

        return true;
    }

    public void pointUpIntellect()
    {
        if (pointUp())
        {
            GameManager.instance.playerData.intellectPoint++;
        }
    }

    public void pointUpWisdom()
    {
        if (pointUp())
        {
            GameManager.instance.playerData.wisdomPoint++;
        }
    }

    public void pointUpDexterity()
    {
        if (pointUp())
        {
            GameManager.instance.playerData.dexterityPoint++;
        }
    }

    public void pointUpConcentration()
    {
        if (pointUp())
        {
            GameManager.instance.playerData.concentrationPoint++;
        }
    }
}
