using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;
    public SkillDataFile skillDataFile;

    public List<Skill> skillDB = new List<Skill>();

    public string spritePath = "Images/Items/Images";

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        skillDataFile = new SkillDataFile();
        skillDataFile.skillDatas = new List<Skill>();

        //saveSkillData();
        loadSkillData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("To Json Data")]
    public void saveSkillData()
    {
        Debug.Log("저장 성공");
        skillDataFile.skillDatas = new List<Skill>();

        skillDataFile.skillDatas.Add(new Skill(0, "허기인내", spritePath + "/" + 3000001, 100, 10000
            , "허기에 대한 저항력이 상승한다.\n\n포화수치가 20 % 미만을 유지하면 숙련도가 올라간다.\n\n포화 감소율 - 1 %\n최대 포화 + 1 % "));
        skillDataFile.skillDatas.Add(new Skill(1, "갈증인내", spritePath + "/" + 3000002, 100, 10000
            , "갈증에 대한 저항력이 상승한다.\n\n갈증수치가 20 % 미만을 유지하면 숙련도가 올라간다.\n\n갈증 감소율 - 1 %\n최대 갈증 + 1 % "));
        skillDataFile.skillDatas.Add(new Skill(2, "배변인내", spritePath + "/" + 3000002, 100, 10000
            , "배변에 대한 저항력이 상승한다.\n\n배변수치가 20 % 미만을 유지하면 숙련도가 올라간다.\n\n배변 감소율 - 1 %\n최대 배변 + 1 % "));
        skillDataFile.skillDatas.Add(new Skill(3, "기력인내", spritePath + "/" + 3000002, 100, 10000
            , "기력에 대한 저항력이 상승한다.\n\n기력수치가 20 % 미만을 유지하면 숙련도가 올라간다.\n\n기력 감소율 - 1 %\n최대 기력 + 1 % "));

        string jsonData = JsonUtility.ToJson(skillDataFile, true);

        Debug.Log(jsonData.Length);

        File.WriteAllText(saveOrLoad(false, true, "SkillData"), jsonData);
    }

    [ContextMenu("From Json Data")]
    public void loadSkillData()
    {
        try
        {
            Debug.Log("스킬 정보 로드 성공");
/*            string jsonData = File.ReadAllText(saveOrLoad(false, false, "SkillData"));
            skillDataFile = JsonUtility.FromJson<SkillDataFile>(jsonData);*/

            skillDataFile = JsonUtility.FromJson<SkillDataFile>(Resources.Load<TextAsset>("SkillData").ToString());

            for (int i = 0; i < skillDataFile.skillDatas.Count; i++)
            {
                skillDB.Add(skillDataFile.skillDatas[i]);
            }
        }
        catch (FileNotFoundException)
        {
            Debug.Log("로드 오류");

            string jsonData = JsonUtility.ToJson(skillDataFile, true);

            File.WriteAllText(saveOrLoad(false, false, "SkillData"), jsonData);
            loadSkillData();
        }
    }

    public string saveOrLoad(bool isMobile, bool isSave, string fileName)
    {
        if (isSave)
        {
            if (isMobile)
            {
                // 모바일 저장
                return Path.Combine(Application.persistentDataPath, fileName + ".json");
            }
            else
            {
                // pc 저장
                return Path.Combine(Application.dataPath, fileName + ".json");
            }
        }
        else
        {
            if (isMobile)
            {
                // 모바일 로드
                return Path.Combine(Application.persistentDataPath, fileName + ".json");
            }
            else
            {
                // pc 로드
                return Path.Combine(Application.dataPath, fileName + ".json");
            }
        }
    }

    public Skill findSkillById(int id)
    {
        for (int i = 0; i < skillDB.Count; i++)
        {
            if (skillDB[i].skillId == id)
            {
                return skillDB[i];
                Debug.Log(skillDB[i].skillName);
            }
        }
        Debug.Log("실패");
        return skillDB[0];
    }
}


[System.Serializable]
public class SkillDataFile
{
    public List<Skill> skillDatas;

    public Skill getSkill(int skillId)
    {
        for (int i = 0; i < skillDatas.Count; i++)
        {
            if (skillDatas[i].skillId == skillId)
            {
                return skillDatas[i];
            }
        }

        return null;
    }

    public void resetSkills()
    {
        for (int i = 0; i < skillDatas.Count; i++)
        {
            skillDatas[i].experience = 0;
            skillDatas[i].level = 0;
        }
    }
}