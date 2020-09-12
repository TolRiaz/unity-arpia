using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerX;
    public float playerY;
    public int questId;
    public int questActionIndex;
    public string name;
    public Job job;
    public Element element;
    public int level;
    public int exp = 5;
    public int nextExp;
    public int money;
    public int gold;
    public int inventorySize;
    //public List<QuestInformation> questInformation;
    public List<Item> items;
    //public Item[] equipments;

    //Quest
    public List<int> startQuest;
    public List<Quest> currentQuest;
    public List<int> clearQuest;

    //Stats
    public int statPoint;
    public int intellectPoint;
    public int wisdomPoint;
    public int dexterityPoint;
    public int concentrationPoint;

    public int power;
    public int armor;
    public int accuracy;
    public int avoid;
    public float critRate;
    public float critDam;

    public float healthPoint;
    public float healthPointMax;
    public float manaPoint;
    public float manaPointMax;

    public float expEff;

    public int fame;
    public int charm;
    public float weight;
    public float weightMax;

    //Equipments
/*    public int reinforce;

    public int powerEquipment;
    public int armorEquipment;
    public int accuracyEquipment;
    public int avoidEquipment;
    public float critRateEquipment;
    public float critDamEquipment;

    public float healthPointEquipment;
    public float manaPointEquipment;

    public float intellectEquipment;
    public float wisdomEquipment;
    public float dexterityEquipment;
    public float concentrationEquipment;

    public float expEffEquipment;*/

    public string getJobName()
    {
        switch (job)
        {
            case Job.APPRENTICE:
                return "견습 마법사";
            case Job.BEGINNER:
                return "초보 마법사";
            case Job.EXPERT:
                return "숙련 마법사";
            case Job.MAGE:
                return "마도사";
            case Job.GREATMAGE:
                return "대마도사";
            case Job.SAGE:
                return "현자";
            case Job.GREATSAGE:
                return "대현자";
            case Job.SUPERMAGE:
                return "초대마도사 (" + getElementName() + ")";
            default :
                return "무직";
        }
    }

    public string getElementName()
    {
        switch (element)
        {
            case Element.FIRE:
                return "화염";
            case Element.ICE:
                return "얼음";
            case Element.EARTH:
                return "대지";
            case Element.THUNDER:
                return "번개";
            case Element.WIND:
                return "바람";
            case Element.LIGHTNESS:
                return "빛";
            case Element.DARKNESS:
                return "어둠";
            default:
                return "무속";
        }
    }
}

[System.Serializable]
public enum Job
{
    NONE,
    APPRENTICE,
    BEGINNER,
    EXPERT,
    MAGE,
    GREATMAGE,
    SAGE,
    GREATSAGE,
    SUPERMAGE
}

[System.Serializable]
public enum Element
{
    FIRE,
    ICE,
    EARTH,
    THUNDER,
    WIND,
    LIGHTNESS,
    DARKNESS
}