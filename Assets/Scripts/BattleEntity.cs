using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEntity : MonoBehaviour
{
    public Animator arrowUI;
    public Animator animator;

    public int transformIndex;
    public float cooldown;
    public float castingTimer;
    public bool isCasting;
    public Action action;
    public GameObject target;
    public Skill skill;
    public List<GameObject> targets;
    public bool isDead;

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

    // HealthBar text
    public int damagedTimer;
    public GameObject healthBarBackground;
    public Image healthBar;
    public float delayHP;

    // Damage text
    public GameObject hudDamageText;
    public Transform hudPos;

    // Skill
    public List<Skill> skills;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = 1.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthPoint / healthPointMax, Time.deltaTime * 3f);
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

        delayHP = healthPoint;
        healthBar.fillAmount = healthPoint / healthPointMax;
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

        this.skills = playerData.skills;

        delayHP = healthPoint;
        healthBar.fillAmount = healthPoint / healthPointMax;
    }

    public void sendThisGameObejct()
    {
        BattleManager.instance.selectedObject = gameObject;
        BattleManager.instance.selectTargetMode();
    }

    public void takeDamage(BattleEntity battleEntity, Action action)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;

        if (action == Action.ATTACK)
        {
            int avoidValue = avoid - battleEntity.accuracy;
            int damage = -((armor - battleEntity.power * Random.Range(600, 1010)) / 1000);

            if (damage <= 0)
            {
                hudText.GetComponent<DamageText>().damage = 0;
                return;
            }

            if (avoidValue > 0)
            {
                if (Random.Range(0, 30) < avoidValue)
                {
                    hudText.GetComponent<DamageText>().damage = 0;
                    return;
                }
            }

            GameObject.Find("Main Camera").GetComponent<MainCamera>().setCameraShake();

            try
            {
                // animator.SetTrigger("doDamaged");
                animator.SetTrigger("doCriticalDamaged");
            }
            catch (System.Exception)
            {
                Debug.Log("등록되지 않은 동작입니다.");
            }

            summonEffect(battleEntity);

            healthPoint -= damage;
            damagedTimer = 30;
            //isDamaged = true;
            //Debug.Log("데미지 : " + damage + "  남은체력 : " + healthPoint);
            //healthBar.fillAmount = healthPoint / healthPointMax;
            healthBarBackground.SetActive(true);

            hudText.GetComponent<DamageText>().damage = damage;

            if (damage > 0 && isCasting)
            {
                BattleManager.instance.cancleCasting(transformIndex);
            }
        }
        else if (action == Action.MAGIC)
        {
            int avoidValue = avoid - battleEntity.accuracy;
            int damage = -((armor - (int) (battleEntity.skill.magicPower * Random.Range(600, 1010)) / 1000));
            battleEntity.manaPoint -= battleEntity.skill.costMP;

            if (damage <= 0)
            {
                hudText.GetComponent<DamageText>().damage = 0;
                return;
            }

            if (avoidValue > 0)
            {
                if (Random.Range(0, 30) < avoidValue)
                {
                    hudText.GetComponent<DamageText>().damage = 0;
                    return;
                }
            }

            GameObject.Find("Main Camera").GetComponent<MainCamera>().setCameraShake();
            
            try
            {
                 animator.SetTrigger("doDamaged");
                // animator.SetTrigger("doCriticalDamaged");
            }
            catch (System.Exception)
            {
                Debug.Log("등록되지 않은 동작입니다.");
            }

            summonEffect(battleEntity);

            damage = (int) Mathf.Round(damage * getElementResult(battleEntity.skill, this));
            healthPoint -= damage;
            damagedTimer = 30;
            // healthBar.fillAmount = healthPoint / healthPointMax;
            healthBarBackground.SetActive(true);

            hudText.GetComponent<DamageText>().damage = damage;

            if (damage > 0 && isCasting)
            {
                Debug.Log("캐스팅 취소");
                BattleManager.instance.cancleCasting(transformIndex);
            }
        }

        if (healthPoint <= 0 && !isDead)
        {
            battleEntity.expStack += exp;

            BattleManager.instance.dealDeath(transformIndex);
        }

        // 뒤집기
        // healthBar.transform.localScale = transform.localScale;
    }

    public float getElementResult(Skill skill, BattleEntity entityData)
    {
        switch (skill.element)
        {
            case Element.FIRE:
                if (entityData.element == Element.ICE)
                {
                    return 1.25f;
                }
                else if (entityData.element == Element.EARTH)
                {
                    return 0.75f;
                }
                break;

            case Element.ICE:
                if (entityData.element == Element.EARTH)
                {
                    return 1.25f;
                }
                else if (entityData.element == Element.FIRE)
                {
                    return 0.75f;
                }
                break;

            case Element.EARTH:
                if (entityData.element == Element.FIRE)
                {
                    return 1.25f;
                }
                else if (entityData.element == Element.ICE)
                {
                    return 0.75f;
                }
                break;
        }

        return 1f;
    }

    private void summonEffect(BattleEntity battleEntity)
    {
        // 임시 마법 이펙트
        switch (battleEntity.skill.skillId)
        {
            case 0:
                EffectManager.instance.createEffect(transform.position, gameObject, 4);
                SoundManager.instance.playEffectSound(9);
                break;
            case 100:
                EffectManager.instance.createEffect(transform.position, gameObject, 3);
                SoundManager.instance.playEffectSound(7);
                break;
            case 103:
                EffectManager.instance.createEffect(transform.position, gameObject, 5);
                SoundManager.instance.playEffectSound(7);
                break;
            default:
                EffectManager.instance.createEffect(transform.position, gameObject, 6);
                SoundManager.instance.playEffectSound(12);
                break;
        }
    }

    public Skill findSkillById(int id)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].skillId == id)
            {
                return skills[i];
            }
        }

        return null;
    }

    public void getTransformIndex()
    {

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