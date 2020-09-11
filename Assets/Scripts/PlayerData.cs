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
}
