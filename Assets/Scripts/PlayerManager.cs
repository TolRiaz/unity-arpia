using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Map location;

    private float h;
    private float v;
    private float verticalSpeed;
    private float horizontalSpeed = 5.0f;
    private float motionStop = 0.1f;
    private int flipReverse = -1;
    private float footPosition = 0.59f;
    private float armPosition = 0.3f;
    private float rayRange = 0.8f;
    public float attackRange;
    private string[] layers = new string[3];
    private int removeMessageTimer;

    public JoystickValue value;
    public float joystickSpeed = 0.03f;

    private Vector3 direction;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private AudioSource playerAudioSource;

    // attack
    public Transform posRight;
    public Transform posLeft;
    public GameObject monster;
    public Vector2 boxSize;
    public bool isAttacking;
    public int damageTimer;
    public GameObject meleeRight;
    public GameObject meleeLeft;

    //levelUp
    public GameObject hudDamageText;
    public GameObject hudLevelUpText;
    public Transform hudPos;
    public bool isLevelUp;
    public bool isPointUp;

    //damaged
    private bool isMiss;
    public StatUI statUI;

    //equipment
    public GameObject leftHand;
    public GameObject[] leftHands;
    public int activeLeftHand;

    public GameObject rightHand;
    public GameObject[] rightHands;
    public int activeRightHand;

    public GameObject head;
    public GameObject[] heads;
    public int activeHead;

    public GameObject top;
    public GameObject[] tops;
    public int activeTop;

    public GameObject pant;
    public GameObject[] pants;
    public int activePants;

    public GameObject glove;
    public GameObject[] gloves;
    public int activeGloves;

    public GameObject shoe;
    public GameObject[] shoes;
    public int activeShoes;

    public GameObject neckless;
    public GameObject[] necklesses;
    public int activeNeckless;

    public GameObject earing;
    public GameObject[] earings;
    public int activeEaring;

    public GameObject ring;
    public GameObject[] rings;
    public int activeRing;

    public GameObject hair;
    public GameObject[] hairs;
    public int activeHair;

    [SerializeField] private AudioClip[] clip;

    public GameObject joystick;
    public bool isKnockDown;
    public GameObject knockDownSet;
    public GameObject popUp;

    // Object Controller
    public ObjectController objectController;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        location = Map.VILLAGE;

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        statUI = GameObject.Find("Canvas").GetComponent<StatUI>();
        verticalSpeed = horizontalSpeed / 3;
        layers[0] = "Entity";
        layers[1] = "Item";
        layers[2] = "Enemy";
        removeMessageTimer = 0;
        damageTimer = 0;
        knockDownSet.SetActive(false);
        popUp = knockDownSet.transform.GetChild(1).gameObject;

        attackRange = 1.5f;

        setEquipmentGameObject();

        activeLeftHand = -1;
        activeRightHand = -1;
        activeHead = -1;
        activeTop = -1;
        activePants = -1;
        activeGloves = -1;
        activeShoes = -1;
        activeNeckless = -1;
        activeEaring = -1;
        activeRing = -1;
        activeHair = -1;

        objectController = GetComponent<ObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        setEquipmentMotion();

        //levelUpSign
        if (isLevelUp)
        {
            playerAudioSource.clip = clip[3];
            playerAudioSource.Play();
            GameObject hudText = Instantiate(hudLevelUpText);
            hudText.transform.position = hudPos.position;
            isLevelUp = false;
        }

        if (isPointUp)
        {
            playerAudioSource.clip = clip[4];
            playerAudioSource.Play();
            isPointUp = false;
        }
    }

    // 장비 아이템 모션 적용
    public void setEquipmentMotion()
    {
        if (spriteRenderer.flipX)
        {
            if (activeLeftHand != -1)
            {
                leftHands[activeLeftHand].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeRightHand != -1)
            {
                rightHands[activeRightHand].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeHead != -1)
            {
                heads[activeHead].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeTop != -1)
            {
                tops[activeTop].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activePants != -1)
            {
                pants[activePants].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeGloves != -1)
            {
                gloves[activeGloves].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeShoes != -1)
            {
                shoes[activeShoes].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeNeckless != -1)
            {
                necklesses[activeNeckless].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeEaring != -1)
            {
                earings[activeEaring].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeRing != -1)
            {
                rings[activeRing].GetComponent<SpriteRenderer>().flipX = true;
            }
            if (activeHair != -1)
            {
                hairs[activeHair].GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (activeLeftHand != -1)
            {
                leftHands[activeLeftHand].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeRightHand != -1)
            {
                rightHands[activeRightHand].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeHead != -1)
            {
                heads[activeHead].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeTop != -1)
            {
                tops[activeTop].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activePants != -1)
            {
                pants[activePants].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeGloves != -1)
            {
                gloves[activeGloves].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeShoes != -1)
            {
                shoes[activeShoes].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeNeckless != -1)
            {
                necklesses[activeNeckless].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeEaring != -1)
            {
                earings[activeEaring].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeRing != -1)
            {
                rings[activeRing].GetComponent<SpriteRenderer>().flipX = false;
            }
            if (activeHair != -1)
            {
                hairs[activeHair].GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void setEquipmentGameObject()
    {
        for (int i = 0; i < leftHands.Length; i++)
        {
            leftHands[i] = leftHand.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < rightHands.Length; i++)
        {
            rightHands[i] = rightHand.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < heads.Length; i++)
        {
            heads[i] = head.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < tops.Length; i++)
        {
            tops[i] = top.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < pants.Length; i++)
        {
            pants[i] = pant.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < gloves.Length; i++)
        {
            gloves[i] = glove.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < shoes.Length; i++)
        {
            shoes[i] = shoe.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < necklesses.Length; i++)
        {
            necklesses[i] = neckless.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < earings.Length; i++)
        {
            earings[i] = earing.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < rings.Length; i++)
        {
            rings[i] = ring.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < hairs.Length; i++)
        {
            hairs[i] = hair.transform.GetChild(i).gameObject;
        }
    }

    private void rayControllerAttack(bool hDown, bool hUp)
    {
        if (hDown && h == -1)
        {
            direction = Vector3.left;
        }
        else if (hDown && h == 1)
        {
            direction = Vector3.right;
        }
    }

    void onDamaged(Vector2 targetPos, bool isMiss, float damageValue)
    {
        statUI.isDataChanged = true;

        // playerAudioSource.clip = clip[3];
        //playerAudioSource.Play();
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;

        if (damageValue <= 0 || isMiss)
        {
            hudText.GetComponent<DamageText>().damage = 0;
            gameObject.layer = 15;
            Invoke("offDamage", 3);

            return;
        }

        GameManager.instance.playerData.healthPoint -= damageValue;

        hudText.GetComponent<DamageText>().damage = (int)damageValue;

        if (!isMiss)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            int dirc = transform.position.x - targetPos.y > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 100, ForceMode2D.Impulse);
        }

        Invoke("offDamage", 3);
    }

    void offDamage()
    {
        gameObject.layer = 12;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public bool isHit(int accuracy)
    {
        int hit = accuracy - GameManager.instance.playerData.avoid;

        if (hit <= 0)
        {
            return false;
        }

        if (Random.Range(0, 20) > hit)
        {
            return false;
        }

        return true;
    }

    public void viewPopUp()
    {
        popUp.SetActive(true);
    }

    public void repeatKnockDown()
    {
        if (!isKnockDown)
        {
            return;
        }

        animator.SetBool("repeatKnockDown", true);
    }

    public void uiOnOffKnockDownSet()
    {
        GameManager.instance.playerData.healthPoint = GameManager.instance.playerData.healthPointMax;
        animator.SetBool("repeatKnockDown", false);
        isKnockDown = false;
        transform.position = new Vector2(-0.3f, -1f);
        GetComponent<Camera>().transform.position = new Vector2(-0.3f, -1f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().mapCode = 0;
        GameObject.Find("SoundManager").GetComponent<SoundManager>().refreshSounds();

        knockDownSet.SetActive(false);
    }
}
