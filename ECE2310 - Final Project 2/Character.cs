using System;

namespace RogueOfTheDarkAges
{
    public abstract class Character
    {
        public string Name;
        public int MaxHealth;
        public int Health;
        public int BonusDamage;
        public int Potions;
        public bool IsBoss;
        public Weapon Weapon;

        public Character(string name, int maxHealth, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            Weapon = weapon;
            BonusDamage = 0;
            Potions = 0;
            IsBoss = false;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public virtual int Attack(Random random)
        {
            return Weapon.GetDamage(random) + BonusDamage;
        }

        public virtual int GetCriticalChance()
        {
            return 20;
        }

        public virtual int ReduceDamageWhenBlocking(int damage)
        {
            int reducedDamage = (int)Math.Ceiling(damage * 0.45);

            if (reducedDamage < 1)
            {
                reducedDamage = 1;
            }

            return reducedDamage;
        }

        public int UsePotion(Random random)
        {
            int healAmount = random.Next(10, 19);
            Health = Health + healAmount;

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            Potions = Potions - 1;
            return healAmount;
        }

        public void RestoreHealthToFull()
        {
            Health = MaxHealth;
        }
    }
}
