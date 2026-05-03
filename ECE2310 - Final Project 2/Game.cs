using System;

namespace RogueOfTheDarkAges
{
    public class Game
    {
        private Random random;
        private PlayerClass player;
        private string[] environments;
        private string[] enemyNames;

        public Game()
        {
            random = new Random();

            environments = new string[]
            {
                "a fog-covered swamp where the earth squelches beneath your boots",
                "the ruins of an old watchtower scattered with broken stone",
                "a pine forest clearing lit by a pale morning sun",
                "a muddy battlefield abandoned after a brutal clash",
                "a narrow mountain pass with cold wind cutting through the rocks",
                "an overgrown monastery courtyard filled with creeping ivy",
                "a torchlit castle hallway smelling of smoke and iron",
                "a river crossing where the current crashes against worn pillars",
                "a village square left eerily silent after nightfall",
                "a windswept cliffside where gulls cry over the sea"
            };

            enemyNames = new string[]
            {
                "Brigand",
                "Mercenary",
                "Raider",
                "Footman",
                "Pirate",
                "Hunter",
                "Knight",
                "Marauder",
                "Bandit Captain",
                "Grave Robber",
                "Soldier",
                "Guard",
                "Scout",
                "Warrior"
            };
        }

        public void Run()
        {
            bool playAgain = true;
            Console.Title = "Rogue of the Dark Ages";

            while (playAgain)
            {
                StartNewGame();
                bool playerWon = PlayGame();

                Console.WriteLine();

                if (playerWon)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You defeated every enemy and slayed the Ancient Dragon!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your journey ends in the Dark Ages.");
                }

                Console.ResetColor();
                playAgain = AskYesOrNo("Would you like to play again? (y/n): ");
                Console.Clear();
            }
        }

        private void StartNewGame()
        {
            Console.Clear();
            PrintTitle();

            Console.WriteLine("Welcome to Rogue of the Dark Ages.");
            Console.WriteLine("You must survive 6 random levels and then defeat the final boss.");
            Console.WriteLine("Each turn you can attack, block, or heal if you have a potion.");
            Console.WriteLine("Attack deals damage to opponent, giving the enemy the chance to attack back.");
            Console.WriteLine("Block negates 75% of damage taken, and has a 25% chance to deal counter damage.");
            Console.WriteLine("Heal gives player back 10-19 health and uses a potion.");
            Console.WriteLine("Good Luck Adventurer!");
            Console.WriteLine();

            player = ChooseClass();
            player.Potions = 4;

            Console.WriteLine();
            Console.WriteLine("You chose the " + player.ClassName + ".");
            Console.WriteLine("Starting weapon: " + player.Weapon.Name);
            Console.WriteLine(player.GetClassDescription());
            Console.WriteLine("You start with 4 potions.");
            Console.WriteLine();
            Console.WriteLine("Press any key to start.");
            Console.ReadKey();
        }

        private bool PlayGame()
        {
            for (int level = 1; level <= 6; level++)
            {
                Console.Clear();
                Enemy enemy = CreateEnemy(level);
                string place = environments[random.Next(environments.Length)];

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== Level " + level + " ===");
                Console.ResetColor();
                Console.WriteLine("You arrive in " + place + ".");
                string enemyPhrase = enemy.Name + " " + enemy.EnemyClassName;
                Console.WriteLine("A " + enemyPhrase + " appears with a " + enemy.Weapon.Name + ".");
                Console.WriteLine();

                bool wonBattle = Battle(enemy, false);

                if (!wonBattle)
                {
                    return false;
                }

                if (level < 6)
                {
                    GiveUpgrade();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("You survived the 6 levels. Now the final boss is waiting...");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("=== Final Boss ===");
            Console.ResetColor();
            Console.WriteLine("An Ancient Dragon flies down from the sky in a storm of fire and ash.");
            Console.WriteLine();

            Enemy dragon = new Enemy("Ancient Dragon", "Boss", 95, new Weapon(WeaponType.IgnitedClaws, "Ignited Claws", 10, 15));
            dragon.IsBoss = true;
            dragon.BonusDamage = 4;
            dragon.Potions = 1;

            return Battle(dragon, true);
        }

        private bool Battle(Enemy enemy, bool isBoss)
        {
            player.RestoreHealthToFull();

            while (player.IsAlive() && enemy.IsAlive())
            {
                ShowBattleInfo(enemy, isBoss);

                BattleAction playerAction = GetPlayerAction();
                BattleAction enemyAction = GetEnemyAction(enemy, isBoss);

                bool playerBlocked = false;
                bool enemyBlocked = false;

                if (playerAction == BattleAction.Block)
                {
                    playerBlocked = true;
                }

                if (enemyAction == BattleAction.Block)
                {
                    enemyBlocked = true;
                }

                if (playerAction == BattleAction.Heal)
                {
                    int healAmount = player.UsePotion(random);
                    Console.WriteLine("Player used a potion and healed " + healAmount + " health.");
                }
                else if (playerAction == BattleAction.Attack)
                {
                    DoAttack(player, enemy, enemyBlocked, true);
                }
                else
                {
                    Console.WriteLine("Player prepares to block.");
                }

                if (!enemy.IsAlive())
                {
                    break;
                }

                if (enemyAction == BattleAction.Heal)
                {
                    int healAmount = enemy.UsePotion(random);
                    Console.WriteLine("The " + enemy.Name + " used a potion and healed " + healAmount + " health.");
                }
                else if (enemyAction == BattleAction.Attack)
                {
                    DoAttack(enemy, player, playerBlocked, false);
                }
                else
                {
                    Console.WriteLine("The " + enemy.Name + " prepares to block.");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key for the next turn.");
                Console.ReadKey();
                Console.Clear();
            }

            if (player.IsAlive())
            {
                Console.ForegroundColor = ConsoleColor.Green;

                if (isBoss)
                {
                    Console.WriteLine("The dragon crashes to the ground. You win!");
                }
                else
                {
                    Console.WriteLine("You defeated the " + enemy.Name + ".");
                }

                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (isBoss)
                {
                    Console.WriteLine("The dragon burns through your final defense.");
                }
                else
                {
                    Console.WriteLine("The " + enemy.Name + " defeated you.");
                }

                Console.ResetColor();
                return false;
            }
        }

        private void DoAttack(Character attacker, Character defender, bool defenderBlocked, bool playerIsAttacking)
        {
            int damage = attacker.Attack(random);
            bool critical = random.Next(1, 101) <= attacker.GetCriticalChance();

            if (critical)
            {
                damage = damage + random.Next(3, 7);
            }

            int originalDamage = damage;
            bool countered = false;
            int counterChance = 25; // small chance to counterattack

            if (defenderBlocked)
            {
                damage = (int)Math.Ceiling(originalDamage * 0.25);

                if (damage < 1)
                {
                    damage = 1;
                }

                if (random.Next(1, 101) <= counterChance)
                {
                    countered = true;
                }
            }

            defender.Health = defender.Health - damage;

            if (defender.Health < 0)
            {
                defender.Health = 0;
            }

            string attackerName;
            string targetName;

            if (playerIsAttacking)
            {
                attackerName = "Player";
                targetName = defender.Name;
            }
            else
            {
                attackerName = "The " + attacker.Name;
                targetName = "player";
            }

            if (defenderBlocked)
            {
                if (critical)
                {
                    Console.WriteLine(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                        " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                }
                else
                {
                    Console.WriteLine(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                        " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                }
            }
            else
            {
                if (critical)
                {
                    Console.WriteLine(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                        " and deals " + damage + " damage.");
                }
                else
                {
                    Console.WriteLine(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                        " and deals " + damage + " damage.");
                }
            }

            if (countered && defender.IsAlive())
            {
                attacker.Health = attacker.Health - originalDamage;

                if (attacker.Health < 0)
                {
                    attacker.Health = 0;
                }

                string counterAttackerName;
                string counterTargetName;

                if (playerIsAttacking)
                {
                    counterAttackerName = "The " + defender.Name;
                    counterTargetName = "player";
                }
                else
                {
                    counterAttackerName = "Player";
                    counterTargetName = attacker.Name;
                }

                Console.WriteLine(counterAttackerName + " counters and deals " + originalDamage + " damage to " + counterTargetName + "!");
            }
        }

        private void GiveUpgrade()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Choose an upgrade:");
            Console.ResetColor();
            Console.WriteLine("1. Increase max health by 10");
            Console.WriteLine("2. Increase weapon damage by 2");
            Console.WriteLine("3. Gain 1 potion");
            Console.WriteLine();

            bool validChoice = false;

            while (!validChoice)
            {
                Console.Write("Enter 1, 2, or 3: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    player.MaxHealth = player.MaxHealth + 10;
                    player.Health = player.MaxHealth;
                    Console.WriteLine("Your max health is now " + player.MaxHealth + ".");
                    validChoice = true;
                }
                else if (input == "2")
                {
                    player.BonusDamage = player.BonusDamage + 2;
                    player.Health = player.MaxHealth;
                    Console.WriteLine("Your bonus damage is now " + player.BonusDamage + ".");
                    validChoice = true;
                }
                else if (input == "3")
                {
                    player.Potions = player.Potions + 1;
                    player.Health = player.MaxHealth;
                    Console.WriteLine("You now have " + player.Potions + " potion(s).");
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private void ShowBattleInfo(Enemy enemy, bool isBoss)
        {
            if (isBoss)
            {
                Console.WriteLine("=== Boss Battle ===");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=== Battle ===");
                Console.ResetColor();
            }

            Console.WriteLine("Class: " + player.ClassName);
            Console.WriteLine("Your Health: " + player.Health + "/" + player.MaxHealth);
            Console.WriteLine("Enemy Health: " + enemy.Health + "/" + enemy.MaxHealth);
            Console.WriteLine("Your Weapon: " + player.Weapon.Name);
            Console.WriteLine("Bonus Damage: " + player.BonusDamage);
            Console.WriteLine("Potions: " + player.Potions);
            Console.WriteLine("Enemy Weapon: " + enemy.Weapon.Name);
            Console.WriteLine();
        }

        private BattleAction GetPlayerAction()
        {
            while (true)
            {
                Console.Write("Choose an action - Attack (A), Block (B), Heal (H): ");
                string input = Console.ReadLine();

                if (input != null)
                {
                    input = input.Trim().ToUpper();
                }
                else
                {
                    input = "";
                }

                if (input == "A" || input == "ATTACK")
                {
                    return BattleAction.Attack;
                }
                else if (input == "B" || input == "BLOCK")
                {
                    return BattleAction.Block;
                }
                else if (input == "H" || input == "HEAL")
                {
                    if (player.Potions > 0)
                    {
                        return BattleAction.Heal;
                    }
                    else
                    {
                        Console.WriteLine("You do not have any potions.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid action.");
                }
            }
        }

        private BattleAction GetEnemyAction(Enemy enemy, bool isBoss)
        {
            if (enemy.Potions > 0 && enemy.Health <= enemy.MaxHealth / 3)
            {
                int healChance = random.Next(1, 101);
                if (healChance <= 40)
                {
                    return BattleAction.Heal;
                }
            }

            int blockChance;

            if (isBoss)
            {
                blockChance = 18;
            }
            else
            {
                blockChance = 24;
            }

            if (enemy.Weapon.Type == WeaponType.Bow || enemy.Weapon.Type == WeaponType.Wand)
            {
                blockChance = blockChance - 8;
            }

            if (enemy.Health < enemy.MaxHealth / 2)
            {
                blockChance = blockChance + 8;
            }

            int roll = random.Next(1, 101);

            if (roll <= blockChance)
            {
                return BattleAction.Block;
            }
            else
            {
                return BattleAction.Attack;
            }
        }

        private Enemy CreateEnemy(int level)
        {
            int classNumber = random.Next(1, 6);
            string className = "";
            Weapon weapon = null;

            if (classNumber == 1)
            {
                className = "Knight";
                weapon = new Weapon(WeaponType.Sword, "Sword", 5, 9);
            }
            else if (classNumber == 2)
            {
                className = "Vanguard";
                weapon = new Weapon(WeaponType.BattleAxe, "Battle Axe", 6, 10);
            }
            else if (classNumber == 3)
            {
                className = "Hunter";
                weapon = new Weapon(WeaponType.Bow, "Bow", 4, 10);
            }
            else if (classNumber == 4)
            {
                className = "Rogue";
                weapon = new Weapon(WeaponType.Dagger, "Dagger", 4, 9);
            }
            else
            {
                className = "Mage";
                weapon = new Weapon(WeaponType.Wand, "Wand", 5, 10);
            }

            string enemyName = enemyNames[random.Next(enemyNames.Length)];

            while (enemyName.ToLower().Contains(className.ToLower()) || className.ToLower().Contains(enemyName.ToLower()))
            {
                enemyName = enemyNames[random.Next(enemyNames.Length)];
            }

            int health = 22 + (level * 4) + random.Next(0, 5);
            Enemy enemy = new Enemy(enemyName, className, health, weapon);

            if (level >= 4)
            {
                enemy.BonusDamage = 1;
            }

            if (level >= 6)
            {
                enemy.Potions = 1;
            }

            return enemy;
        }

        private PlayerClass ChooseClass()
        {
            Console.WriteLine("Choose your class:");
            Console.WriteLine("1. Knight (Sword)");
            Console.WriteLine("2. Vanguard (Axe)");
            Console.WriteLine("3. Hunter (Bow)");
            Console.WriteLine("4. Rogue (Dagger)");
            Console.WriteLine("5. Mage (Wand)");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter 1 to 5: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    return new Knight();
                }
                else if (input == "2")
                {
                    return new Vanguard();
                }
                else if (input == "3")
                {
                    return new Hunter();
                }
                else if (input == "4")
                {
                    return new Rogue();
                }
                else if (input == "5")
                {
                    return new Mage();
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }

        private bool AskYesOrNo(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (input != null)
                {
                    input = input.Trim().ToLower();
                }
                else
                {
                    input = "";
                }

                if (input == "y" || input == "yes")
                {
                    return true;
                }
                else if (input == "n" || input == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please type y or n.");
                }
            }
        }

        private void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("========================================");
            Console.WriteLine("      ROGUE OF THE DARK AGES");
            Console.WriteLine("========================================");
            Console.ResetColor();
        }
    }
}
