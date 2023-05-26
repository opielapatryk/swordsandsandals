﻿namespace swordsandsandals
{
    public class Fight
    {
        #region FIELDS
        private Character myCharacter;
        private Enemy actualEnemy;
        private Arena mainArena;
        private Enemy otherEnemy;
        #endregion

        #region CONSTRUCTOR
        public Fight(Character myCharacter, Enemy actualEnemy, Arena mainArena, Enemy otherEnemy, int bounty, string winMess, bool isBoss)
        {
            this.myCharacter = myCharacter;
            this.actualEnemy = actualEnemy;
            this.mainArena = mainArena;
            this.otherEnemy = otherEnemy;

            Console.Clear();
            Console.WriteLine("Solve math operations to deal damage to the enemy.\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();

            BattleLoop(bounty, winMess, isBoss);
        }
        #endregion

        #region METHODS
        private void BattleLoop(int bounty, string winMess, bool isBoss)
        {
            MathProblemGenerator problemGenerator = new MathProblemGenerator(actualEnemy.NextNum, actualEnemy.NextNumSec);
            ConsoleInteraction consoleInteraction = new ConsoleInteraction();

            int enemyHP = actualEnemy.Health;
            int myHP = myCharacter.Health;

            while (myHP > 0 && enemyHP > 0)
            {
                Console.WriteLine("Boss: {0}\nHP: {1}\n-----------------------------\nYour HP: {2}\n-----------------------------", actualEnemy.Name, enemyHP, myHP);

                MathProblem mathProblem = problemGenerator.GenerateProblem();
                consoleInteraction.DisplayMathProblem(mathProblem);

                int answer = consoleInteraction.ReadAnswer();

                if (mathProblem.IsAnswerCorrect(answer))
                {
                    Console.WriteLine("Good job!");
                    enemyHP -= mathProblem.CorrectAnswer;
                }
                else
                {
                    Console.WriteLine("Wrong!\nCorrect answer: {0}", mathProblem.CorrectAnswer);
                    myHP -= mathProblem.CorrectAnswer;
                }

                consoleInteraction.ClearConsole();
            }

            if (myHP > 0)
            {
                myCharacter.Level += bounty;
                myCharacter.StatPoints += bounty;
                actualEnemy.NextNum += 1;
                actualEnemy.NextNumSec += 1;
                Console.WriteLine(winMess, myCharacter.Level);
                new Menu(myCharacter, mainArena, actualEnemy, otherEnemy);
            }
            else
            {
                consoleInteraction.DisplayLoseMessage();

                if (consoleInteraction.BackToMenu())
                {
                    new Menu(myCharacter, mainArena, actualEnemy, otherEnemy);
                }
                else if (isBoss)
                {
                    new Fight(myCharacter, actualEnemy, mainArena, otherEnemy, 1, "Congratulations! You won!\nYou have reached new lvl: {0} and gain one stat point.", true);
                }
                else
                {
                    new Fight(myCharacter, actualEnemy, mainArena, otherEnemy, 0, "Congratulations! You won!", false);
                }
            }
        }
        #endregion

    }
}
