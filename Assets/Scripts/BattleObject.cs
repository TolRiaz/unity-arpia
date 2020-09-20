using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    public List<EntityData> entityDatas;
    public int fieldType = 1;

    // Start is called before the first frame update
    void Start()
    {
        // 임시 테스트용
        setEntity(MonsterDatabase.instance.mobDB[0]);
        setEntity(MonsterDatabase.instance.mobDB[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // TODO 충돌 후 전투 처리
            BattleManager.instance.setFieldEnemy(entityDatas, fieldType);
            BattleManager.instance.setFieldTeam();

            BattleManager.instance.setBattleField();

            SoundManager.instance.stopAllSounds();
            SoundManager.instance.playMusic(21);

            Destroy(gameObject);
        }
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
