using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    public List<EntityData> entityDatas;
    public int fieldType = 1;
    public bool isBattleStart;
    public bool isFinish;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isBattleStart = false;
        isFinish = false;

        animator = GetComponent<Animator>();

        // 임시 테스트용
        setEntity(MonsterDatabase.instance.mobDB[1]);
        setEntity(MonsterDatabase.instance.mobDB[1]);
        setEntity(MonsterDatabase.instance.mobDB[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBattleStart)
        {
            if (!isFinish)
            {
                collisionState();
                animator.SetTrigger("doCollision");
                isFinish = true;
            }
            GameObject.Find("Player").GetComponent<MouseMovement>().stopMovement();

            Invoke("changeScene", 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !BattleManager.instance.isBattle)
        {
            isBattleStart = true;
        }
    }

    public void changeScene()
    {
        // TODO 충돌 후 전투 처리
        BattleManager.instance.setFieldEnemy(entityDatas, fieldType);
        BattleManager.instance.setFieldTeam();

        BattleManager.instance.setBattleField();

        SoundManager.instance.stopAllSounds();
        SoundManager.instance.playMusic(21);

        CancelInvoke("changeScene");

        Destroy(gameObject);
    }

    private void collisionState()
    {
        GameManager.instance.isBattle = true;
        SoundManager.instance.stopAllSounds();
        SoundManager.instance.playEffectSound(1);
    }

    public void setEntity(EntityData entityData)
    {
        entityDatas.Add(entityData);
    }

    public void setFieldType(int fieldType)
    {
        this.fieldType = fieldType;
    }
}
