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
    private GameObject battlePanel;
    public List<GameObject> teamArrows = new List<GameObject>();
    public List<GameObject> enemyArrows = new List<GameObject>();

    // Battle casting bar position
    private Vector2 chargingPivot;
    private Vector2 cooldownPivot;
    private Vector2 castingPivot;
    private Vector2 actionPivot;
    private float teamPivot = 140;
    private float enemyPivot = -120;

    // cooldown
    public bool isBattle;
    private float term = 50;
    public float[] cooldownTeam;
    public float[] cooldownEnemy;
    private float castingTime;
    public bool isReachedCasting;

    // Targeting
    public int actionIndex;
    public bool isTargetSelectingOn = false;
    public GameObject selectedObject;
    public GameObject targetObject;
    public bool readyToAction = false;
    public int nowCastingTurn;

    // Battle ending
    public int surviveTeamCount;
    public int surviveEnemyCount;
    public int totalExp;
    public float endingTimer;

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

            cooldownTeam[i] = (0.13f * Screen.width) / term;
        }

        // casting arrow register
        for (int i = 0; i < 4; i++)
        {
            teamArrows[i] = battleSet.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject;
            enemyArrows[i] = battleSet.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(i).gameObject;

            cooldownEnemy[i] = (0.0625f * Screen.width) / term;
        }

        cooldownEnemy[0] = (0.090f * Screen.width) / term;
        cooldownEnemy[1] = (0.075f * Screen.width) / term;

        setResolution();

        castingBar = battleSet.transform.Find("CastingBar").gameObject;
        battlePanel = battleSet.transform.Find("BattlePanel").gameObject;
        battlePanel.SetActive(false);
        setArrowPosition();

        battleSet.SetActive(false);

        castingTime = ((0.15625f * Screen.width) / term);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBattle)
        {
            dealActionTeam();
            dealActionEnemy();
            dealCooldown();
            dealBattleEnd();
        }
    }

    // 해상도 별 캐스팅 바 포지션 세팅
    public void setResolution()
    {
        chargingPivot = new Vector2(Screen.width / 2, Screen.height / 3);
        cooldownPivot = new Vector2(Screen.width / 4, Screen.height / 3);
        castingPivot = new Vector2(Screen.width / 12 * 7, Screen.height / 3);
        actionPivot = new Vector2(Screen.width / 4 * 3, Screen.height / 3);
    }
    
    // 캐스팅 처리, 공격 마법 처리, 행동 처리
    public void dealActionTeam()
    {
        for (int i = 0; i < teamCount; i++)
        {
            if (teams[i].GetComponent<BattleEntity>().isCasting && teamArrows[i].transform.position.x >= actionPivot.x)
            {
                int transformIndex = teams[i].GetComponent<BattleEntity>().target.GetComponent<BattleEntity>().transformIndex;

                // Action.ATTACK
                if (transformIndex < 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    // TODO 공격 처리
                    teams[transformIndex].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.ATTACK);

/*                    // 캐스팅 시 취소
                    if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, true);
                    }*/

                    setAfterAction(i);
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    transformIndex -= 10;
                    // TODO 공격 처리
                    enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.ATTACK);

/*                    // 캐스팅 취소
                    if (enemies[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, false);
                    }*/

                    setAfterAction(i);
                }

                // Action.MAGIC
                if (transformIndex < 10 && teams[i].GetComponent<BattleEntity>().action == Action.MAGIC)
                {
                    // 단일 타겟
                    if (teams[i].GetComponent<BattleEntity>().skill.isTargetOne)
                    {
                        teams[transformIndex].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.MAGIC);

                        setAfterAction(i);
                    }
                    // 다중 타겟
                    else
                    {
                        for (int j = 0; j < enemyCount; j++)
                        {
                            if (teams[j].gameObject.activeSelf)
                            {
                                teams[j].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.MAGIC);
                            }
                        }

                        setAfterAction(i);
                    }
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.MAGIC)
                {
                    transformIndex -= 10;

                    // 단일 타겟
                    if (teams[i].GetComponent<BattleEntity>().skill.isTargetOne)
                    {
                        enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.MAGIC);

                        setAfterAction(i);
                    }
                    // 다중 타겟
                    else
                    {
                        for (int j = 0; j < enemyCount; j++)
                        {
                            if (enemies[j].gameObject.activeSelf)
                            {
                                enemies[j].GetComponent<BattleEntity>().takeDamage(teams[i].GetComponent<BattleEntity>(), Action.MAGIC);
                            }
                        }

                        setAfterAction(i);
                    }
                }

            }
        }

        applyResult();
    }

    public void dealActionEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemies[i].GetComponent<BattleEntity>().isCasting && enemyArrows[i].transform.position.x >= actionPivot.x)
            {
                Debug.Log(enemies[i].name + "의 공격 준비 완료! 대상 : " + enemies[i].GetComponent<BattleEntity>().target.name);

                int transformIndex = enemies[i].GetComponent<BattleEntity>().target.GetComponent<BattleEntity>().transformIndex;

                if (transformIndex < 10 && enemies[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    // TODO 공격 처리
                    teams[transformIndex].GetComponent<BattleEntity>().takeDamage(enemies[i].GetComponent<BattleEntity>(), Action.ATTACK);

                    Debug.Log(teams[transformIndex].name + "의 남은 체력 : " + teams[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + teams[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 시 취소
/*                    if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, true);
                    }*/

                    setAfterAction(i, false);
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    transformIndex -= 10;
                    // TODO 공격 처리
                    enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(enemies[i].GetComponent<BattleEntity>(), Action.ATTACK);

                    Debug.Log(enemies[transformIndex].name + "의 남은 체력 : " + enemies[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + enemies[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 취소
/*                    if (enemies[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, false);
                    }*/

                    setAfterAction(i, false);
                }

                // Action.MAGIC
                if (transformIndex < 10 && enemies[i].GetComponent<BattleEntity>().action == Action.MAGIC)
                {
                    // TODO 공격 처리
                    teams[transformIndex].GetComponent<BattleEntity>().takeDamage(enemies[i].GetComponent<BattleEntity>(), Action.ATTACK);

                    Debug.Log(teams[transformIndex].name + "의 남은 체력 : " + teams[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + teams[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 시 취소
/*                    if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, true);
                    }*/

                    setAfterAction(i, false);
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.MAGIC)
                {
                    transformIndex -= 10;
                    // TODO 공격 처리
                    enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(enemies[i].GetComponent<BattleEntity>(), Action.ATTACK);

                    Debug.Log(enemies[transformIndex].name + "의 남은 체력 : " + enemies[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + enemies[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 취소
/*                    if (enemies[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        cancleCasting(transformIndex, false);
                    }*/

                    setAfterAction(i, false);
                }
            }
        }

        applyResult();
    }

    // 액션 처리 부분, 캐스팅 취소
    public void cancleCasting(int transformIndex, bool isTeam = true)
    {
        if (transformIndex >= 10)
        {
            isTeam = false;
            transformIndex -= 10;
        }

        if (isTeam)
        {
            float previousPosition = teamArrows[transformIndex].transform.position.x;
            float cooldownAmount = teamArrows[transformIndex].transform.position.x - cooldownPivot.x;
            float newPosition = previousPosition - (cooldownAmount / 3 * 2);

            teamArrows[transformIndex].transform.position = new Vector2(newPosition, teamArrows[transformIndex].transform.position.y);
            teams[transformIndex].GetComponent<BattleEntity>().target = null;
            teams[transformIndex].GetComponent<BattleEntity>().isCasting = false;
        }
        else
        {
            float previousPosition = enemyArrows[transformIndex].transform.position.x;
            float cooldownAmount = enemyArrows[transformIndex].transform.position.x - cooldownPivot.x;
            float newPosition = previousPosition - (cooldownAmount / 3 * 2);

            enemyArrows[transformIndex].transform.position = new Vector2(newPosition, enemyArrows[transformIndex].transform.position.y);
            enemies[transformIndex].GetComponent<BattleEntity>().target = null;
            enemies[transformIndex].GetComponent<BattleEntity>().isCasting = false;
        }
    }

    // 액션 처리 부분, 리셋
    private void setAfterAction(int i, bool isTeam = true)
    {
        if (isTeam)
        {
            teams[i].GetComponent<BattleEntity>().target = null;

            teams[i].GetComponent<BattleEntity>().isCasting = false;
            teams[i].GetComponent<BattleEntity>().action = Action.NONE;
            teamArrows[i].transform.position = new Vector2(cooldownPivot.x, teamArrows[i].transform.position.y);
        }
        else
        {
            enemies[i].GetComponent<BattleEntity>().target = null; // 타겟 끄기

            enemies[i].GetComponent<BattleEntity>().isCasting = false;
            enemies[i].GetComponent<BattleEntity>().action = Action.NONE;
            enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, enemyArrows[i].transform.position.y);
        }
    }

    // 사망 처리
    public void dealDeath(int index)
    {
        if (index < 10)
        {
            surviveTeamCount--;
            teams[index].GetComponent<BattleEntity>().isDead = true;
            teamArrows[index].gameObject.SetActive(false);
        }
        else
        {
            index -= 10;
            surviveEnemyCount--;
            enemies[index].GetComponent<BattleEntity>().isDead = true;

            // TODO 사후 처리
            enemies[index].gameObject.SetActive(false);
            enemyArrows[index].gameObject.SetActive(false);
        }
    }

    private void dealBattleEnd()
    {
        if (surviveTeamCount == 0 || surviveEnemyCount == 0)
        {
            if (endingTimer < 2) 
            {
                endingTimer += Time.deltaTime;
            }
            else
            {
                if (surviveTeamCount == 0)
                {
                    surviveTeamCount = -1;

                    for (int i = 0; i < teamCount; i++)
                    {
                        teamArrows[i].gameObject.SetActive(false);
                        enemyArrows[i].gameObject.SetActive(false);
                    }
                }
                else if (surviveEnemyCount == 0)
                {
                    surviveEnemyCount = -1;

                    for (int i = 0; i < teamCount; i++)
                    {
                        teamArrows[i].gameObject.SetActive(false);
                        enemyArrows[i].gameObject.SetActive(false);
                    }

                    SoundManager.instance.stopAllSounds();
                    SoundManager.instance.playEffectSound(2);

                    getExp();
                }
            }
        }
    }

    public void offBattleSet()
    {
        if (surviveTeamCount <= 0)
        {
            GameManager.instance.isBattle = false;
            isBattle = false;
            SoundManager.instance.stopAllSounds();
            SoundManager.instance.playMusic(19);

            GameObject.Find("Main Camera").transform.position = new Vector2(
                GameObject.Find("Main Camera").GetComponent<MainCamera>().playerTransform.position.x,
                GameObject.Find("Main Camera").GetComponent<MainCamera>().playerTransform.position.y);

            battleSet.SetActive(false);
        }
        else if (surviveEnemyCount <= 0)
        {
            GameManager.instance.isBattle = false;
            isBattle = false;
            SoundManager.instance.stopAllSounds();
            SoundManager.instance.playMusic(19);

            GameObject.Find("Main Camera").transform.position = new Vector2(
                GameObject.Find("Main Camera").GetComponent<MainCamera>().playerTransform.position.x,
                GameObject.Find("Main Camera").GetComponent<MainCamera>().playerTransform.position.y);

            battleSet.SetActive(false);
        }

        battlePanel.SetActive(false);

        GameManager.instance.levelUp();
    }

    public void getExp()
    {
        battlePanel.SetActive(true);

        for (int i = 0; i < teams.Count; i++)
        {
            if (teams[i].GetComponent<BattleEntity>().isDead)
            {
                continue;
            }

            teams[i].GetComponent<BattleEntity>().exp += (int)teams[i].GetComponent<BattleEntity>().expStack;
            totalExp += (int)teams[i].GetComponent<BattleEntity>().expStack;
            teams[i].GetComponent<BattleEntity>().expStack = 0;

            if (i == 0)
            {
                GameManager.instance.setPlayerDataByEntityData(teams[0].GetComponent<BattleEntity>());
            }
        }

        battlePanel.transform.GetChild(2).gameObject.GetComponent<Text>().text = "총 획득 경험치 : " + totalExp;
    }

    private void setArrowPosition()
    {
        for (int i = 0; i < teamArrows.Count; i++)
        {
            teamArrows[i].transform.position = new Vector2(cooldownPivot.x, chargingPivot.y * 2 + Screen.height / 4);
            //teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x, Screen.height * 0.0740741f);
        }

        for (int i = 0; i < enemyArrows.Count; i++)
        {
            enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, chargingPivot.y * 2);
            //enemyArrows[i].transform.position = new Vector2(enemyArrows[i].transform.position.x, -(Screen.height * 0.055556f));
        }
    }

    // 행동 쿨타임 컨트롤러
    private void dealCooldown()
    {
        if (isReachedCasting || surviveTeamCount < 0 || surviveEnemyCount < 0)
        {
            return;
        }

        // 팀 캐스팅
        for (int i = 0; i < teamCount; i++)
        {
            if (teamArrows[i].activeSelf)
            {
                // 캐스팅
                if (teams[i].GetComponent<BattleEntity>().isCasting)
                {
                    if (teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                    {
                        cooldownTeam[i] = castingTime / 0.5f;
                        teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x + cooldownTeam[i], teamArrows[i].transform.position.y);
                    }
                    // 스킬별 쿨타임 적용
                    else if (teams[i].GetComponent<BattleEntity>().action == Action.MAGIC)
                    {
                        cooldownTeam[i] = castingTime / teams[i].GetComponent<BattleEntity>().skill.castingTime;
                        teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x + cooldownTeam[i], teamArrows[i].transform.position.y);
                    }

                }
                // 쿨타임
                else
                {
                    cooldownTeam[i] = castingTime;
                    teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x + cooldownTeam[i], teamArrows[i].transform.position.y);
                }
            }
            else
            {
                continue;
            }

            if (teamArrows[i].transform.position.x >= castingPivot.x && !teams[i].GetComponent<BattleEntity>().isCasting)
            {
                isReachedCasting = true;
                nowCastingTurn = i;
                actionMenu[i].SetActive(true);
                actionMenu[i].GetComponent<Animator>().SetBool("isActionMenuOn", true);
            }
        }

        // 적 캐스팅
        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyArrows[i].activeSelf)
            {
                enemyArrows[i].transform.position = new Vector2(enemyArrows[i].transform.position.x + cooldownEnemy[i], enemyArrows[i].transform.position.y);
            }

            if (enemyArrows[i].transform.position.x >= castingPivot.x && !enemies[i].GetComponent<BattleEntity>().isCasting)
            {
                // TODO 아래 정의해둔 것은 버튼안에서 이루어지는 동작임, 수정 필요
                pushAttackButtonEnemy(i);
/*                isReachedCasting = false;

                int transformIndex = 0;
                teams[actionIndex].GetComponent<BattleEntity>().target = teams[transformIndex];

                teams[transformIndex].GetComponent<BattleEntity>().takeDamage(10);

                Debug.Log(teams[transformIndex].name + "의 남은 체력 : " + teams[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + teams[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                enemies[actionIndex].GetComponent<BattleEntity>().target = null;

*//*                if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                {
                    float previousPosition = teams[transformIndex].transform.position.x;
                    float cooldownAmount = teams[transformIndex].transform.position.x - cooldownPosition.x;
                    float newPosition = previousPosition - (cooldownAmount / 3 * 2);

                    teams[transformIndex].transform.position = new Vector2(newPosition, teams[transformIndex].transform.position.y);
                    teams[transformIndex].GetComponent<BattleEntity>().target = null;
                    teams[transformIndex].GetComponent<BattleEntity>().isCasting = false;
                }*/
            }
        }
    }

    // 전투 시작 설정
    public void setBattleField()
    {
        setCasting();

        totalExp = 0;
    }

    public void setCasting()
    {
        //GameManager.instance.isBattle = true;
        isBattle = true;

        battleSet.SetActive(true);

        // Team arrow settings.
        for (int i = 0; i < teamCount; i++)
        {
            teamArrows[i].SetActive(true);
            teamArrows[i].GetComponent<Image>().sprite = teams[i].GetComponent<SpriteRenderer>().sprite;
            teamArrows[i].transform.position = new Vector2(cooldownPivot.x, cooldownPivot.y + teamPivot);
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
            enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, cooldownPivot.y + enemyPivot);
        }

        for (int i = enemyCount; i < enemies.Count; i++)
        {
            enemyArrows[i].SetActive(false);
        }
    }

    // 전투 시작 설정
    public void setFieldTeam(List<EntityData> petDatas = null)
    {
        surviveTeamCount = 0;

        teams[0].SetActive(true);
        surviveTeamCount++;
        teams[0].GetComponent<BattleEntity>().isDead = false;
        teams[0].GetComponent<BattleEntity>().setBattleEntityData(GameManager.instance.playerData);
        setActionMenuSkill(0);
        teams[0].transform.GetChild(0).gameObject.SetActive(false); // Arrow
        teams[0].GetComponent<BattleEntity>().isCasting = false;

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
            surviveTeamCount++;
            teams[i].GetComponent<BattleEntity>().isDead = false;
            teams[i].GetComponent<BattleEntity>().setBattleEntityData(petDatas[i - 1]);
            setActionMenuSkill(i);
            teams[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow
            teams[i].GetComponent<BattleEntity>().isCasting = false;

/*            if (teams[i].transform.childCount > 3)
            {
                Destroy(teams[i].transform.GetChild(3).gameObject);
            }

            //enemies[i].GetComponent<SpriteRenderer>().sprite = entityDatas[i].loadSprite(entityDatas[i].spritePath);

            GameObject enemyPrefab = entityDatas[i].loadPrefab(entityDatas[i].prefabPath);
            GameObject go = Instantiate(enemyPrefab);
            go.transform.SetParent(teams[i].transform);
            go.GetComponent<Transform>().localScale = new Vector3(0.75f, 0.75f, 1);
            go.transform.position = teams[i].transform.GetChild(2).position; // 이유는 모르겠지만 해당 위치가 0, -1 됨*/
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

        surviveEnemyCount = 0;

        this.fieldType = fieldType;
        enemyCount = entityDatas.Count;
        endingTimer = 0;

        for (int i = 0; i < entityDatas.Count; i++)
        {
            enemies[i].SetActive(true);
            surviveEnemyCount++;
            enemies[i].GetComponent<BattleEntity>().isDead = false;
            enemies[i].GetComponent<BattleEntity>().setBattleEntityData(entityDatas[i]);
            enemies[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow
            enemies[i].GetComponent<BattleEntity>().isCasting = false;

            if (enemies[i].transform.childCount > 3)
            {
                Destroy(enemies[i].transform.GetChild(3).gameObject);
            }

            enemies[i].GetComponent<SpriteRenderer>().sprite = entityDatas[i].loadSprite(entityDatas[i].spritePath);

            GameObject enemyPrefab = entityDatas[i].loadPrefab(entityDatas[i].prefabPath);
            GameObject go = Instantiate(enemyPrefab);
            go.transform.SetParent(enemies[i].transform);
            go.GetComponent<Transform>().localScale = new Vector3(0.75f, 0.75f, 1);
            go.transform.position = enemies[i].transform.GetChild(2).position; // 이유는 모르겠지만 해당 위치가 0, -1 됨
        }

        for (int i = entityDatas.Count; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void setActionMenuSkill(int index)
    {
        Transform spellMenu = actionMenu[index].transform.GetChild(1);
        List<Skill> skills = teams[index].GetComponent<BattleEntity>().skills;
        int spellMenuCount = 4;
        int fire = 0;
        int ice = 0;
        int earth = 0;
        int none = 0;

        for (int i = 0; i < spellMenuCount; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                spellMenu.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }

            for (int j = 0; j < skills.Count; j++)
            {
                // 불 마법 ( i 는 마법 탭 종류 )
                if (i == 0 && skills[j].skillId < 100)
                {
                    spellMenu.GetChild(i).GetChild(fire).gameObject.SetActive(true);
                    spellMenu.GetChild(i).GetChild(fire).gameObject.GetComponent<SpriteRenderer>().sprite = skills[j].sprite;
                    spellMenu.GetChild(i).GetChild(fire).gameObject.GetComponent<ActionSlot>().skill = skills[j];
                    fire++;
                }
                else if (i == 1 && skills[j].skillId >= 100 && skills[j].skillId < 200)
                {
                    spellMenu.GetChild(i).GetChild(ice).gameObject.SetActive(true);
                    spellMenu.GetChild(i).GetChild(ice).gameObject.GetComponent<SpriteRenderer>().sprite = skills[j].sprite;
                    spellMenu.GetChild(i).GetChild(ice).gameObject.GetComponent<ActionSlot>().skill = skills[j];
                    ice++;
                }
                else if (i == 2 && skills[j].skillId >= 200 && skills[j].skillId < 300)
                {
                    spellMenu.GetChild(i).GetChild(earth).gameObject.SetActive(true);
                    spellMenu.GetChild(i).GetChild(earth).gameObject.GetComponent<SpriteRenderer>().sprite = skills[j].sprite;
                    spellMenu.GetChild(i).GetChild(earth).gameObject.GetComponent<ActionSlot>().skill = skills[j];
                    earth++;
                }
                else if (i == 3 && skills[j].skillId >= 300 && skills[j].skillId < 400)
                {
                    spellMenu.GetChild(i).GetChild(none).gameObject.SetActive(true);
                    spellMenu.GetChild(i).GetChild(none).gameObject.GetComponent<SpriteRenderer>().sprite = skills[j].sprite;
                    spellMenu.GetChild(i).GetChild(none).gameObject.GetComponent<ActionSlot>().skill = skills[j];
                    none++;
                }
            }

        }
    }

    // 타겟 선택
    public void selectTargetMode()
    {
        // 너무 빠른 타겟 클릭으로 다음 턴 액션 메뉴 애니메이션이 완벽하게 꺼지지 않은 상태가 되는 현상 방지
        if (!(actionMenu[actionIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ActionEnd")
            && actionMenu[actionIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f))
        {
            return;
        }

        if (isTargetSelectingOn)
        {
            // 최종 타겟 선택
            if (selectedObject != null && selectedObject == targetObject)
            {
                int transformIndex = targetObject.GetComponent<BattleEntity>().transformIndex;

                // team 선택
                if (transformIndex < 10)
                {
                    teams[actionIndex].GetComponent<BattleEntity>().target = teams[transformIndex];

                    offArrowUI();
                }
                else // enemy 선택
                {
                    transformIndex -= 10;
                    teams[actionIndex].GetComponent<BattleEntity>().target = enemies[transformIndex];

                    offArrowUI();
                }

                readyToAction = true;
                targetObject = null;
                selectedObject = null;
                isTargetSelectingOn = false;

                teams[actionIndex].GetComponent<BattleEntity>().isCasting = true;
                isReachedCasting = false;
                actionMenu[actionIndex].SetActive(false);

                applyResult();

                return;
            }

            // 최초 타겟 선택
            if (targetObject != selectedObject)
            {
                Debug.Log("선택한 오브젝트 : " + selectedObject.name);

                // 스킬 타겟 단일
                if (teams[nowCastingTurn].GetComponent<BattleEntity>().skill.isTargetOne || teams[nowCastingTurn].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    if (targetObject != null)
                    {
                        offArrowUI();
                    }

                    targetObject = selectedObject;

                    targetObject.transform.GetChild(0).gameObject.SetActive(true); // Arrow 켜기
                    targetObject.GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", true); // Arrow UI 켜기
                    selectedObject = null;
                }
                // 스킬 타겟 다중
                else
                {
                    if (targetObject != null)
                    {
                        offArrowUI();
                    }

                    targetObject = selectedObject;

                    if (targetObject.GetComponent<BattleEntity>().transformIndex < 10)
                    {
                        for (int i = 0; i < teamCount; i++)
                        {
                            teams[i].transform.GetChild(0).gameObject.SetActive(true); // Arrow 켜기
                            teams[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", true); // Arrow UI 켜기
                        }
                    }
                    else
                    {
                        for (int i = 0; i < enemyCount; i++)
                        {
                            enemies[i].transform.GetChild(0).gameObject.SetActive(true); // Arrow 켜기
                            enemies[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", true); // Arrow UI 켜기
                        }
                    }
                }
            }
        }
    }

    private void offArrowUI()
    {
        for (int i = 0; i < teamCount; i++)
        {
            teams[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
            teams[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", false); // Arrow UI 끄기
        }
        for (int i = 0; i < enemyCount; i++)
        {
            enemies[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
            enemies[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", false); // Arrow UI 끄기
        }
    }

    // 버튼 처리
    public void buttonCancel()
    {
        if (! (!(actionMenu[actionIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ActionMagic"))
            || actionMenu[actionIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ActionMagic")
            && actionMenu[actionIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f) )
        {
            return;
        }

        SoundManager.instance.playButtonEffectSound();
        isTargetSelectingOn = false;

        if (targetObject != null)
        {
            for (int i = 0; i < teamCount; i++)
            {
                teams[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
                teams[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", false); // Arrow UI 끄기
            }
            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
                enemies[i].GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", false); // Arrow UI 끄기
            }

            selectedObject = null;
            targetObject = selectedObject;
        }

        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMenuOn", true);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMagicOn", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellFire", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellIce", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellEarth", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellNone", false);
        teams[actionIndex].GetComponent<BattleEntity>().action = Action.NONE;
    }

    public void buttonSpellCancel()
    {
        

        SoundManager.instance.playButtonEffectSound();
        isTargetSelectingOn = false;

        if (targetObject != null)
        {
            targetObject.transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
            targetObject.GetComponent<BattleEntity>().arrowUI.SetBool("isUIOn", false); // Arrow UI 끄기
            selectedObject = null;
            targetObject = selectedObject;
        }

        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellFire", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellIce", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellEarth", false);
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellNone", false);
        teams[actionIndex].GetComponent<BattleEntity>().action = Action.NONE;
    }

    public void buttonAttack(int index)
    {
        SoundManager.instance.playButtonEffectSound();
        isTargetSelectingOn = true;
        selectedObject = null;
        actionIndex = index;
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMenuOn", false);
        teams[actionIndex].GetComponent<BattleEntity>().action = Action.ATTACK;
        teams[actionIndex].GetComponent<BattleEntity>().skill = new Skill(-1);
    }

    public void buttonMagic(int index)
    {
        SoundManager.instance.playButtonEffectSound();
        actionIndex = index;
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMagicOn", true);
    }

    public void buttonSpell(int spellType)
    {
        SoundManager.instance.playButtonEffectSound();
        switch (spellType)
        {
            case 0:
                actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellFire", true);
                break;
            case 1:
                actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellIce", true);
                break;
            case 2:
                actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellEarth", true);
                break;
            case 3:
                actionMenu[actionIndex].GetComponent<Animator>().SetBool("isSpellNone", true);
                break;
        }
    }

    public void buttonUseSkill(int index)
    {
        // TODO 버튼을 누른 슬롯을 추적하여 스킬정보를 가져와서 등록하기

        Transform spellMenu = actionMenu[nowCastingTurn].transform.GetChild(1);

        if (index < 10)
        {
            teams[nowCastingTurn].GetComponent<BattleEntity>().skill = spellMenu.GetChild(0).GetChild(index).gameObject.GetComponent<ActionSlot>().skill;
        }
        else if (index >= 100 && index < 200)
        {
            index -= 100;
            teams[nowCastingTurn].GetComponent<BattleEntity>().skill = spellMenu.GetChild(1).GetChild(index).gameObject.GetComponent<ActionSlot>().skill;
        }
        else if (index >= 200 && index < 300)
        {
            index -= 200;
            teams[nowCastingTurn].GetComponent<BattleEntity>().skill = spellMenu.GetChild(2).GetChild(index).gameObject.GetComponent<ActionSlot>().skill;
        }
        else if (index >= 300 && index < 400)
        {
            index -= 300;
            teams[nowCastingTurn].GetComponent<BattleEntity>().skill = spellMenu.GetChild(3).GetChild(index).gameObject.GetComponent<ActionSlot>().skill;
        }

        SoundManager.instance.playButtonEffectSound();
        isTargetSelectingOn = true;
        selectedObject = null;
        actionIndex = nowCastingTurn;
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMenuOn", false);
        teams[actionIndex].GetComponent<BattleEntity>().action = Action.MAGIC;
    }

    public void pushAttackButtonEnemy(int index)
    {
        selectedObject = null;
        actionIndex = index;
        enemies[actionIndex].GetComponent<BattleEntity>().action = Action.ATTACK;
        enemies[actionIndex].GetComponent<BattleEntity>().skill = new Skill(-1);

        // 임시로 타겟은 플레이어
        targetObject = teams[0].gameObject;

        // 최종 타겟 선택
        if (targetObject != null)
        {
            int transformIndex = targetObject.GetComponent<BattleEntity>().transformIndex;

            // team 선택
            if (transformIndex < 10)
            {
                Debug.Log(enemies[index].name + "이 타겟을 등록 : " + targetObject.name);
                enemies[actionIndex].GetComponent<BattleEntity>().target = teams[transformIndex];
            }
            else // enemy 선택
            {
                transformIndex -= 10;
                enemies[actionIndex].GetComponent<BattleEntity>().target = enemies[transformIndex];
            }

            targetObject = null;

            enemies[actionIndex].GetComponent<BattleEntity>().isCasting = true;
            isReachedCasting = false;

            applyResult();

            return;
        }
    }

    public void applyResult()
    {
        GameManager.instance.playerData.setPlayerDataByBattleEntity(teams[0].GetComponent<BattleEntity>());

        for (int i = 1; i < teamCount; i++)
        {
            // TODO 펫이 추가되면 펫에게도 실시간 정보 반송
        }
    }
}
