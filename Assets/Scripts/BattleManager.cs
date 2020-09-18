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
    public bool isReachedCasting;

    // Targeting
    public int actionIndex;
    public bool isTargetSelectingOn = false;
    public GameObject selectedObject;
    public GameObject targetObject;
    public bool readyToAction = false;

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

        cooldownEnemy[0] = (0.080f * Screen.width) / term;
        cooldownEnemy[1] = (0.055f * Screen.width) / term;

        setResolution();

        castingBar = battleSet.transform.Find("CastingBar").gameObject;
        setArrowPosition();

        battleSet.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBattle)
        {
            dealActionTeam();
            dealActionEnemy();
            dealCooldown();
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
    
    public void dealActionTeam()
    {
        for (int i = 0; i < teamCount; i++)
        {
            if (teams[i].GetComponent<BattleEntity>().isCasting && teamArrows[i].transform.position.x >= actionPivot.x)
            {
                int transformIndex = teams[i].GetComponent<BattleEntity>().target.GetComponent<BattleEntity>().transformIndex;

                if (transformIndex < 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    // TODO 공격 처리
                    teams[transformIndex].GetComponent<BattleEntity>().takeDamage(10);

                    Debug.Log(teams[transformIndex].name + "의 남은 체력 : " + teams[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + teams[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 시 취소
                    if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        float previousPosition = teamArrows[transformIndex].transform.position.x;
                        float cooldownAmount = teamArrows[transformIndex].transform.position.x - cooldownPivot.x;
                        float newPosition = previousPosition - (cooldownAmount / 3 * 2);

                        teamArrows[transformIndex].transform.position = new Vector2(newPosition, teamArrows[transformIndex].transform.position.y);
                        teams[transformIndex].GetComponent<BattleEntity>().target = null;
                        teams[transformIndex].GetComponent<BattleEntity>().isCasting = false;
                    }
                    teams[i].GetComponent<BattleEntity>().target = null;

                    teams[i].GetComponent<BattleEntity>().isCasting = false;
                    teams[i].GetComponent<BattleEntity>().action = Action.NONE;
                    teamArrows[i].transform.position = new Vector2(cooldownPivot.x, teamArrows[i].transform.position.y);
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    transformIndex -= 10;
                    // TODO 공격 처리
                    enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(10);

                    Debug.Log(enemies[transformIndex].name + "의 남은 체력 : " + enemies[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + enemies[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 취소
                    if (enemies[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        float previousPosition = enemyArrows[transformIndex].transform.position.x;
                        float cooldownAmount = enemyArrows[transformIndex].transform.position.x - cooldownPivot.x;
                        float newPosition = previousPosition - (cooldownAmount / 3 * 2);

                        enemyArrows[transformIndex].transform.position = new Vector2(newPosition, enemyArrows[transformIndex].transform.position.y);
                        enemies[transformIndex].GetComponent<BattleEntity>().target = null;
                        enemies[transformIndex].GetComponent<BattleEntity>().isCasting = false;
                    }

                    teams[i].GetComponent<BattleEntity>().target = null; // 타겟 끄기

                    teams[i].GetComponent<BattleEntity>().isCasting = false;
                    teams[i].GetComponent<BattleEntity>().action = Action.NONE;
                    teamArrows[i].transform.position = new Vector2(cooldownPivot.x, teamArrows[i].transform.position.y);
                }
            }
        }
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
                    teams[transformIndex].GetComponent<BattleEntity>().takeDamage(10);

                    Debug.Log(teams[transformIndex].name + "의 남은 체력 : " + teams[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + teams[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 시 취소
                    if (teams[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        float previousPosition = teamArrows[transformIndex].transform.position.x;
                        float cooldownAmount = teamArrows[transformIndex].transform.position.x - cooldownPivot.x;
                        float newPosition = previousPosition - (cooldownAmount / 3 * 2);

                        teamArrows[transformIndex].transform.position = new Vector2(newPosition, teamArrows[transformIndex].transform.position.y);
                        teams[transformIndex].GetComponent<BattleEntity>().target = null;
                        teams[transformIndex].GetComponent<BattleEntity>().isCasting = false;
                    }
                    enemies[i].GetComponent<BattleEntity>().target = null;

                    enemies[i].GetComponent<BattleEntity>().isCasting = false;
                    enemies[i].GetComponent<BattleEntity>().action = Action.NONE;
                    enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, enemyArrows[i].transform.position.y);
                }
                else if (transformIndex >= 10 && teams[i].GetComponent<BattleEntity>().action == Action.ATTACK)
                {
                    transformIndex -= 10;
                    // TODO 공격 처리
                    enemies[transformIndex].GetComponent<BattleEntity>().takeDamage(10);

                    Debug.Log(enemies[transformIndex].name + "의 남은 체력 : " + enemies[transformIndex].GetComponent<BattleEntity>().healthPoint + "/" + enemies[transformIndex].GetComponent<BattleEntity>().healthPointMax);

                    // 캐스팅 취소
                    if (enemies[transformIndex].GetComponent<BattleEntity>().isCasting)
                    {
                        float previousPosition = enemyArrows[transformIndex].transform.position.x;
                        float cooldownAmount = enemyArrows[transformIndex].transform.position.x - cooldownPivot.x;
                        float newPosition = previousPosition - (cooldownAmount / 3 * 2);

                        enemyArrows[transformIndex].transform.position = new Vector2(newPosition, enemyArrows[transformIndex].transform.position.y);
                        enemies[transformIndex].GetComponent<BattleEntity>().target = null;
                        enemies[transformIndex].GetComponent<BattleEntity>().isCasting = false;
                    }

                    enemies[i].GetComponent<BattleEntity>().target = null; // 타겟 끄기

                    enemies[i].GetComponent<BattleEntity>().isCasting = false;
                    enemies[i].GetComponent<BattleEntity>().action = Action.NONE;
                    enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, enemyArrows[i].transform.position.y);
                }
            }
        }
    }

    public void setArrowPosition()
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
    public void dealCooldown()
    {
        if (isReachedCasting)
        {
            return;
        }

        // 팀 캐스팅
        for (int i = 0; i < teamCount; i++)
        {
            teamArrows[i].transform.position = new Vector2(teamArrows[i].transform.position.x + cooldownTeam[i], teamArrows[i].transform.position.y);

            if (teamArrows[i].transform.position.x >= castingPivot.x && !teams[i].GetComponent<BattleEntity>().isCasting)
            {
                isReachedCasting = true;
                actionMenu[i].SetActive(true);
                actionMenu[i].GetComponent<Animator>().SetBool("isActionMenuOn", true);
            }
        }

        // 적 캐스팅
        for (int i = 0; i < enemyCount; i++)
        {
            enemyArrows[i].transform.position = new Vector2(enemyArrows[i].transform.position.x + cooldownEnemy[i], enemyArrows[i].transform.position.y);

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
            // teamArrows[i].transform.position = new Vector2(cooldownPivot.x, cooldownPivot.y + teamPivot);
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
            // enemyArrows[i].transform.position = new Vector2(cooldownPivot.x, cooldownPivot.y + enemyPivot);
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
        teams[0].transform.GetChild(0).gameObject.SetActive(false); // Arrow

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
            teams[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow
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
            enemies[i].transform.GetChild(0).gameObject.SetActive(false); // Arrow
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
        isTargetSelectingOn = true;
        selectedObject = null;
        actionIndex = index;
        actionMenu[actionIndex].GetComponent<Animator>().SetBool("isActionMenuOn", false);
        teams[actionIndex].GetComponent<BattleEntity>().action = Action.ATTACK;
    }

    public void pushAttackButtonEnemy(int index)
    {
        selectedObject = null;
        actionIndex = index;
        enemies[actionIndex].GetComponent<BattleEntity>().action = Action.ATTACK;

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

    // 타겟 선택
    public void selectTargetMode()
    {
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

                    targetObject.transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
                }
                else // enemy 선택
                {
                    transformIndex -= 10;
                    teams[actionIndex].GetComponent<BattleEntity>().target = enemies[transformIndex];

                    targetObject.transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
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

                if (targetObject != null)
                {
                    targetObject.transform.GetChild(0).gameObject.SetActive(false); // Arrow 끄기
                }

                targetObject = selectedObject; // Arrow 켜기

                targetObject.transform.GetChild(0).gameObject.SetActive(true);
                selectedObject = null;
            }
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
