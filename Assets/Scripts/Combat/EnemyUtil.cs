using System.Collections.Generic;
enum EnemyType
{
    Snail,
    Cactus,
    Bandit,
    DesertWarrior,
    DesertShinobi,
    DarkShinobi,
    ShadeShinobi,
}

public class EnemyUtil
{
    public static int s_currentEnemyID = 0;

    public static bool[] s_enemyAliveStatus = new bool[18];

    public static void ResetEnemyStatus()
    {
        for (int i = 0; i < s_enemyAliveStatus.Length; ++i)
        {
            s_enemyAliveStatus[i] = true;
        }
    }

    

    public static void SetupBandit(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "BANDIT";
        attrs.Playable = false;
        attrs.Gold = 6;
        attrs.Xp = 8;

        Attribute mhp = new Attribute("MHP", 15);
        Attribute hp = new Attribute("HP", 15);
        Attribute dmg = new Attribute("Dmg", 4);
        Attribute def = new Attribute("Def", 2);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    public static void SetupWarrior(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "WARRIOR";
        attrs.Playable = false;
        attrs.Gold = 52;
        attrs.Xp = 45;

        Attribute mhp = new Attribute("MHP", 75);
        Attribute hp = new Attribute("HP", 75);
        Attribute dmg = new Attribute("Dmg", 2);
        Attribute def = new Attribute("Def", 50);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    public static void SetupCactus(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "CACTUS";
        attrs.Playable = false;
        attrs.Gold = 3;
        attrs.Xp = 4;

        Attribute mhp = new Attribute("MHP", 5);
        Attribute hp = new Attribute("HP", 5);
        Attribute dmg = new Attribute("Dmg", 3);
        Attribute def = new Attribute("Def", 1);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    public static void SetupShinobiDesert(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "DESERT NINJA";
        attrs.Playable = false;
        attrs.Gold = 15;
        attrs.Xp = 35;

        Attribute mhp = new Attribute("MHP", 15);
        Attribute hp = new Attribute("HP", 15);
        Attribute dmg = new Attribute("Dmg", 6);
        Attribute def = new Attribute("Def", 2);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    public static void SetupShinobiDark(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "DARK NINJA";
        attrs.Playable = false;
        attrs.Gold = 67;
        attrs.Xp = 64;

        Attribute mhp = new Attribute("MHP", 25);
        Attribute hp = new Attribute("HP", 25);
        Attribute dmg = new Attribute("Dmg", 9);
        Attribute def = new Attribute("Def", 10);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    // rare enemy
    public static void SetupShinobiShade(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "SHADE NINJA";
        attrs.Playable = false;
        attrs.Gold = 142;
        attrs.Xp = 312;

        Attribute mhp = new Attribute("MHP", 50);
        Attribute hp = new Attribute("HP", 50);
        Attribute dmg = new Attribute("Dmg", 35);
        Attribute def = new Attribute("Def", 10);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }

    public static void SetupSnail(CharacterAttributes attrs)
    {
        attrs.ClearAttributes();

        attrs.Name = "SNAIL";
        attrs.Playable = false;
        attrs.Gold = 2;
        attrs.Xp = 1;

        Attribute mhp = new Attribute("MHP", 2);
        Attribute hp = new Attribute("HP", 2);
        Attribute dmg = new Attribute("Dmg", 1);
        Attribute def = new Attribute("Def", 1);
        attrs.AddAttribute(mhp);
        attrs.AddAttribute(hp);
        attrs.AddAttribute(dmg);
        attrs.AddAttribute(def);
    }
}
