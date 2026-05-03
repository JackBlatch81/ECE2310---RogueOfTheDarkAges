using System;

namespace RogueOfTheDarkAges
{
    public abstract class PlayerClass : Character
    {
        public string ClassName;

        public PlayerClass(string className, int maxHealth, Weapon weapon)
            : base("Player", maxHealth, weapon)
        {
            ClassName = className;
        }

        public virtual string GetClassDescription()
        {
            return "This is a class.";
        }
    }

    public class Knight : PlayerClass
    {
        public Knight() : base("Knight", 55, new Weapon(WeaponType.Sword, "Sword", 6, 10))
        {
        }

        public override int GetCriticalChance()
        {
            return 18;
        }

        public override int ReduceDamageWhenBlocking(int damage)
        {
            int reducedDamage = (int)Math.Ceiling(damage * 0.35);

            if (reducedDamage < 1)
            {
                reducedDamage = 1;
            }

            return reducedDamage;
        }

        public override string GetClassDescription()
        {
            return "High defense and very good at blocking.";
        }
    }

    public class Vanguard : PlayerClass
    {
        public Vanguard() : base("Vanguard", 60, new Weapon(WeaponType.BattleAxe, "Battle Axe", 7, 11))
        {
        }

        public override int GetCriticalChance()
        {
            return 16;
        }

        public override string GetClassDescription()
        {
            return "Has more health and strong Axe attacks.";
        }
    }

    public class Hunter : PlayerClass
    {
        public Hunter() : base("Hunter", 48, new Weapon(WeaponType.Bow, "Bow", 5, 11))
        {
        }

        public override int GetCriticalChance()
        {
            return 28;
        }

        public override string GetClassDescription()
        {
            return "Has a high critical hit chance with the Bow.";
        }
    }

    public class Rogue : PlayerClass
    {
        public Rogue() : base("Rogue", 45, new Weapon(WeaponType.Dagger, "Dagger", 5, 10))
        {
        }

        public override int GetCriticalChance()
        {
            return 30;
        }

        public override int Attack(Random random)
        {
            int firstHit = Weapon.GetDamage(random) + BonusDamage;
            int extraHit = random.Next(1, 4);
            return firstHit + extraHit;
        }

        public override string GetClassDescription()
        {
            return "Lower health, but fast attacks and high crit chance.";
        }
    }

    public class Mage : PlayerClass
    {
        public Mage() : base("Mage", 42, new Weapon(WeaponType.Wand, "Wand", 6, 12))
        {
        }

        public override int GetCriticalChance()
        {
            return 22;
        }

        public override string GetClassDescription()
        {
            return "Low health, but strong magic attacks.";
        }
    }
}
