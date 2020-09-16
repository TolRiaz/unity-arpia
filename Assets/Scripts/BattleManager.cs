using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<GameObject> teams = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> actionMenu = new List<GameObject>();
    public int fieldType;
    public int teamCount = 4;
    public int enemyCount = 4;

    // battle UI, Arrow
    public GameObject battleSet;
    private GameObject castingBar;
    public List<GameObject> teamArrows = new List<GameObject>();
    public List<GameObject> enemyArrows = new List<GameObject>();

    // Battle casting bar position
    private GameObject chargingPivot;
    private Vector2 cooldownPosition;
    private Vector2 castingPosition;
    private Vector2 actionPosition;
    private float teamPivot = 140;
    private float enemyPivot = -120;

    // cooldown
    public bool isBattle;
    private float term = 50;
    public float[] cooldownTeam;
    public float[] cooldownEnemy;
    public bool isReachedCasting;

    public float timer;

    //public GameObject mobPrefab;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        cooldownTeam = new float[4];
        cooldownEnemy = new float[4];

        // mob, team gameObject register
        for (int i = 0; i < 4; i++)
        {
            // team 과 enemy 의 객체수가 다를경우 기능 분리해야함!
            teams[i] = transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
            enemies[i] = transform.GetChild(0).GetChild(1).GetChild(i).gameObject;
            actionMenu[i] = transform.GetChild(0).GetChild(0).GetChild(i + 4).gameObject;
            actionMenu[i].SetActive(false);

            cooldownTeam[i] = 200 / term;
        }

        // casting arrow register
        for (int i = 0; i < 4; i++)
        {
            teamArrows[i] = battleSet.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject;
            enemyArrows[i] = battleSet.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).gameObject;

            cooldownEnemy[i] = 120 / term;
        }

        cooldownEnemy[0] = 150 / term;
        cooldownEnemy[1] = 100 / term;

        chargingPivot = battleSet.transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
        cooldownPosition = chargingPivot.transform.position;
        castingPosition = new Vector2(cooldownPosition.x + 600, cooldownPosition.y);
        actionPosition = new Vector2(cooldownPosition.x + 900, cooldownPosition.y);

        castingBar = battleSet.transform.Find("CastingBar").gameObject;

        battleSet.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBattle)
        {
            dealAction();
            dealCooldown();
        }
    }
    
    public void dealAction()
    {
        for (int i = 0; i < teamCount; i++)
        {
            if (teams[i].GetComponent<BattleEntity>().isCasting && teamArrows[i].transform.position.x >= actionPosition.x)
            {
                if (teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    // TODO 공격 처리
                    teams[i].GetComponent<BattleEntity>().isCasting = false;
                    teams[i].GetComponent<BattleEntity>().action = Action.NONE;
                    teamArrows[i].transform.position = new Vector2(cooldownPosition.x, teamArrows[i].transform.position.y);
                }
            }
        }

        for (int i = 0; i < enemyCount; i++)
        {
            if (enemies[i].GetComponent<BattleEntity>().isCasting && enemyArrows[i].transform.position.x >= actionPosition.x)
            {
                if (enemies[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    // TODO 공격 처리
                    enemies[i].GetComponent<BattleEntity>().isCasting = false;
                    enemies[i].GetComponent<BattleEntity>().action = Action.NONE;
                    enemyArrows[i].transform.position = new Vector2(cooldownPosition.x, enemyArrows[i].transform.position.y);
                }
            }
        }
    }

    public void dealCooldown()
    {
        if (isReachedCasting)
        {
            return;
        }

        for (int i = 0; i < teamCount; i++)
        {
            teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x + cooldownTeam[i], teamArrows[i].transform.position.y);

            if (teamArrows[i].transform.position.x >= castingPosition.x && !teams[i].GetComponent<BattleEntity>().isCasting)
            {
                isReachedCasting = true;
                actionMenu[i].SetActive(true);
                actionMenu[i].GetComponent<Animator>().SetBool("isActionMenuOn", true);
            }
        }

        for (int i = 0; i < enemyCount; i++)
        {
            enemyArrows[i].transform.position = new Vector2(enemyArrows[i].transform.position.x + cooldownEnemy[i], enemyArrows[i].transform.position.y);

            if (enemyArrows[i].transform.position.x >= castingPosition.x && !enemies[i].GetComponent<BattleEntity>().isCasting)
            {
                enemies[i].GetComponent<BattleEntity>().isCasting = true;
                enemies[i].GetComponent<BattleEntity>().action = Action.ATTACK;
                isReachedCasting = false;
            }
        }
    }

    // 전투 시작 설정
    public void setBattleField()
    {
        setCasting();
    }

    public void setCasting()
    {
        GameManager.instance.isBattle = true;
        isBattle = true;

        battleSet.SetActive(true);

        // Team arrow settings.
        for (int i = 0; i < teamCount; i++)
        {
            teamArrows[i].SetActive(true);
            teamArrows[i].GetComponent<Image>().sprite = teams[i].GetComponent<SpriteRenderer>().sprite;
            teamArrows[i].transform.position = new Vector2(cooldownPosition.x, cooldownPosition.y + teamPivot);
        }

        for (int i = teamCount; i < teams.Count; i++)
        {
            teamArrows[i].SetActive(false);
        }

        // Enemy arrow settings.
        for (int i = 0; i < enemyCount; i++)
        {
            enemyArrows[i].SetActive(true);
            enemyArrows[i].GetComponent<Image>().sprite = enemies[i].GetComponent<SpriteRenderer>().sprite;
            enemyArrows[i].transform.position = new Vector2(cooldownPosition.x, cooldownPosition.y + enemyPivot);
        }

        for (int i = enemyCount; i < enemies.Count; i++)
        {
            enemyArrows[i].SetActive(false);
        }


    }

    // 전투 시작 설정
    public void setFieldTeam(List<EntityData> petDatas = null)
    {
        teams[0].SetActive(true);
        teams[0].GetComponent<BattleEntity>().setBattleEntityData(GameManager.instance.playerData);

        if (petDatas == null)
        {
            for (int i = 1; i < teams.Count; i++)
            {
                teams[i].SetActive(false);
            }
            teamCount = 1;

            return;
        }

        teamCount = 1 + petDatas.Count;

        for (int i = 1; i < petDatas.Count + 1; i++)
        {
            teams[i].SetActive(true);
            teams[i].GetComponent<BattleEntity>().setBattleEntityData(petDatas[i - 1]);
            teams[i].GetComponent<BattleEntity>().isCasting = false;
        }

        for (int i = petDatas.Count + 1; i < teams.Count; i++)
        {
            teams[i].SetActive(false);
        }
    }

    // 전투 시작 설정
    public void setFieldEnemy(List<EntityData> entityDatas, int fieldType)
    {
        /*        GameObject go = Instantiate(mobPrefab);
                go.transform.position = new Vector2(dungeonLocation.x, dungeonLocation.y + 1f);*/

        this.fieldType = fieldType;
        enemyCount = entityDatas.Count;

        for (int i = 0; i < entityDatas.Count; i++)
        {
            enemies[i].SetActive(true);
            enemies[i].GetComponent<BattleEntity>().setBattleEntityData(entityDatas[i]);
            enemies[i].GetComponent<BattleEntity>().isCasting = false;

            enemies[i].GetComponent<SpriteRenderer>().sprite = entityDatas[i].loadSprite(entityDatas[i].spritePath);
        }

        for (int i = entityDatas.Count; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void pushAttackButton(int index)
    {
        actionMenu[index].GetComponent<Animator>().SetBool("isActionMenuOn", false);
        teams[index].GetComponent<BattleEntity>().isCasting = true;
        teams[index].GetComponent<BattleEntity>().action = Action.ATTACK;
        isReachedCasting = false;
        actionMenu[index].SetActive(false);
    }

    // a : 메소드 행위의 주체자, b : 메소드 행위의 피격 대상자
    public void doAttack(GameObject a, GameObject b)
    {
        dealDamage(a, b);
    }

    public void dealDamage(GameObject a, GameObject b, bool isMagicAttack = true)
    {
        float totalDamage = 0;

        totalDamage += a.GetComponent<BattleEntity>().power - b.GetComponent<BattleEntity>().armor;

        b.GetComponent<BattleEntity>().healthPoint -= totalDamage;

        if (b.GetComponent<BattleEntity>().healthPoint < 1)
        {
            a.GetComponent<BattleEntity>().expStack += b.GetComponent<BattleEntity>().expStack;
            dealDeath(b);
        }
    }

    public void dealDeath(GameObject gameObject)
    {
        if (!gameObject.tag.Equals("Monster"))
        {
            gameObject.SetActive(false);
        }
    }

    public void getExp()
    {
        for (int i = 0; i < teams.Count; i++)
        {
            teams[i].GetComponent<BattleEntity>().exp += (int) teams[i].GetComponent<BattleEntity>().expStack;
            teams[i].GetComponent<BattleEntity>().expStack = 0;
        }
    }
}
