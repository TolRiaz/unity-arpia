using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public GameObject skillSet;
    public Animator buttonMenuAnimator;

    public SkillInformationUI infoSet;
    public Image skillImage;

    // Start is called before the first frame update
    void Start()
    {
        skillSet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void uiOnOff()
    {
        if (skillSet.activeSelf)
        {
            skillSet.SetActive(false);
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

            skillSet.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void informationUIOnOff()
    {
        if (infoSet.gameObject.activeSelf)
        {
            infoSet.gameObject.SetActive(false);
        }
        else
        {
            infoSet.gameObject.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);
        }
    }

    public void informationUIOnOff(Skill skill, Image skillImage)
    {
        if (infoSet.gameObject.activeSelf)
        {
            infoSet.gameObject.SetActive(false);
        }
        else
        {
            infoSet.gameObject.SetActive(true);
            buttonMenuAnimator.SetBool("isUIOn", true);

            infoSet.skillName.text = skill.skillName;
            infoSet.skillLevel.text = "기술 레벨 : " + GameManager.instance.playerSkillData.getSkill(skill.skillId).level + " / " + skill.level;
            infoSet.skillExp.text = "숙련도 : " + GameManager.instance.playerSkillData.getSkill(skill.skillId).experience + " / " + skill.calculateMastery(skill.level);
            infoSet.content.text = skill.information;
            infoSet.skillImage.sprite = skillImage.sprite;
        }
    }
}