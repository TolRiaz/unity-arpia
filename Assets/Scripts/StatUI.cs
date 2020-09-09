using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public string NEW_LINE = "\n";
    public GameObject statSet;

    public Image[] statPointUp;

    public Text nameText;
    public Text typeText;
    public Text levelText;
    public Text jobText;
    public Text money;
    public Text beneficenceScoreText;

    public Text hpText;
    public Text mpText;
    public Text expText;

    public Text powerText;
    public Text armorText;
    public Text magicPowerText;
    public Text magicArmorText;
    public Text critRateText;
    public Text critDamText;
    public Text accuracyText;
    public Text avoidText;

    public Text statPointText;
    public Text intellectText;
    public Text wisdomText;
    public Text dexterityText;
    public Text concentrationText;

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
        SoundManager.instance.playButtonEffectSound();

        if (statSet.activeSelf)
        {
            statSet.SetActive(false);
        }
        else
        {
            refresh();
            statSet.SetActive(true);
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

        levelText.text = "레 벨 : " + GameManager.instance.playerData.level;
        hpText.text = GameManager.instance.playerData.healthPoint + " / " + GameManager.instance.playerData.healthPointMax;
        mpText.text = GameManager.instance.playerData.manaPoint + " / " + GameManager.instance.playerData.manaPointMax;
        expText.text = GameManager.instance.playerData.exp + " / " + GameManager.instance.playerData.nextExp;
        powerText.text = "" + GameManager.instance.playerData.power;
        armorText.text = "" + GameManager.instance.playerData.armor;
        magicPowerText.text = "" + GameManager.instance.playerData.power;
        magicArmorText.text = "" + GameManager.instance.playerData.armor;
        accuracyText.text = "" + GameManager.instance.playerData.accuracy;
        avoidText.text = "" + GameManager.instance.playerData.avoid;
        critRateText.text = "" + Mathf.Round(GameManager.instance.playerData.critRate * 10) / 10 + "%";
        critDamText.text = "" + Mathf.Round(GameManager.instance.playerData.critDam * 10) / 10 + "%";

        /*        GameManager.instance.playerEquipment.updateTotalStats();
                GameManager.instance.playerData = Calculator.calcAll(GameManager.instance.playerData);

                setPlayerEquipedTool();

                levelText.text = "레벨 : " + GameManager.instance.playerData.level;
                statPointText.text = "남은 스탯포인트 : " + GameManager.instance.playerData.statPoint;
                expText.text = GameManager.instance.playerData.exp + " / " + GameManager.instance.playerData.nextExp;
                intellectText.text = "" + GameManager.instance.playerData.intellectPoint;
                wisdomText.text = "" + GameManager.instance.playerData.wisdomPoint;
                dexterityText.text = "" + GameManager.instance.playerData.dexterityPoint;
                concentrationText.text = "" + GameManager.instance.playerData.concentrationPoint;*/

        /*if (GameManager.instance.playerData.powerEquipment > 0)
        {
            powerText.text = "공격력 : " + GameManager.instance.playerData.power + NEW_LINE + " ( + " + GameManager.instance.playerData.powerEquipment + " )";
        }
        else if (GameManager.instance.playerData.powerEquipment < 0)
        {
            powerText.text = "공격력 : " + GameManager.instance.playerData.power + NEW_LINE + " ( " + GameManager.instance.playerData.powerEquipment + " )";
        }
        else
        {
            powerText.text = "공격력 : " + GameManager.instance.playerData.power;
        }

        if (GameManager.instance.playerData.armorEquipment > 0)
        {
            armorText.text = "방어력 : " + GameManager.instance.playerData.armor + NEW_LINE + " ( + " + GameManager.instance.playerData.armorEquipment + " )";
        }
        else if (GameManager.instance.playerData.armorEquipment < 0)
        {
            armorText.text = "방어력 : " + GameManager.instance.playerData.armor + NEW_LINE + " ( " + GameManager.instance.playerData.armorEquipment + " )";
        }
        else
        {
            armorText.text = "방어력 : " + GameManager.instance.playerData.armor;
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

        hpText.text = "체력" + NEW_LINE + (int)GameManager.instance.playerData.healthPoint + " / " + GameManager.instance.playerData.healthPointMax;
        mpText.text = "마력" + NEW_LINE + (int)GameManager.instance.playerData.manaPoint + " / " + GameManager.instance.playerData.manaPointMax;

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
        }*/
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
