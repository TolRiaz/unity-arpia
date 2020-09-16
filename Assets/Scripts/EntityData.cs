using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityData
{
    public string entityName;
    public int code;

    public Job job;
    public Element element;
    public int level;
    public int exp = 5;
    public int nextExp;
    public int money;
    public int gold;
    public List<Item> items;

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

    public int fame;
    public int charm;

    public float expStack;
    public int sortingIndex;

    public Sprite sprite;
    public string spritePath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public EntityData()
    {

    }

    public EntityData(string name, int code, Job job, Element element, int level, int exp, int nextExp, int money, int gold,
        List<Item> items, int statPoint, int intellectPoint, int wisdomPoint, int dexterityPoint, int concentrationPoint, 
        int power, int armor, int magicPower, int magicArmor, int accuracy, int avoid, float critRate, float critDam,
        float healthPoint, float healthPointMax, float manaPoint, float manaPointMax, string spritePath = null,
        int fame = 0, int charm = 0, float expStack = 0, int sortingIndex = 3)
    {
        this.entityName = name;
        this.job = job;
        this.element = element;
        this.level = level;
        this.exp = exp;
        this.nextExp = nextExp;
        this.money = money;
        this.gold = gold;
        this.items = items;
        this.statPoint = statPoint;
        this.intellectPoint = intellectPoint;
        this.wisdomPoint = wisdomPoint;
        this.dexterityPoint = dexterityPoint;
        this.concentrationPoint = concentrationPoint;

        this.power = power;
        this.armor = armor;
        this.magicPower = magicPower;
        this.magicArmor = magicArmor;
        this.accuracy = accuracy;
        this.avoid = avoid;
        this.critRate = critRate;
        this.critDam = critDam;

        this.healthPoint = healthPoint;
        this.healthPointMax = healthPointMax;
        this.manaPoint = manaPoint;
        this.manaPointMax = manaPointMax;
        this.fame = fame;
        this.charm = charm;
        this.expStack = expStack;
        this.sortingIndex = sortingIndex;
        this.spritePath = spritePath;

        this.spritePath = spritePath;

        sprite = loadSprite(spritePath);
    }

    public void setEntityData(EntityData entityData)
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
        this.magicPower = entityData.magicPower;
        this.magicArmor = entityData.magicArmor;
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
        this.spritePath = entityData.spritePath;

        this.spritePath = entityData.spritePath;
        sprite = loadSprite(spritePath);
    }

    [ContextMenu("From Json Data")]
    public Sprite loadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }
}
