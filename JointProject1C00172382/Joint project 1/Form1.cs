//Name : Jason Murphy
//Student ID : C00172382
//Date started : 15/11/2012
//Date due : 13/12/2012
//Description of Task :
//
//We are to make a simlified version of the game robotron. The idea of this project is to demonstrate what we have learned so far this year.
//the player sprites is to move up and down with the sprite facing the corresponding direction and the player should also be able to shoot
//there should be visible collision detection between all entities
//other game entities (enemies, hostages) should move on their own. this is to be done by following the player or changing direction at different time intervals 
//or by following the hostage.
//there should be a display GUI that shows the players score and remaining lives
//the game must have a goal that makes it fun
//
//Description of this game
//
//my game is a classic shooting game based on old arcade games such as robotron albeit simplified
//the player can move around and shoot and the controls are shown before the game starts(extra function)
//there are 3 enemies and 1 hostage, which changes direction veery 4 seconds. 1 enemy will follow the hostage. if it gets the hostage you will
//lose points and the hostage will appear in a new place
//the other 2 enemies follow you, one of these will attempt to shoot you(extra function)
//if you get caught by the enemy you will lose a life and spawn in a random place
//if you get shot you will lose a life.
//you have 3 lives at the start which are shown by 3 picture boxes
//when you shoot an enemy you gain points and the enemy will appear in a new place
//there is a button that will increase the enemies speed(difficulty). this is extra functionality
//there is a button to start the game and when you run out of lives a restart button appears instead of
//the start button(also extra function)
//the game ends when the players score hits 200 or higher.
//
//Known Bugs : Enemy sprites will sometimes jump while moving up i.e. they will flicker between their left and right facing sprites
//the enemy bullet will follow the direction of the enemy that shoots if the player changes direction quick eneough
//This occurs because the enemy is following the player and the bullet takes its direction from the enemy so with a quick enough player the bullet turns around and moves the opposite way
//
//List of enemies : imp(brown) = fastest enemy.. also refered to as contact enemy
//enemyGun(red and brown) : slow moving enemy that shoots
//human(green) : medium moving enemy that chases the hostage
//
//hostage(blue) : medium moving, changes direction every 4 seconds
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace Joint_project_1
{
    public partial class Form1 : Form
    {
        //player images
        Image myImage;//right
        Image myImage2;//left
        Image myImage3;//up
        Image myImage4;//down
        //human enemy images
        Image humanImage;//up
        Image humanImage2;//left
        Image humanImage3;//down
        Image humanImage4;//right
        //player bullet
        Image playerBullet;
        //contact enemy
        Image contactEnemy;//up
        Image contactEnemy2;//left
        Image contactEnemy3;//down
        Image contactEnemy4;//right
        //shooting enemy
        Image enemyGun;//up
        Image enemyGun2;//left
        Image enemyGun3;//down
        Image enemyGun4;//right
        //player bullet variables : x, y, width, height, speed, direction, bulletAlive
        bool playerBulletAlive; //check if bullet alive
        int playerBulletSpeed = 4;
        int playerBulletX;
        int playerBulletY;
        int playerBulletWidth = 6;
        int playerBulletHeight = 6;
        int playerBulletDirection;
        //enemy bullet image
        Image enemyBullet;
        //enemy bullet variables : x, y, width, height, speed, direction, bulletAlive
        bool enemyBulletAlive;//check if bullet is alive
        int enemyBulletDirection;
        int enemyBulletX;
        int enemyBulletY;
        int enemyBulletSpeed = 4;
        int enemyBulletWidth = 6;
        int enemyBulletHeight = 6;
        //player variables  : x, y, width, height, speed, direction
        int playerX = 20;
        int playerY = 20;
        int playerHeight = 19;
        int playerWidth = 15;
        int playerDirection;
        //human enemy variables : x, y, width, height, speed, direction
        int humanX;
        int humanSpeed = 2;
        int humanY;
        int humanWidth = 25;
        int humanHeight = 26;
        int humanDirection;
        //game start check
        bool gameStart;//checks if game is atarted
        //(contact)imp enemy variables : x, y, width, height, speed, direction
        int impX;
        int impY;
        int impSpeed = 5;
        int impDirection;
        int impWidth = 27;
        int impHeight = 26;
        //(shooting)enemy gun variables : x, y, width, height, speed, direction
        int enemyGunX;
        int enemyGunY;
        int enemyGunWidth = 25;
        int enemyGunHeight = 32;
        int enemyGunSpeed = 2;
        int enemyGunDirection;
        //hostage images
        Image hostageLeft;//left
        Image hostageRight;//right
        Image hostageFront;//up
        Image hostageBack;//down
        //hostage variables : x, y, width, height, speed, direction, amount killed
        int hostageX;
        int hostageY;
        int hostageWidth = 25;
        int hostageHeight = 27;
        int hostageSpeed = 3;
        int hostageDirection;
        int hostagesLost = 0;
        //random generators
        Random randX = new Random();//for x values
        Random randY = new Random();//for y values
        Random randDir = new Random();//for setting direction
        //score & lives variables
        int previousScore = 0;
        int newScore;
        int previousLivesLeft = 3;
        int newLivesLeft;
        //timer for hostage direction change
        int time = 0;
        //for getting the enemy to shoot
        bool shoot = true;//used to set the bullets starting x and y values
        int shootTimer = 0;//timer for making the enemy shoot
        //sounds : extra function
        SoundPlayer gun = new SoundPlayer("Resources/GunShoot.WAV");//gun shot
        SoundPlayer hit = new SoundPlayer("Resources/Impact.WAV");//hit by bullet
        SoundPlayer taken = new SoundPlayer("Resources/hostageTook.WAV");//hostage talen
        SoundPlayer enemyShoot = new SoundPlayer("Resources/enemyShoot.WAV");//laser shot


        public Form1()
        {
            //possibly move into instance variables and get rid of current declarations above, might speed up load times
            InitializeComponent();
            //player
            myImage = Image.FromFile("Resources/newSnakeRight.PNG");//right
            myImage2 = Image.FromFile("Resources/newSnakeLeft.PNG");//left
            myImage3 = Image.FromFile("Resources/newSnakeUp.PNG");//up
            myImage4 = Image.FromFile("Resources/newSnakeDown.PNG");//down

            //human enemy
            humanImage = Image.FromFile("Resources/DoomMarineFront.PNG");//up
            humanImage2 = Image.FromFile("Resources/DoomMarineLeft.PNG");//left
            humanImage3 = Image.FromFile("Resources/DoomMarineBack.PNG");//down
            humanImage4 = Image.FromFile("Resources/DoomMarineRight.PNG");//right

            //players bullet
            playerBullet = Image.FromFile("Resources/PlayerBullet.PNG");
            //enemy bullet
            enemyBullet = Image.FromFile("Resources/enemyBullet.PNG");

            //contact enemy(imp)
            contactEnemy = Image.FromFile("Resources/DoomImpFront.PNG");//up
            contactEnemy2 = Image.FromFile("Resources/DoomImpLeft.PNG");//left
            contactEnemy3 = Image.FromFile("Resources/DoomImpBack.PNG");//down
            contactEnemy4 = Image.FromFile("Resources/DoomImpRight.PNG");//right

            //enemyGun
            enemyGun = Image.FromFile("Resources/DoomChaingunZombieFront.PNG");//up
            enemyGun2 = Image.FromFile("Resources/DoomChaingunZombieLeft.PNG");//left
            enemyGun3 = Image.FromFile("Resources/DoomChaingunZombieBack.PNG");//down
            enemyGun4 = Image.FromFile("Resources/DoomChaingunZombieRight.PNG");//right

            //hostage1
            hostageLeft = Image.FromFile("Resources/hostageLeft.PNG");//left
            hostageRight = Image.FromFile("Resources/hostageRight.PNG");//right
            hostageFront = Image.FromFile("Resources/hostageFront.PNG");//up
            hostageBack = Image.FromFile("Resources/hostageBack.PNG");//down
            
        }
        public void UpdateWorld()
        {
            if (previousScore >= 200)//end the game when score hits 200 or more
            {
                gameStart = false;
                lblWin.Visible = true;
                enemyBulletAlive = false;
                shootTimer = 0;
                lblHostagesLostIs.Visible = true;
                lblHostagesLost.Text = Convert.ToString(hostagesLost);
                lblHostagesLost.Visible = true;
                btnRestartGame.Visible = true;
            }
            time++;//timer for hostage to change direction
            if (time == 40)
            {
                hostageDirection = randDir.Next(1, 5);
                time = 0;
            }
            //timer for the enemy to shoot
            shootTimer++;
            //method calls
            humanEnemyMove();
            impEnemyMove();
            enemyGunMove();
            playerBulletMove();
            hostageMove1();
            EnemyGunCanShoot();
            enemyBulletMove();

            //collisions
            //player + imp
            if (DetectCollisionPlayerImp() == true)
            {
                if (gameStart == true)
                {
                    playerX = randX.Next(1, 300);
                    playerY = randY.Next(1, 300);
                    Lives();
                }
            }
            //hostage and human
            if (DetectCollisionHostageHuman() == true && gameStart == true)
            {
                newScore = previousScore - 1;
                lblScore.Text = Convert.ToString(newScore);
                previousScore = newScore;
                hostageX = randX.Next(150, 250);
                hostageY = randY.Next(100, 200);
                taken.Play();//plays sound
                hostagesLost++;
            }
            //player and enemyGun
            if (DetectCollisionPlayerEnemyGun() == true)
            {
                if (gameStart == true)
                {
                    playerX = 20;
                    playerY = 20;
                    Lives();
                }
            }
            //player bullet and imp
            if (DetectCollisionPlayerBulletImp() == true && gameStart == true)
            {
                impX = randX.Next(300, 600);
                impY = randY.Next(250, 500);
                newScore = previousScore + 10;
                lblScore.Text = Convert.ToString(newScore);
                previousScore = newScore;
                playerBulletAlive = false;
                hit.Play();//plays sound
            }
            //player bullet and enemyGun
            if (DetectCollisionPlayerBulletEnemyGun() == true && gameStart == true)
            {
                enemyGunX = randX.Next(300, 600);
                enemyGunY = randY.Next(250, 500);
                newScore = previousScore + 10;
                lblScore.Text = Convert.ToString(newScore);
                previousScore = newScore;
                playerBulletAlive = false;
                hit.Play();//plays sound
            }
            //player bullet and human
            if (DetectCollisionPlayerBulletHuman() == true && gameStart == true)
            {
                humanX = randX.Next(300, 600);
                humanY = randY.Next(250, 500);
                newScore = previousScore + 10;
                lblScore.Text = Convert.ToString(newScore);
                previousScore = newScore;
                playerBulletAlive = false;
                hit.Play();//plays sound
            }
            //enemy bullet and player
            if (DetectCollisionEnemyBulletPlayer() == true && gameStart == true)
            {
                playerX = randX.Next(10, 50);
                playerY = randY.Next(10, 50);
                newScore = previousScore - 5;
                lblScore.Text = Convert.ToString(newScore);
                previousScore = newScore;
                Lives();
                enemyBulletAlive = false;
                hit.Play();//plays sound
            }
            //player and human
            if (DetectCollisionPlayerHuman() == true)
            {
                if (gameStart == true)
                {
                    playerX = randX.Next(1, 300);
                    playerY = randY.Next(1, 300);
                    Lives();
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            //changes direction player sprite is facing depending on movement and draws the corresponding sprite
            if (picBxGameOver.Visible == false)
            {
                if (playerDirection == 1)//up
                {
                    g.DrawImage(myImage3, playerX - 5, playerY);
                }
                if (playerDirection == 2)//left
                {
                    g.DrawImage(myImage2, playerX, playerY);
                }

                if (playerDirection == 3)//down
                {
                    g.DrawImage(myImage4, playerX, playerY);
                }

                if (playerDirection == 4)//right
                {
                    g.DrawImage(myImage, playerX - 15, playerY);
                }
            }

            //checks if game has started before drawing enemy sprites
            if (gameStart == true)
            {
                //changes direction human enemy sprite is facing and draws the corresponding sprite
                if (humanDirection == 1)//up
                {
                    g.DrawImage(humanImage, humanX, humanY);
                }

                if (humanDirection == 2)//left
                {
                    g.DrawImage(humanImage2, humanX, humanY);
                }

                if (humanDirection == 3)//down
                {
                    g.DrawImage(humanImage3, humanX, humanY);
                }

                if (humanDirection == 4)//right
                {
                    g.DrawImage(humanImage4, humanX, humanY);
                }

                //changes direction imp is facing and draws the corresponding sprite
                if (impDirection == 1)//up
                {
                    g.DrawImage(contactEnemy, impX, impY);
                }
                if (impDirection == 2)//left
                {
                    g.DrawImage(contactEnemy2, impX, impY);
                }
                if (impDirection == 3)//down
                {
                    g.DrawImage(contactEnemy3, impX, impY);
                }
                if (impDirection == 4)//right
                {
                    g.DrawImage(contactEnemy4, impX, impY);
                }

                //changes direction enemyGun is facing and draws the corresponding sprite
                if (enemyGunDirection == 1)//up
                {
                    g.DrawImage(enemyGun, enemyGunX, enemyGunY);
                }
                if (enemyGunDirection == 2)//left
                {
                    g.DrawImage(enemyGun2, enemyGunX, enemyGunY);
                }
                if (enemyGunDirection == 3)//down
                {
                    g.DrawImage(enemyGun3, enemyGunX, enemyGunY);
                }
                if (enemyGunDirection == 4)//right
                {
                    g.DrawImage(enemyGun4, enemyGunX, enemyGunY);
                }

                //changes direction hostage1 is facing and draws the corresponding sprite
                if (hostageDirection == 4)//right
                {
                    g.DrawImage(hostageRight, hostageX, hostageY);
                }
                if (hostageDirection == 3)//dwon
                {
                    g.DrawImage(hostageBack, hostageX, hostageY);
                }
                if (hostageDirection == 2)//left
                {
                    g.DrawImage(hostageLeft, hostageX, hostageY);
                }
                if (hostageDirection == 1)//up
                {
                    g.DrawImage(hostageFront, hostageX, hostageY);
                }
            }
            //draws players bullet
            if (playerBulletAlive == true)
            {
                g.DrawImage(playerBullet, playerBulletX, playerBulletY);
            }
            //draws enemy bullet
            if (enemyBulletAlive == true)
            {
                g.DrawImage(enemyBullet, enemyBulletX, enemyBulletY);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //movement of player depending on what keys the user presses
            if (e.KeyCode == Keys.D)
            {
                playerDirection = 4;//right
                playerX = playerX + 7;
                if (playerX > this.Width)
                {
                    playerX = 0;
                }
            }

            if (e.KeyCode == Keys.A)
            {
                playerDirection = 2;//left
                playerX = playerX - 7; 
                if (playerX < 0)
                {
                    playerX = this.Width;
                }
            }

            if (e.KeyCode == Keys.S)
            {
                playerDirection = 3;//down
                playerY = playerY + 7;
                if (playerY > this.Height)
                {
                    playerY = 0;
                }
            }

            if (e.KeyCode == Keys.W)
            {
                playerDirection = 1;//up
                playerY = playerY - 7;
                if (playerY < 0)
                {
                    playerY = this.Height;
                }
            }
            //lets the player shoot
            if (e.KeyCode == Keys.F)
            {
                playerBulletX = playerX;
                playerBulletY = playerY; 
                playerBulletAlive = true;
                playerBulletMove();
                gun.Play();//plays sound
                if (playerDirection == 1)
                {
                    playerBulletDirection = 1;//up
                }
                if (playerDirection == 2)
                {
                    playerBulletDirection = 2;//left
                }
                if (playerDirection == 3)
                {
                    playerBulletDirection = 3;//down
                }
                if (playerDirection == 4)
                {
                    playerBulletDirection = 4;//right
                }
            }
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            //spawns enemies when pressed and sets their x and y values
            gameStart = true;               //starts the game
            humanX = randX.Next(300, 600);
            humanY = randY.Next(250, 500);
            impX = randX.Next(200, 570);
            impY = randY.Next(210, 450);
            enemyGunX = randX.Next(200, 550);
            enemyGunY = randY.Next(200, 400);
            hostageX = 300;
            hostageY = 250;
            lblStartMessage.Visible = false;
            //the tutorial is extra functionality
            lblTutorial.Visible = false;
            enemyBulletAlive = false;
            btnStartGame.Visible = false;
        }
        public void humanEnemyMove()
        {
            //makes human enemy follow hostage1 and sets its direction
            if (gameStart == true)
            {
                humanX = humanX + 1;
                if (hostageX < humanX)
                {
                    humanX = humanX - humanSpeed;
                    humanDirection = 2;//left
                }

                if (hostageX > humanX)
                {
                    humanX = humanX + humanSpeed;
                    humanDirection = 4;//right
                }

                if (hostageY < humanY)
                {
                    humanY = humanY - humanSpeed;
                    humanDirection = 3;//down
                }

                if (hostageY > humanY)
                {
                    humanY = humanY + humanSpeed;
                    humanDirection = 1;//up
                }
            }
        }
        public void impEnemyMove()
        {
            //makes contact enemy follow player and sets direction
            if (playerY > impY)
            {
                impY = impY + impSpeed;
                impDirection = 1;//up
            }
            else if (playerY < impY)
            {
                impY = impY - impSpeed;
                impDirection = 3;//down
            }
            if (playerX > impX)
            {
                impX = impX + impSpeed;
                impDirection = 4;//right
            }
            else if (playerX < impX)
            {
                impX = impX - impSpeed;
                impDirection = 2;//left
            }
        }
        public void enemyGunMove()
        {
            //make enemy gun follow player and sets direction of enemy and its bullet
            if (playerY > enemyGunY)
            {
                enemyGunY = enemyGunY + enemyGunSpeed;
                enemyGunDirection = 1;//up
            }
            //make enemy gun follow player and sets direction
            else if (playerY < enemyGunY)
            {
                enemyGunY = enemyGunY - enemyGunSpeed;
                enemyGunDirection = 3;//down
            }
            //make enemy gun follow player and sets direction
            if (playerX > enemyGunX)
            {
                enemyGunX = enemyGunX + enemyGunSpeed;
                enemyGunDirection = 4;//right
            }
            //make enemy gun follow player and sets direction
            else if (playerX < enemyGunX)
            {
                enemyGunX = enemyGunX - enemyGunSpeed;
                enemyGunDirection = 2;//left
            }
        }
        public void playerBulletMove()
        {
            //allows players bullet to move
            if (playerBulletAlive == true)
            {
                if (playerBulletDirection == 1)//up
                {
                    playerBulletY = playerBulletY - playerBulletSpeed;
                }
                if (playerBulletDirection == 2)//left
                {
                    playerBulletX = playerBulletX - playerBulletSpeed;
                }
                if (playerBulletDirection == 3)//down
                {
                    playerBulletY = playerBulletY + playerBulletSpeed;
                }
                if (playerBulletDirection == 4)//right
                {
                    playerBulletX = playerBulletX + playerBulletSpeed;
                }
            }
        }
        public void enemyBulletMove()
        {
            //method for the enemy bullet to move
            if (gameStart == true && enemyBulletAlive == true)
            {
                //sets the starting point for the bullet to be drawn at
                if (shoot == true)
                {
                    enemyBulletY = enemyGunY;
                    enemyBulletX = enemyGunX;
                    shoot = false;
                }

                //sets the bullets direction depending on the enemies direction
                if (enemyGunDirection == 1)//up
                {
                    enemyBulletDirection = 1;
                }
                if (enemyGunDirection == 2)//left
                {
                    enemyBulletDirection = 2;
                }
                if (enemyGunDirection == 3)//down
                {
                    enemyBulletDirection = 3;
                }
                if (enemyGunDirection == 4)//right
                {
                    enemyBulletDirection = 4;
                }

                //determines which way the bullet should move
                if (enemyBulletDirection == 1)//up
                {
                    enemyBulletY -=  enemyBulletSpeed;
                }
                else if (enemyBulletDirection == 2)//left
                {
                    enemyBulletX -= enemyBulletSpeed;
                }
                else if (enemyBulletDirection == 3)//down
                {
                    enemyBulletY += enemyBulletSpeed;
                }
                else if (enemyBulletDirection == 4)//right
                {
                    enemyBulletX += enemyBulletSpeed;
                }
            }
        }
        public void hostageMove1()
        {
            //movement of hostage1 depending on player direction
            if (gameStart == true)
            {
                {
                    if (hostageDirection == 1)//up
                    {
                        hostageY = hostageY + hostageSpeed;
                        if (hostageY == 0)
                        {
                            hostageY = 250;
                            time = 0;
                        }
                    }
                    if (hostageDirection == 2)//left
                    {
                        hostageX = hostageX - hostageSpeed; 
                        if (hostageX == 10)
                        {
                            hostageX = 350;
                            time = 0;
                        }
                    }
                    if (hostageDirection == 3)//down
                    {
                        hostageY = hostageY - hostageSpeed;
                        if (hostageY == 250)
                        {
                            hostageY = 50;
                            time = 0;
                        }
                    }
                    if (hostageDirection == 4)//right
                    {
                        hostageX = hostageX + hostageSpeed;
                        if (hostageX == this.Width)
                        {
                            hostageX = 0;
                            time = 0;
                        }
                    }
                }
            }
        }
        public bool DetectCollisionPlayerImp()
        {
            //collision detection between player and imp(contact enemy)

                if (playerX >= impX + impWidth)
                {
                    return false;
                }
                if (playerY + playerHeight <= impY)
                {
                    return false;
                }
                if (playerX + playerWidth <= impX)
                {
                    return false;
                }
                if (playerY >= impY + impHeight)
                {
                    return false;
                }
                    return true;
        }
        public bool DetectCollisionHostageHuman()//collision between human enemy and hostage
        {
            if (hostageX >= humanX + humanWidth)
            {
                return false;
            }
            if (hostageY + hostageHeight <= humanY)
            {
                return false;
            }
            if (hostageX + hostageWidth <= humanX)
            {
                return false;
            }
            if (hostageY >= humanY + humanHeight)
            {
                return false;
            }
            return true;
        }
        public bool DetectCollisionPlayerEnemyGun()//collision between player and enemyGun(shooter)
        {
            if (playerX >= enemyGunX + enemyGunWidth)
            {
                return false;
            }
            if (playerY + playerHeight <= enemyGunY)
            {
                return false;
            }
            if (playerX + playerWidth <= enemyGunX)
            {
                return false;
            }
            if (playerY >= enemyGunY + enemyGunHeight)
            {
                return false;
            }
            return true;
        }
        public bool DetectCollisionPlayerBulletImp()//collision between players bullet and imp enemy
        {
            if (playerBulletX >= impX + impWidth)
            {
                return false;
            }
            if (playerBulletY + playerBulletHeight <= impY)
            {
                return false;
            }
            if (playerBulletX + playerBulletWidth <= impX)
            {
                return false;
            }
            if (playerBulletY >= impY + impHeight)
            {
                return false;
            }
            return true;
        }
        public bool DetectCollisionPlayerBulletEnemyGun()//collision between players bullet and enemy gun
        {
            if (playerBulletX >= enemyGunX + enemyGunWidth)
            {
                return false;
            }
            if (playerBulletY + playerBulletHeight <= enemyGunY)
            {
                return false;
            }
            if (playerBulletX + playerBulletWidth <= enemyGunX)
            {
                return false;
            }
            if (playerBulletY >= enemyGunY + enemyGunHeight)
            {
                return false;
            }
            return true;
        }
        public bool DetectCollisionPlayerBulletHuman()//collision between players bullet and human enemy
        {
            if (playerBulletX >= humanX + humanWidth)
            {
                return false;
            }
            if (playerBulletY + playerBulletHeight <= humanY)
            {
                return false;
            }
            if (playerBulletX + playerBulletWidth <= humanX)
            {
                return false;
            }
            if (playerBulletY >= humanY + humanHeight)
            {
                return false;
            }
            return true;
        }
        public void Lives()//tracks how many lives are left and ends the game when there are none left
        {
            //calculations
            newLivesLeft = previousLivesLeft - 1;
            previousLivesLeft = newLivesLeft;
            //life display
            if (previousLivesLeft == 2)
            {
                picBxLives3.Visible = false;
            }
            if (previousLivesLeft == 1)
            {
                picBxLives2.Visible = false;
            }
            if (previousLivesLeft == 0)
            {
                picBxLives1.Visible = false;
            }
            lblLivesLeft.Text = Convert.ToString(newLivesLeft);
            if (previousLivesLeft == 0)
            {
                gameStart = false;
                btnRestart.Visible = true;
                picBxGameOver.Visible = true;
                lblHostagesLostIs.Visible = true;
                lblHostagesLost.Text = Convert.ToString(hostagesLost);
                lblHostagesLost.Visible = true;
            }

        }
        public bool DetectCollisionPlayerHuman()//collision between player and human enemy
        {
            if (playerX >= humanX + humanWidth)
            {
                return false;
            }
            if (playerY + hostageHeight <= humanY)
            {
                return false;
            }
            if (playerX + playerWidth <= humanX)
            {
                return false;
            }
            if (playerY >= humanY + humanHeight)
            {
                return false;
            }
            return true;
        }
        //collision between enemy bullet and player
        public bool DetectCollisionEnemyBulletPlayer()
        {
            if (enemyBulletX >= playerX + playerWidth)
            {
                return false;
            }
            if (enemyBulletY + enemyBulletHeight <= playerY)
            {
                return false;
            }
            if (enemyBulletX + enemyBulletWidth <= playerX)
            {
                return false;
            }
            if (enemyBulletY >= playerY + playerHeight)
            {
                return false;
            }
            return true;

        }

        //extra functionality
        private void btnRestart_Click(object sender, EventArgs e)//restarts the game
        {
            //shows how many lives the player has left
            picBxLives1.Visible = true;
            picBxLives2.Visible = true;
            picBxLives3.Visible = true;
            previousLivesLeft = 3;
            previousScore = 0;
            //game over screen
            picBxGameOver.Visible = false;
            //restart button
            btnRestart.Visible = false;
            //sets game start to true
            gameStart = true;
            //makes original start button invisible
            btnStartGame.Visible = false;
            //sets speeds back to original values
            impSpeed = 5;
            humanSpeed = 2;
            enemyGunSpeed = 4;
            hostageSpeed = 2;
            //makes warning invisible
            lblWarning.Visible = false;
            impX = 450;
            enemyGunX = 350;
            lblHostagesLostIs.Visible = false;
            hostagesLost = 0;
            lblHostagesLost.Visible = false;
            lblDifficulty.Text = "normal";

        }
        public void EnemyGunCanShoot()
        {
            //enemy can shoot when the timeer hits 40
            if (shootTimer == 40)
            {
                enemyBulletAlive = true;
                enemyShoot.Play();
                shoot = true;
                shootTimer = 0;
            }
        }
        //extra functionality
        private void btnDifficulty_Click(object sender, EventArgs e)
        {
            //increases speeds to make the game harder
            impSpeed = impSpeed + 1;
            humanSpeed = humanSpeed + 1;
            enemyGunSpeed = enemyGunSpeed + 1;
            hostageSpeed = hostageSpeed + 1;
            //difficulty display
            if (impSpeed == 7)
            {
                lblDifficulty.Text = "Hard";
            }
            if (impSpeed == 9)
            {
                lblDifficulty.Text = "Extreme";
            }
            if (impSpeed == 10)
            {
                lblDifficulty.Text = "Good Luck!";
                lblWarning.Visible = true;
            }
        }

        private void btnRestartGame_Click(object sender, EventArgs e)
        {
            //shows how many lives the player has left
            picBxLives1.Visible = true;
            picBxLives2.Visible = true;
            picBxLives3.Visible = true;
            previousLivesLeft = 3;
            previousScore = 0;
            //restart button
            btnRestartGame.Visible = false;
            //sets game start to true
            gameStart = true;
            //sets speeds back to original values
            impSpeed = 5;
            humanSpeed = 2;
            enemyGunSpeed = 4;
            hostageSpeed = 2;
            //makes warning invisible
            lblWarning.Visible = false;
            impX = 450;
            enemyGunX = 350;
            lblHostagesLostIs.Visible = false;
            hostagesLost = 0;
            lblHostagesLost.Visible = false;
            lblDifficulty.Text = "normal";
            lblWin.Visible = false;
        }
        }

    }


