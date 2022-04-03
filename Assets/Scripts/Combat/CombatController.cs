using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    private FirstStrikeChance m_firstStrikeScript;

    private Dictionary<int, GameObject> m_battleOrder;

    private float m_battleWait = 1.5f;

    [SerializeField]
    private List<GameObject> m_party;
    public List<GameObject> Party { get { return m_party; } set { m_party = value; } }
    public Vector2[] PartyInitPositions;
    public Vector2[] EnemyInitPositions;
    public List<GameObject> EnemyList { get; set; }

    private GameObject[] EnemySelectors;
    private GameObject[] XpBars;

    private int m_currentChar;

    public Text m_statusTxt;
    public Text m_rewardTxt;

    private int m_goldReward = 0;
    private int m_xpReward = 0;

    // Start is called before the first frame update
    void Start()
    {
        EnemySelectors = new GameObject[9];
        XpBars = new GameObject[4];

        PartyInitPositions = new Vector2[4];
        EnemyInitPositions = new Vector2[9];

        GetComponent<CombatCursorController>().CurrentPartyIndex = 0;

        if (m_party != null)
        {
            foreach (var member in Party)
            {
                member.GetComponent<CharacterAttributes>().Level = 1;
                member.GetComponent<CharacterAttributes>().Gold = 100;
                member.GetComponent<CharacterAttributes>().Xp = 0;
                member.GetComponent<CharacterAttributes>().LevelUpThreshold = 100;
            }
        }

        SetupSelectors();
        SetupXpBars();
        Combat();
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatEnum.CombatState.ActionSelect == CombatEnum.s_currentCombatState)
        {
            m_statusTxt.text = "CHOOSE ACTION";

            if (GetComponent<CombatCursorController>().ChooseEnemyTarget)
            {
                m_statusTxt.text = "CHOOSE ENEMY";

                if (Input.GetMouseButtonUp(1))
                {
                    GetComponent<CombatCursorController>().RevertAttackAction();
                    //cancel sound
                }
            }
            else
            {
                // action selection shortcuts
                if (Input.GetMouseButtonUp(1))
                {
                    GetComponent<CombatCursorController>().AttackAction();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    GetComponent<CombatCursorController>().FleeAction();
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    GetComponent<CombatCursorController>().BlockAction();
                }
            }
        }

        else if (CombatEnum.CombatState.Battle == CombatEnum.s_currentCombatState)
        {
            GenEnemyActions();
            StartCoroutine(ExecuteBattleOrder());
            CombatEnum.s_currentCombatState = CombatEnum.CombatState.Inactive;
        }

        else if (CombatEnum.CombatState.Victory == CombatEnum.s_currentCombatState ||
            CombatEnum.CombatState.Failure == CombatEnum.s_currentCombatState ||
            CombatEnum.CombatState.Escape == CombatEnum.s_currentCombatState)
        {
            if (CombatEnum.CombatState.Victory == CombatEnum.s_currentCombatState)
            {

                m_rewardTxt.text = "REWARD: " + m_goldReward + 'G' + "\n                  " + m_xpReward + "XP";
            }

            else if (CombatEnum.CombatState.Escape == CombatEnum.s_currentCombatState)
            {
                m_statusTxt.text = "ESCAPED!";
            }

            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetMouseButtonDown(0))
            {
                if (CombatEnum.CombatState.Victory == CombatEnum.s_currentCombatState)
                {
                }
                else if (CombatEnum.CombatState.Escape == CombatEnum.s_currentCombatState)
                {
                }
                else if (CombatEnum.CombatState.Failure == CombatEnum.s_currentCombatState)
                {
                }
            }

            else if (CombatEnum.CombatState.Battle == CombatEnum.s_currentCombatState)
            {
                GenEnemyActions();
                ExecuteBattleOrder();
                CombatEnum.s_currentCombatState = CombatEnum.CombatState.ActionSelect;
                m_currentChar = -1;
                ChangeActivePartyMember();
            }
        }
    }

    public void Combat()
    {
        if (!Utilities.s_testMode)
        {
            UpdateXpBars();
            GenerateEnemies();

            CalculateGoldXpRewards(ref m_goldReward, ref m_xpReward, EnemyList);
            StartCombat();
            GetComponent<GenerateGrids>().CreatePartyGrid();
            GetComponent<GenerateGrids>().CreateEnemyGrid();
            PositionPartyOnGrid();

            PositionEnemyOnGrid();

            GetComponent<CombatUIController>().SetupNameTexts(Party);
            GetComponent<CombatUIController>().UpdateHpTexts(Party);

            CombatEnum.s_currentCombatState = CombatEnum.CombatState.ActionSelect;

            m_currentChar = -1;
            ChangeActivePartyMember();
        }
        else
        {
            GenerateEnemies();
            StartCombat();
            GetComponent<GenerateGrids>().CreatePartyGrid();
            GetComponent<GenerateGrids>().CreateEnemyGrid();
        }
    }

    public void StartCombat()
    {
        m_battleOrder = new Dictionary<int, GameObject>();

        m_firstStrikeScript = GetComponent<FirstStrikeChance>();

        m_firstStrikeScript.SetOnAdvantage(CombatEnum.s_advantage);

        if (m_firstStrikeScript.FirstStrikeCheck())
        {
            Debug.Log("You get to strike first.");

            for (int i = 0; i < Party.Count; ++i)
            {
                m_battleOrder.Add(i + 1, Party[i]);
            }

            for (int i = 0; i < EnemyList.Count; ++i)
            {
                m_battleOrder.Add(i + Party.Count + 1, EnemyList[i]);
            }

            foreach (var item in m_battleOrder)
            {
                Debug.Log(item.Key + ": " + item.Value.GetComponent<CharacterAttributes>().Name);
            }
        }
        else
        {
            Debug.Log("Enemies strike first.");

            for (int i = 0; i < EnemyList.Count; ++i)
            {
                m_battleOrder.Add(i + 1, EnemyList[i]);
            }

            for (int i = 0; i < Party.Count; ++i)
            {
                m_battleOrder.Add(i + EnemyList.Count + 1, Party[i]);
            }

            foreach (var item in m_battleOrder)
            {
                Debug.Log(item.Key + ": " + item.Value.GetComponent<CharacterAttributes>().Name);
            }
        }
    }

    public void GenerateEnemies()
    {
        EnemyList = new List<GameObject>();

        GameObject characterTemp = Resources.Load<GameObject>("CharacterTemplate");

        int m_enemyCount = Random.Range(1, 10);

        for (int i = 0; i < m_enemyCount; i++)
        {
            GameObject enemy = Instantiate(characterTemp);

            int rareEnemyChance = Random.Range(1, 101);

            EnemyType enemyType;

            if (rareEnemyChance >= 90) // 10% chance
            {
                enemyType = (EnemyType)6;
            }
            else
            {
                enemyType = (EnemyType)Random.Range(0, 6);
            }

            switch (enemyType)
            {
                case EnemyType.Bandit:
                    EnemyUtil.SetupBandit(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bandit");
                    break;
                case EnemyType.DesertWarrior:
                    EnemyUtil.SetupWarrior(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("desert-warrior");
                    break;
                case EnemyType.Cactus:
                    EnemyUtil.SetupCactus(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cactus-revenge");
                    break;
                case EnemyType.DesertShinobi:
                    EnemyUtil.SetupShinobiDesert(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("desert-shinobi");
                    break;
                case EnemyType.DarkShinobi:
                    EnemyUtil.SetupShinobiDark(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dark-shinobi");
                    break;
                case EnemyType.ShadeShinobi:
                    EnemyUtil.SetupShinobiShade(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("shade-shinobi");
                    break;
                case EnemyType.Snail:
                    EnemyUtil.SetupSnail(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("snail");
                    break;
                default:
                    break;
            }

            EnemyList.Add(enemy);
        }
    }

    public static List<GameObject> GenerateEnemiesTest()
    {
        List<GameObject> enemyTest = new List<GameObject>();

        GameObject characterTemp = Resources.Load<GameObject>("CharacterTemplate");

        int m_enemyCount = 1;

        for (int i = 0; i < m_enemyCount; i++)
        {
            GameObject enemy = Instantiate(characterTemp);

            EnemyType enemyType = EnemyType.Bandit;

            switch (enemyType)
            {
                case EnemyType.Bandit:
                    EnemyUtil.SetupBandit(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bandit");
                    break;
                case EnemyType.DesertWarrior:
                    EnemyUtil.SetupWarrior(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("desert-warrior");
                    break;
                case EnemyType.Cactus:
                    EnemyUtil.SetupCactus(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cactus-revenge");
                    break;
                case EnemyType.DesertShinobi:
                    EnemyUtil.SetupShinobiDesert(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("desert-shinobi");
                    break;
                case EnemyType.DarkShinobi:
                    EnemyUtil.SetupShinobiDark(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dark-shinobi");
                    break;
                case EnemyType.ShadeShinobi:
                    EnemyUtil.SetupShinobiShade(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("shade-shinobi");
                    break;
                case EnemyType.Snail:
                    EnemyUtil.SetupSnail(enemy.GetComponent<CharacterAttributes>());
                    enemy.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("snail");
                    break;
                default:
                    break;
            }

            enemyTest.Add(enemy);
        }
        return enemyTest;
    }

    public void PositionPartyOnGrid()
    {
        for (int i = 0; i < Party.Count; ++i)
        {
            Party[i].transform.position = GetComponent<GenerateGrids>().PartyGrid[i, 1];
            PartyInitPositions[i] = GetComponent<GenerateGrids>().PartyGrid[i, 1];
        }
    }

    public void PositionEnemyOnGrid()
    {
        int k = 0;

        for (int i = 0; i < GetComponent<GenerateGrids>().RowEnemy; ++i)
        {
            for (int j = 0; j < GetComponent<GenerateGrids>().ColumnEnemy; ++j)
            {
                EnemyList[k].transform.position = GetComponent<GenerateGrids>().EnemyGrid[j, i];
                EnemyInitPositions[k] = GetComponent<GenerateGrids>().EnemyGrid[j, i];
                ++k;

                if (k >= EnemyList.Count) break;
            }
            if (k >= EnemyList.Count) break;
        }
    }

    public void ChangeActivePartyMember()
    {
        // shift previous character back
        if (m_currentChar != -1)
        {
            Party[m_currentChar].transform.position = GetComponent<GenerateGrids>().PartyGrid[m_currentChar, 1];
            PartyInitPositions[m_currentChar] = GetComponent<GenerateGrids>().PartyGrid[m_currentChar, 1];
        }

        m_currentChar++;
        GetComponent<CombatCursorController>().CurrentPartyIndex = m_currentChar;

        while (m_currentChar < Party.Count && !Party[m_currentChar].activeSelf)
        {
            m_currentChar++;
            GetComponent<CombatCursorController>().CurrentPartyIndex = m_currentChar;
        }

        // if current character is the last character, return to first character and start battle
        if (m_currentChar >= Party.Count)
        {
            Party[Party.Count - 1].transform.position = GetComponent<GenerateGrids>().PartyGrid[Party.Count - 1, 1];
            PartyInitPositions[Party.Count - 1] = GetComponent<GenerateGrids>().PartyGrid[Party.Count - 1, 1];
            m_currentChar = 0;
            GetComponent<CombatCursorController>().CurrentPartyIndex = m_currentChar;
            Debug.Log(GetComponent<CombatCursorController>().CurrentPartyIndex);
            CombatEnum.s_currentCombatState = CombatEnum.CombatState.Battle;
            return;
        }
        Party[m_currentChar].transform.position = GetComponent<GenerateGrids>().PartyGrid[m_currentChar, 0];
        PartyInitPositions[m_currentChar] = GetComponent<GenerateGrids>().PartyGrid[m_currentChar, 0];
    }

    public void GenEnemyActions()
    {
        for (int i = 0; i < EnemyList.Count; ++i)
        {
            if (!EnemyList[i].activeSelf) continue;

            // if hp less than half
            if (EnemyList[i].GetComponent<CharacterAttributes>().FindAttribute("HP").Value <=
                EnemyList[i].GetComponent<CharacterAttributes>().FindAttribute("MHP").Value / 2)
            {
                int chance = Random.Range(1, 101);

                if (chance >= 60)
                {
                    EnemyList[i].GetComponent<ActionController>().Action = ActionController.CombatAction.Block;
                    EnemyList[i].GetComponent<ActionController>().StatusTxt = m_statusTxt;
                    continue;
                }
            }

            EnemyList[i].GetComponent<ActionController>().Action = ActionController.CombatAction.Fight;

            int targetPartyMember = Random.Range(0, 4);

            if (!Party[targetPartyMember].activeSelf)
            {
                --i;
                continue;
            }

            EnemyList[i].GetComponent<ActionController>().Target = Party[targetPartyMember];
            EnemyList[i].GetComponent<ActionController>().TargetInitPos = PartyInitPositions[targetPartyMember];
            EnemyList[i].GetComponent<ActionController>().StatusTxt = m_statusTxt;
        }
    }

    public void GetNewEnemyTarget(out GameObject newTarget, out Vector2 newTargetInitPos)
    {
        newTarget = null;
        newTargetInitPos = new Vector2();

        for (int i = 0; i < EnemyList.Count; ++i)
        {
            if (EnemyList[i].activeSelf)
            {
                newTarget = EnemyList[i];
                newTargetInitPos = EnemyInitPositions[i];
            }
        }
    }

    public void GetNewPartyTarget(out GameObject newTarget, out Vector2 newTargetInitPos)
    {
        newTarget = null;
        newTargetInitPos = new Vector2();

        for (int i = 0; i < Party.Count; ++i)
        {
            if (Party[i].activeSelf)
            {
                newTarget = Party[i];
                newTargetInitPos = PartyInitPositions[i];
            }
        }
    }

    public IEnumerator ExecuteBattleOrder()
    {
        int playableChar = -1;

        foreach (var character in m_battleOrder)
        {
            if (character.Value.activeSelf)
            {
                if (character.Value.GetComponent<CharacterAttributes>().Playable)
                {
                    playableChar++;
                    if (character.Value.GetComponent<ActionController>().Action == ActionController.CombatAction.Fight)
                    {
                        character.Value.transform.position = GetComponent<GenerateGrids>().PartyGrid[playableChar, 0];
                    }
                    character.Value.GetComponent<ActionController>().ExecuteAction();
                    GetComponent<CombatUIController>().UpdateHpTexts(Party);
                    UpdateHpBars();
                    yield return new WaitForSeconds(m_battleWait);
                    character.Value.transform.position = GetComponent<GenerateGrids>().PartyGrid[playableChar, 1];
                }
                else
                {
                    character.Value.GetComponent<ActionController>().ExecuteAction();
                    GetComponent<CombatUIController>().UpdateHpTexts(Party);
                    UpdateHpBars();
                    yield return new WaitForSeconds(m_battleWait);
                }
            }
            else if (!character.Value.activeSelf)
            {
                if (character.Value.GetComponent<CharacterAttributes>().Playable)
                {
                    playableChar++;
                }
            }

            if (CombatEnum.s_currentCombatState == CombatEnum.CombatState.Escape || BattleEnd()) yield break;
        }

        if (CombatEnum.CombatState.Victory != CombatEnum.s_currentCombatState &&
            CombatEnum.CombatState.Failure != CombatEnum.s_currentCombatState &&
            CombatEnum.CombatState.Escape != CombatEnum.s_currentCombatState)
        {
            CombatEnum.s_currentCombatState = CombatEnum.CombatState.ActionSelect;
            m_currentChar = -1;
            ChangeActivePartyMember();
        }

    }

    private bool BattleEnd()
    {
        int enemyCount = 0;
        int partyCount = 0;

        foreach (var member in Party)
        {
            if (!member.activeSelf) partyCount++;
            if (partyCount >= Party.Count)
            {
                CombatEnum.s_currentCombatState = CombatEnum.CombatState.Failure;
                Debug.Log("You have lost the battle...");
                m_statusTxt.text = "YOU LOST THE BATTLE...";
                return true;
            }
        }

        foreach (var enemy in EnemyList)
        {
            if (!enemy.activeSelf) enemyCount++;
            if (enemyCount >= EnemyList.Count)
            {
                CombatEnum.s_currentCombatState = CombatEnum.CombatState.Victory;
                Debug.Log("All enemies terminated!");
                m_statusTxt.text = "YOU WON THE BATTLE!";
                updateXp();
                UpdateXpBars();

                return true;
            }
        }

        return false;
    }

    public static bool BattleEndTest(List<GameObject> party, List<GameObject> enemies)
    {
        int enemyCount = 0;
        int partyCount = 0;

        foreach (var member in party)
        {
            if (!member.activeSelf) partyCount++;
            if (partyCount >= party.Count)
            {
                CombatEnum.s_currentCombatState = CombatEnum.CombatState.Failure;
                return true;
            }
        }

        foreach (var enemy in enemies)
        {
            if (!enemy.activeSelf) enemyCount++;
            if (enemyCount >= enemies.Count)
            {
                CombatEnum.s_currentCombatState = CombatEnum.CombatState.Victory;
                return true;
            }
        }

        return false;
    }

    public static void CalculateGoldXpRewards(ref int goldReward, ref int xpReward, List<GameObject> enemies)
    {
        foreach (var enemy in enemies)
        {
            goldReward += enemy.GetComponent<CharacterAttributes>().Gold;
            xpReward += enemy.GetComponent<CharacterAttributes>().Xp;
        }
    }

    public void UpdateHpBars()
    {
        foreach (var selector in EnemySelectors)
        {
            selector.GetComponent<EnemySelector>().transform.GetChild(0).GetComponent<HPDisplayController>().UpdateHpBar(selector.GetComponent<EnemySelector>());
        }
    }

    void SetupSelectors()
    {
        for (int i = 0; i < EnemySelectors.Length; ++i)
        {
            Debug.Log("EnemyPos" + (i + 1).ToString());
            EnemySelectors[i] = GameObject.Find("EnemyPos" + (i + 1).ToString());
        }
    }

    void SetupXpBars()
    {
        for (int i = 0; i < XpBars.Length; ++i)
        {
            Debug.Log("XpBar" + (i + 1).ToString());
            XpBars[i] = GameObject.Find("XPBar" + (i + 1).ToString());
        }
    }

    public void UpdateXpBars()
    {
        for (int i = 0; i < XpBars.Length; ++i)
        {
            XpBars[i].GetComponent<XpBarController>().UpdateXpBar(Party[i]);
        }
    }

    public void updateXp()
    {
        for (int i = 0; i <= 3; i++)
        {
            if (m_party[i].activeSelf && CombatEnum.CombatState.Escape != CombatEnum.s_currentCombatState)
            {
                m_party[i].GetComponent<CharacterAttributes>().Xp += m_xpReward;

                if (m_party[i].GetComponent<CharacterAttributes>().Xp >= m_party[i].GetComponent<CharacterAttributes>().LevelUpThreshold)
                {
                    Party[i].transform.GetChild(0).gameObject.SetActive(true);

                    if (Party[i].GetComponent<CharacterAttributes>().Name == "NINJA")
                    {
                        PartyUtil.LevelUpFighter(m_party[i].GetComponent<CharacterAttributes>());
                    }
                    else
                    {
                        PartyUtil.LevelUpFighter(m_party[i].GetComponent<CharacterAttributes>());
                    }
                }
            }
        }
    }
}
