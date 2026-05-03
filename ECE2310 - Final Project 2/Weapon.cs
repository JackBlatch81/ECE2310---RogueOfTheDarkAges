using System;

namespace RogueOfTheDarkAges
{
    public class Weapon
    {
        public WeaponType Type;
        public string Name;
        public int MinDamage;
        public int MaxDamage;

        public Weapon(WeaponType type, string name, int minDamage, int maxDamage)
        {
            Type = type;
            Name = name;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public int GetDamage(Random random)
        {
            return random.Next(MinDamage, MaxDamage + 1);
        }
    }
}
