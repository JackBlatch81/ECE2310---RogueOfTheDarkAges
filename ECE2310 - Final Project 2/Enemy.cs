namespace RogueOfTheDarkAges
{
    public class Enemy : Character
    {
        public string EnemyClassName;

        public Enemy(string name, string enemyClassName, int maxHealth, Weapon weapon)
            : base(name, maxHealth, weapon)
        {
            EnemyClassName = enemyClassName;
        }
    }
}
