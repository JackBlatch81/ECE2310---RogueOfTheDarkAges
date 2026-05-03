using System;
using System.Drawing;
using System.Windows.Forms;

namespace RogueOfTheDarkAges
{
    public class MainForm : Form
    {
        private Random random;
        private PlayerClass player;
        private Enemy currentEnemy;
        private int currentLevel;
        private bool bossBattle;
        private bool waitingForUpgrade;
        private bool gameOver;

        private string[] environments;
        private string[] enemyNames;

        private Label titleLabel;
        private Label setupHeaderLabel;
        private Label playerHeaderLabel;
        private Label enemyHeaderLabel;
        private Label actionsHeaderLabel;
        private Label upgradeHeaderLabel;
        private Label logHeaderLabel;
        private Label statusLabel;

        private Label classSelectLabel;
        private Label levelLabel;
        private Label classLabel;
        private Label playerHealthLabel;
        private Label weaponLabel;
        private Label bonusDamageLabel;
        private Label potionLabel;
        private Label enemyNameLabel;
        private Label enemyHealthLabel;
        private Label environmentLabel;

        private ProgressBar playerHealthBar;
        private ProgressBar enemyHealthBar;

        private ComboBox classSelector;
        private Button startButton;
        private Button restartButton;
        private Button attackButton;
        private Button blockButton;
        private Button healButton;
        private Button upgradeHealthButton;
        private Button upgradeDamageButton;
        private Button upgradePotionButton;

        private RichTextBox logBox;

        public MainForm()
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

            BuildInterface();
            ResetToMenu();
        }

        private void BuildInterface()
        {
            Text = "Rogue of the Dark Ages";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 820);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(20, 22, 28);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            titleLabel = new Label();
            titleLabel.Text = "ROGUE OF THE DARK AGES";
            titleLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            titleLabel.ForeColor = Color.Gold;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(24, 18);
            Controls.Add(titleLabel);

            statusLabel = new Label();
            statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            statusLabel.ForeColor = Color.LightSkyBlue;
            statusLabel.AutoSize = false;
            statusLabel.Location = new Point(28, 58);
            statusLabel.Size = new Size(1040, 24);
            Controls.Add(statusLabel);

            setupHeaderLabel = CreateSectionLabel("Game Setup", 28, 95);
            Controls.Add(setupHeaderLabel);

            classSelectLabel = new Label();
            classSelectLabel.Text = "Choose Class:";
            classSelectLabel.Location = new Point(30, 128);
            classSelectLabel.Size = new Size(100, 24);
            classSelectLabel.ForeColor = Color.WhiteSmoke;
            Controls.Add(classSelectLabel);

            classSelector = new ComboBox();
            classSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            classSelector.Location = new Point(135, 125);
            classSelector.Size = new Size(180, 28);
            classSelector.Items.Add("Knight");
            classSelector.Items.Add("Vanguard");
            classSelector.Items.Add("Hunter");
            classSelector.Items.Add("Rogue");
            classSelector.Items.Add("Mage");
            classSelector.SelectedIndex = 0;
            Controls.Add(classSelector);

            startButton = CreateButton("Start Game", 330, 123, 120, 34);
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            restartButton = CreateButton("Restart", 460, 123, 100, 34);
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);

            playerHeaderLabel = CreateSectionLabel("Player", 28, 180);
            Controls.Add(playerHeaderLabel);

            levelLabel = CreateInfoLabel(30, 214, 460);
            Controls.Add(levelLabel);

            classLabel = CreateInfoLabel(30, 244, 460);
            Controls.Add(classLabel);

            playerHealthLabel = CreateInfoLabel(30, 274, 460);
            Controls.Add(playerHealthLabel);

            playerHealthBar = new ProgressBar();
            playerHealthBar.Location = new Point(30, 304);
            playerHealthBar.Size = new Size(300, 18);
            playerHealthBar.Style = ProgressBarStyle.Continuous;
            Controls.Add(playerHealthBar);

            weaponLabel = CreateInfoLabel(30, 334, 460);
            Controls.Add(weaponLabel);

            bonusDamageLabel = CreateInfoLabel(30, 364, 460);
            Controls.Add(bonusDamageLabel);

            potionLabel = CreateInfoLabel(30, 394, 460);
            Controls.Add(potionLabel);

            enemyHeaderLabel = CreateSectionLabel("Enemy", 540, 180);
            Controls.Add(enemyHeaderLabel);

            enemyNameLabel = CreateInfoLabel(542, 214, 540);
            Controls.Add(enemyNameLabel);

            enemyHealthLabel = CreateInfoLabel(542, 244, 540);
            Controls.Add(enemyHealthLabel);

            enemyHealthBar = new ProgressBar();
            enemyHealthBar.Location = new Point(542, 274);
            enemyHealthBar.Size = new Size(300, 18);
            enemyHealthBar.Style = ProgressBarStyle.Continuous;
            Controls.Add(enemyHealthBar);

            environmentLabel = new Label();
            environmentLabel.Location = new Point(542, 304);
            environmentLabel.Size = new Size(540, 90);
            environmentLabel.ForeColor = Color.LightCyan;
            environmentLabel.AutoSize = false;
            Controls.Add(environmentLabel);

            actionsHeaderLabel = CreateSectionLabel("Actions", 28, 450);
            Controls.Add(actionsHeaderLabel);

            attackButton = CreateButton("Attack", 30, 485, 130, 42);
            attackButton.Click += AttackButton_Click;
            Controls.Add(attackButton);

            blockButton = CreateButton("Block", 180, 485, 130, 42);
            blockButton.Click += BlockButton_Click;
            Controls.Add(blockButton);

            healButton = CreateButton("Heal", 330, 485, 130, 42);
            healButton.Click += HealButton_Click;
            Controls.Add(healButton);

            upgradeHeaderLabel = CreateSectionLabel("Upgrade Choice", 28, 560);
            Controls.Add(upgradeHeaderLabel);

            upgradeHealthButton = CreateButton("+10 Max Health", 30, 595, 150, 42);
            upgradeHealthButton.Click += UpgradeHealthButton_Click;
            Controls.Add(upgradeHealthButton);

            upgradeDamageButton = CreateButton("+2 Damage", 190, 595, 130, 42);
            upgradeDamageButton.Click += UpgradeDamageButton_Click;
            Controls.Add(upgradeDamageButton);

            upgradePotionButton = CreateButton("+1 Potion", 330, 595, 130, 42);
            upgradePotionButton.Click += UpgradePotionButton_Click;
            Controls.Add(upgradePotionButton);

            logHeaderLabel = CreateSectionLabel("Battle Log", 560, 430);
            Controls.Add(logHeaderLabel);

            logBox = new RichTextBox();
            logBox.Location = new Point(560, 470);
            logBox.Size = new Size(520, 315);
            logBox.ReadOnly = true;
            logBox.Font = new Font("Consolas", 10F, FontStyle.Regular);
            logBox.BackColor = Color.FromArgb(12, 14, 18);
            logBox.ForeColor = Color.WhiteSmoke;
            logBox.BorderStyle = BorderStyle.FixedSingle;
            logBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            logBox.DetectUrls = false;
            Controls.Add(logBox);
        }

        private Label CreateSectionLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            label.ForeColor = Color.Goldenrod;
            label.Location = new Point(x, y);
            label.Size = new Size(280, 24);
            return label;
        }

        private Label CreateInfoLabel(int x, int y, int width)
        {
            Label label = new Label();
            label.Location = new Point(x, y);
            label.Size = new Size(width, 24);
            label.ForeColor = Color.WhiteSmoke;
            return label;
        }

        private Button CreateButton(string text, int x, int y, int width, int height)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, height);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(80, 90, 110);
            button.BackColor = Color.FromArgb(38, 43, 54);
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;
            return button;
        }

        private void ResetToMenu()
        {
            player = null;
            currentEnemy = null;
            currentLevel = 0;
            bossBattle = false;
            waitingForUpgrade = false;
            gameOver = false;

            levelLabel.Text = "Level: -";
            classLabel.Text = "Class: -";
            playerHealthLabel.Text = "Health: -";
            weaponLabel.Text = "Weapon: -";
            bonusDamageLabel.Text = "Bonus Damage: -";
            potionLabel.Text = "Potions: -";

            enemyNameLabel.Text = "Enemy: -";
            enemyHealthLabel.Text = "Enemy Health: -";
            environmentLabel.Text = "Environment: Choose a class and start a new run.";

            playerHealthBar.Value = 0;
            enemyHealthBar.Value = 0;

            logBox.Clear();
            AddHeaderLog("Welcome to Rogue of the Dark Ages.");
            AddInfoLog("Block reduces incoming damage to 25% and has a 25% chance to counter.");
            AddInfoLog("Choose a class, then press Start Game.");

            statusLabel.Text = "Ready to begin.";
            SetBattleButtons(false);
            SetUpgradeButtons(false);

            startButton.Enabled = true;
            classSelector.Enabled = true;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            player = CreateSelectedPlayerClass();
            player.Potions = 4;
            player.BonusDamage = 0;
            currentLevel = 1;
            bossBattle = false;
            waitingForUpgrade = false;
            gameOver = false;

            logBox.Clear();
            AddPlayerLog("You chose the " + player.ClassName + ".");
            AddInfoLog("Starting weapon: " + player.Weapon.Name);
            AddInfoLog(player.GetClassDescription());
            AddInfoLog("You start with 4 potions.");

            startButton.Enabled = false;
            classSelector.Enabled = false;

            StartCurrentLevel();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ResetToMenu();
        }

        private PlayerClass CreateSelectedPlayerClass()
        {
            string selectedClass = classSelector.SelectedItem.ToString();

            if (selectedClass == "Knight")
            {
                return new Knight();
            }
            else if (selectedClass == "Vanguard")
            {
                return new Vanguard();
            }
            else if (selectedClass == "Hunter")
            {
                return new Hunter();
            }
            else if (selectedClass == "Rogue")
            {
                return new Rogue();
            }
            else
            {
                return new Mage();
            }
        }

        private void StartCurrentLevel()
        {
            waitingForUpgrade = false;
            SetUpgradeButtons(false);
            SetBattleButtons(true);

            player.RestoreHealthToFull();

            if (currentLevel <= 6)
            {
                bossBattle = false;
                currentEnemy = CreateEnemy(currentLevel);
                string place = environments[random.Next(environments.Length)];

                AddLog("");
                AddHeaderLog("=== Level " + currentLevel + " ===");
                AddInfoLog("You arrive in " + place + ".");
                AddEnemyLog("A " + currentEnemy.Name + " " + currentEnemy.EnemyClassName + " appears with a " + currentEnemy.Weapon.Name + ".");
                environmentLabel.Text = "Environment: " + place;
                statusLabel.Text = "Choose an action: Attack, Block, or Heal.";
            }
            else
            {
                bossBattle = true;
                currentEnemy = new Enemy("Ancient Dragon", "Boss", 95, new Weapon(WeaponType.IgnitedClaws, "Ignited Claws", 10, 15));
                currentEnemy.IsBoss = true;
                currentEnemy.BonusDamage = 4;
                currentEnemy.Potions = 1;

                AddLog("");
                AddHeaderLog("=== Final Boss ===");
                AddDefeatLog("An Ancient Dragon flies down from the sky in a storm of fire and ash.");
                environmentLabel.Text = "Environment: a storm of fire and ash";
                statusLabel.Text = "Final Boss battle. Choose your action carefully.";
            }

            UpdateUi();
        }

        private void AttackButton_Click(object sender, EventArgs e)
        {
            TakeTurn(BattleAction.Attack);
        }

        private void BlockButton_Click(object sender, EventArgs e)
        {
            TakeTurn(BattleAction.Block);
        }

        private void HealButton_Click(object sender, EventArgs e)
        {
            TakeTurn(BattleAction.Heal);
        }

        private void TakeTurn(BattleAction playerAction)
        {
            if (player == null || currentEnemy == null || gameOver || waitingForUpgrade)
            {
                return;
            }

            AddLog("");
            AddHeaderLog(bossBattle ? "=== Boss Battle ===" : "=== Battle ===");

            BattleAction enemyAction = GetEnemyAction(currentEnemy, bossBattle);

            bool playerBlocked = playerAction == BattleAction.Block;
            bool enemyBlocked = enemyAction == BattleAction.Block;

            if (playerAction == BattleAction.Heal)
            {
                if (player.Potions > 0)
                {
                    int healAmount = player.UsePotion(random);
                    AddPlayerLog("You used a potion and healed " + healAmount + " health.");
                }
                else
                {
                    AddWarningLog("You do not have any potions.");
                }
            }
            else if (playerAction == BattleAction.Attack)
            {
                DoAttack(player, currentEnemy, enemyBlocked, true);
            }
            else
            {
                AddInfoLog("You brace for impact and prepare to block.");
            }

            if (!currentEnemy.IsAlive())
            {
                EndBattle(true);
                return;
            }

            if (enemyAction == BattleAction.Heal)
            {
                int healAmount = currentEnemy.UsePotion(random);
                AddEnemyLog("The " + currentEnemy.Name + " used a potion and healed " + healAmount + " health.");
            }
            else if (enemyAction == BattleAction.Attack)
            {
                DoAttack(currentEnemy, player, playerBlocked, false);
            }
            else
            {
                AddEnemyLog("The " + currentEnemy.Name + " prepares to block.");
            }

            if (!player.IsAlive())
            {
                EndBattle(false);
                return;
            }

            statusLabel.Text = "Choose your next action.";
            UpdateUi();
        }

        private void DoAttack(Character attacker, Character defender, bool defenderBlocked, bool playerIsAttacking)
        {
            int damage = attacker.Attack(random);
            bool critical = random.Next(1, 101) <= attacker.GetCriticalChance();

            if (critical)
            {
                damage += random.Next(3, 7);
            }

            int originalDamage = damage;
            bool countered = false;
            int counterChance = 25;

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

            defender.Health -= damage;

            if (defender.Health < 0)
            {
                defender.Health = 0;
            }

            string attackerName;
            string targetName;

            if (playerIsAttacking)
            {
                attackerName = "You";
                targetName = defender.Name;
            }
            else
            {
                attackerName = "The " + attacker.Name;
                targetName = "you";
            }

            if (defenderBlocked)
            {
                if (critical)
                {
                    if (playerIsAttacking)
                    {
                        AddPlayerLog(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                                     " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                    }
                    else
                    {
                        AddEnemyLog(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                                    " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                    }
                }
                else
                {
                    if (playerIsAttacking)
                    {
                        AddPlayerLog(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                                     " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                    }
                    else
                    {
                        AddEnemyLog(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                                    " for " + originalDamage + " damage, but it was blocked down to " + damage + ".");
                    }
                }
            }
            else
            {
                if (critical)
                {
                    if (playerIsAttacking)
                    {
                        AddPlayerLog(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                                     " and deals " + damage + " damage.");
                    }
                    else
                    {
                        AddEnemyLog(attackerName + " lands a critical hit with " + attacker.Weapon.Name +
                                    " and deals " + damage + " damage.");
                    }
                }
                else
                {
                    if (playerIsAttacking)
                    {
                        AddPlayerLog(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                                     " and deals " + damage + " damage.");
                    }
                    else
                    {
                        AddEnemyLog(attackerName + " attacks " + targetName + " with " + attacker.Weapon.Name +
                                    " and deals " + damage + " damage.");
                    }
                }
            }

            if (countered && defender.IsAlive())
            {
                attacker.Health -= originalDamage;

                if (attacker.Health < 0)
                {
                    attacker.Health = 0;
                }

                string counterAttackerName;
                string counterTargetName;

                if (playerIsAttacking)
                {
                    counterAttackerName = "The " + defender.Name;
                    counterTargetName = "you";
                    AddWarningLog(counterAttackerName + " counters and deals " + originalDamage + " damage to " + counterTargetName + "!");
                }
                else
                {
                    counterAttackerName = "You";
                    counterTargetName = attacker.Name;
                    AddVictoryLog(counterAttackerName + " counterattacks and deals " + originalDamage + " damage to " + counterTargetName + "!");
                }
            }

            UpdateUi();
        }

        private void EndBattle(bool playerWon)
        {
            SetBattleButtons(false);
            UpdateUi();

            if (playerWon)
            {
                if (bossBattle)
                {
                    AddVictoryLog("The dragon crashes to the ground. You win!");
                    AddVictoryLog("You defeated every enemy and slayed the Ancient Dragon!");
                    statusLabel.Text = "Victory. Press Restart to begin a new run.";
                    gameOver = true;
                }
                else
                {
                    AddVictoryLog("You defeated the " + currentEnemy.Name + ".");

                    if (currentLevel <= 6)
                    {
                        waitingForUpgrade = true;
                        SetUpgradeButtons(true);

                        if (currentLevel == 6)
                        {
                            statusLabel.Text = "Choose one final upgrade before the boss.";
                            AddLog("");
                            AddWarningLog("You survived the 6 levels. Choose one final upgrade before the Ancient Dragon.");
                        }
                        else
                        {
                            statusLabel.Text = "Choose one upgrade to continue.";
                            AddLog("");
                            AddWarningLog("Choose an upgrade.");
                        }
                    }
                }
            }
            else
            {
                if (bossBattle)
                {
                    AddDefeatLog("The dragon burns through your final defense.");
                }
                else
                {
                    AddDefeatLog("The " + currentEnemy.Name + " defeated you.");
                }

                AddDefeatLog("Your journey ends in the Dark Ages.");
                statusLabel.Text = "Defeat. Press Restart to try again.";
                gameOver = true;
                SetUpgradeButtons(false);
            }
        }

        private void UpgradeHealthButton_Click(object sender, EventArgs e)
        {
            if (!waitingForUpgrade || player == null)
            {
                return;
            }

            player.MaxHealth += 10;
            player.Health = player.MaxHealth;
            AddInfoLog("Your max health is now " + player.MaxHealth + ".");
            FinishUpgrade();
        }

        private void UpgradeDamageButton_Click(object sender, EventArgs e)
        {
            if (!waitingForUpgrade || player == null)
            {
                return;
            }

            player.BonusDamage += 2;
            player.Health = player.MaxHealth;
            AddInfoLog("Your bonus damage is now " + player.BonusDamage + ".");
            FinishUpgrade();
        }

        private void UpgradePotionButton_Click(object sender, EventArgs e)
        {
            if (!waitingForUpgrade || player == null)
            {
                return;
            }

            player.Potions += 1;
            player.Health = player.MaxHealth;
            AddInfoLog("You now have " + player.Potions + " potion(s).");
            FinishUpgrade();
        }

        private void FinishUpgrade()
        {
            waitingForUpgrade = false;
            SetUpgradeButtons(false);
            currentLevel++;
            StartCurrentLevel();
        }

        private void SetBattleButtons(bool enabled)
        {
            attackButton.Enabled = enabled;
            blockButton.Enabled = enabled;
            healButton.Enabled = enabled;
        }

        private void SetUpgradeButtons(bool enabled)
        {
            upgradeHealthButton.Enabled = enabled;
            upgradeDamageButton.Enabled = enabled;
            upgradePotionButton.Enabled = enabled;
        }

        private void UpdateUi()
        {
            if (player == null)
            {
                return;
            }

            levelLabel.Text = bossBattle ? "Level: Final Boss" : "Level: " + currentLevel;
            classLabel.Text = "Class: " + player.ClassName;
            playerHealthLabel.Text = "Health: " + player.Health + "/" + player.MaxHealth;
            weaponLabel.Text = "Weapon: " + player.Weapon.Name;
            bonusDamageLabel.Text = "Bonus Damage: " + player.BonusDamage;
            potionLabel.Text = "Potions: " + player.Potions;

            playerHealthBar.Maximum = Math.Max(1, player.MaxHealth);
            playerHealthBar.Value = Math.Max(0, Math.Min(player.Health, playerHealthBar.Maximum));

            if (currentEnemy != null)
            {
                enemyNameLabel.Text = "Enemy: " + currentEnemy.Name + " " + currentEnemy.EnemyClassName + " (" + currentEnemy.Weapon.Name + ")";
                enemyHealthLabel.Text = "Enemy Health: " + currentEnemy.Health + "/" + currentEnemy.MaxHealth;

                enemyHealthBar.Maximum = Math.Max(1, currentEnemy.MaxHealth);
                enemyHealthBar.Value = Math.Max(0, Math.Min(currentEnemy.Health, enemyHealthBar.Maximum));
            }
            else
            {
                enemyNameLabel.Text = "Enemy: -";
                enemyHealthLabel.Text = "Enemy Health: -";
                enemyHealthBar.Value = 0;
            }
        }

        private void AddLog(string text)
        {
            AddLog(text, Color.WhiteSmoke);
        }

        private void AddLog(string text, Color color)
        {
            logBox.SelectionStart = logBox.TextLength;
            logBox.SelectionLength = 0;
            logBox.SelectionColor = color;
            logBox.AppendText(text + Environment.NewLine);
            logBox.SelectionColor = logBox.ForeColor;
            logBox.ScrollToCaret();
        }

        private void AddHeaderLog(string text)
        {
            AddLog(text, Color.Gold);
        }

        private void AddPlayerLog(string text)
        {
            AddLog(text, Color.LightGreen);
        }

        private void AddEnemyLog(string text)
        {
            AddLog(text, Color.Salmon);
        }

        private void AddInfoLog(string text)
        {
            AddLog(text, Color.LightSkyBlue);
        }

        private void AddWarningLog(string text)
        {
            AddLog(text, Color.Khaki);
        }

        private void AddVictoryLog(string text)
        {
            AddLog(text, Color.MediumSpringGreen);
        }

        private void AddDefeatLog(string text)
        {
            AddLog(text, Color.OrangeRed);
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
                blockChance -= 8;
            }

            if (enemy.Health < enemy.MaxHealth / 2)
            {
                blockChance += 8;
            }

            int roll = random.Next(1, 101);

            if (roll <= blockChance)
            {
                return BattleAction.Block;
            }

            return BattleAction.Attack;
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
                weapon = new Weapon(WeaponType.Dagger, "Daggers", 4, 9);
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
    }
}