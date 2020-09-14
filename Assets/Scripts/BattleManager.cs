using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> teams = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // a : 메소드 행위의 주체자, b : 메소드 행위의 피격 대상자
    public void doAttack(GameObject a, GameObject b)
    {
        dealDamage(a, b);
    }

    public void dealDamage(GameObject a, GameObject b, bool isMagicAttack = true)
    {
        float totalDamage = 0;

        totalDamage += a.GetComponent<PlayerData>().power - b.GetComponent<PlayerData>().armor;

        b.GetComponent<PlayerData>().healthPoint -= totalDamage;

        if (b.GetComponent<PlayerData>().healthPoint < 1)
        {
            a.GetComponent<PlayerData>().expStack += b.GetComponent<PlayerData>().expStack;
            dealDeath(b);
        }
    }

    public void dealDeath(GameObject gameObject)
    {
        if (!gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
