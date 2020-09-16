using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : MonoBehaviour
{
    public float cooldown;
    public float castingTimer;
    public bool isCasting;
    public Action action;

    public float playerX;
    public float playerY;
    public int questId;
    public int questActionIndex;
    public string entityName;
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
    public int magicPower;
    public int magicArmor;
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

    public float expStack;
    public int sortingIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setBattleEntityData(EntityData entityData)
    {
        this.entityName = entityData.entityName;
        this.job = entityData.job;
        this.element = entityData.element;
        this.level = entityData.level;
        this.exp = entityData.exp;
        this.nextExp = entityData.nextExp;
        this.money = entityData.money;
        this.gold = entityData.gold;
        this.items = entityData.items;
        this.statPoint = entityData.statPoint;
        this.intellectPoint = entityData.intellectPoint;
        this.wisdomPoint = entityData.wisdomPoint;
        this.dexterityPoint = entityData.dexterityPoint;
        this.concentrationPoint = entityData.concentrationPoint;

        this.power = entityData.power;
        this.armor = entityData.armor;
        this.accuracy = entityData.accuracy;
        this.avoid = entityData.avoid;
        this.critRate = entityData.critRate;
        this.critDam = entityData.critDam;

        this.healthPoint = entityData.healthPoint;
        this.healthPointMax = entityData.healthPointMax;
        this.manaPoint = entityData.manaPoint;
        this.manaPointMax = entityData.manaPointMax;
        this.fame = entityData.fame;
        this.charm = entityData.charm;
        this.expStack = entityData.expStack;
        this.sortingIndex = entityData.sortingIndex;
    }

    public void setBattleEntityData(PlayerData playerData)
    {
        this.playerX = playerData.playerX;
        this.playerY = playerData.playerY;
        this.questId = playerData.questId;
        this.questActionIndex = playerData.questActionIndex;
        this.entityName = playerData.name;
        this.job = playerData.job;
        this.element = playerData.element;
        this.level = playerData.level;
        this.exp = playerData.exp;
        this.nextExp = playerData.nextExp;
        this.money = playerData.money;
        this.gold = playerData.gold;
        this.inventorySize = playerData.inventorySize;
        //this.questInformation = playerData.questInformation;
        this.items = playerData.items;
        //this.Item[] equipments = playerData.equipments;

        //Quest
        this.startQuest = playerData.startQuest;
        this.currentQuest = playerData.currentQuest;
        this.clearQuest = playerData.clearQuest;

        //Stats
        this.statPoint = playerData.statPoint;
        this.intellectPoint = playerData.intellectPoint;
        this.wisdomPoint = playerData.wisdomPoint;
        this.dexterityPoint = playerData.dexterityPoint;
        this.concentrationPoint = playerData.concentrationPoint;

        this.power = playerData.power;
        this.armor = playerData.armor;
        this.magicPower = playerData.magicPower;
        this.magicArmor = playerData.magicArmor;
        this.accuracy = playerData.accuracy;
        this.avoid = playerData.avoid;
        this.critRate = playerData.critRate;
        this.critDam = playerData.critDam;

        this.healthPoint = playerData.healthPoint;
        this.healthPointMax = playerData.healthPointMax;
        this.manaPoint = playerData.manaPoint;
        this.manaPointMax = playerData.manaPointMax;

        this.expEff = playerData.expEff;

        this.fame = playerData.fame;
        this.charm = playerData.charm;
        this.weight = playerData.weight;
        this.weightMax = playerData.weightMax;

        this.expStack = playerData.expStack;
        this.sortingIndex = playerData.sortingIndex;
    }
}

public enum Action
{
    NONE,
    ATTACK,
    MAGIC,
    USEITEM,
    SKIP,
    RUNAWAY
}