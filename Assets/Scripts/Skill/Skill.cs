using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Skill
{
    public int skillId;
    public string skillName;
    public string imagePath;
    public int level;
    public int experience;
    public string information;

    public Image image;

    public Skill()
    {

    }

    public Skill(int skillId, string skillName, string imagePath, int level, int experience, string information)
    {
        this.skillId = skillId;
        this.skillName = skillName;
        this.imagePath = imagePath;
        this.level = level;
        this.experience = experience;
        this.information = information;

        image = loadImage(imagePath);
    }

    [ContextMenu("From Json Data")]
    public Image loadImage(string path)
    {
        return Resources.Load<Image>(path);
    }

    public int calculateMastery(int level)
    {
        int value = 1;

        if (level < 10)
        {
            for (int i = 0; i < level + 1; i++)
            {
                value *= 2;
            }
            return level * value;
        }
        else
        {
            return 2000;
        }
    }

    public void addSkillExperience(int skillId)
    {
        experience += skillId;
    }

    public void setLevel()
    {
        int tempExp = experience;
        for(int i = 0; i < 100; i++)
        {
            if (experience - calculateMastery(i) < 0)
            {
                level = i;
                return;
            }
        }
    }
}
