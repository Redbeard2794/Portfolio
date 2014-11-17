#include "Shuffleboard.h"
#include <cstdlib>//this is for using rand() to generate random numbers

Shuffleboard * Shuffleboard::mInstance = 0;

//-------------------------------------------------------------------------------------
Shuffleboard::Shuffleboard( SceneManager *sceneMan ) :
	mSceneMgr( sceneMan ) {

	initialise();
}

//-------------------------------------------------------------------------------------
void Shuffleboard::initialise() {
	// Create the Shuffleboard.
	Entity * ent = mSceneMgr->createEntity("Shuffleboard", "Shuffleboard.mesh");
	SceneNode *node = mSceneMgr->getRootSceneNode()->createChildSceneNode("nodeShuffleboard");
	node->attachObject( ent );	
	node->setPosition( 0, 0, 0);

	//create the arrow
	ent = mSceneMgr->createEntity("Arrow", "arrow.mesh");
	mArrow = mSceneMgr->getRootSceneNode()->createChildSceneNode("nodeArrow");
	mArrow->attachObject(ent);
	mArrow->setPosition(5,4,15);
	mArrow->yaw(Ogre::Degree(90));

	mForce = 0;
	mUpdateScore = false;
}


//-------------------------------------------------------------------------------------
Shuffleboard * Shuffleboard::instance(SceneManager * sceneMgr ) {
	if (mInstance == 0)  {  
		mInstance = new Shuffleboard( sceneMgr );
	}
    return mInstance;
}

void Shuffleboard::setArrowVisible(bool visible)
{
	mArrow->setVisible(visible);
}

void Shuffleboard::rotateArrow(OIS::MouseEvent const & e)
{
	Ogre::Real xMovement = e.state.X.rel * 0.1f;
	Ogre::Vector3 directionFacing = mArrow->getOrientation() * Vector3::UNIT_X;
	Ogre::Vector3 straightAhead(0,0,-1);
	Ogre::Degree angle = directionFacing.angleBetween(straightAhead);
	if (angle < Ogre::Degree(GameConstants::MAX_ANGLE) || xMovement<0 || xMovement>0)
	{
		mArrow->yaw(Ogre::Degree(xMovement * -1));
	}
}

void Shuffleboard::launchPuck()
{
	Puck * puck = PuckManager::instance()->getNextPuck();
	setCurrentPuck(puck);

	//players turn
	if (puck != 0)
	{
		if(puck->getPuckType() == Puck::REDSTONE)
		{
			Ogre::Real centreX = ( GameConstants::MAX_X - GameConstants::MIN_X ) / 2 + GameConstants::MIN_X;
			Ogre::Vector3 launchPos( centreX, GameConstants::LAUNCH_Y, GameConstants::LAUNCH_Z );
			puck->activate(launchPos);
			//pucks direction is based on the arrows direction when it is launched
			puck->setDirection(mArrow->getOrientation() * Vector3::UNIT_X);
			//apply force from the progress bar when the player presses and holds space
			puck->applyForce(mForce);
			//score panel above puck
			Gui3D::Panel* panel = PuckManager::instance()->getScorePanel();
			panel->mNode->getParent()->removeChild(panel->mNode);
			mSceneMgr->getSceneNode("node" + puck->getName())->addChild(panel->mNode);
			Ogre::Vector3 pos = panel->mNode->getPosition();
			pos.y = 1;
			panel->mNode->setPosition(pos);
			updateCaption("0");
			//reset force and arrow orientation
			mForce = 0;
			resetArrowOrientation();

			mUpdateScore = true;
		}
		else if(puck->getPuckType() == Puck::YELLOWSTONE)
		{
			//if Basic AI
			if(AI::instance()->getEasy() == true)
			{
				Ogre::Real centreX = ( GameConstants::MAX_X - GameConstants::MIN_X ) / 2 + GameConstants::MIN_X;
				Ogre::Vector3 launchPos( centreX, GameConstants::LAUNCH_Y, GameConstants::LAUNCH_Z );
				puck->activate(launchPos);
				//get a random number and base the pucks direction on it
				srand (time(NULL));
				float x;
				x = rand() % 3 + 0;
				//so left(0), middle(1) or right(1)
				if(x == 0)
					puck->setDirection(Ogre::Vector3(-0.1f, 0, -1));
				else if (x == 1)
					puck->setDirection(Ogre::Vector3(0, 0, -1));
				else puck->setDirection(Ogre::Vector3(0.1f, 0, -1));
				//apply a random force between 4 and 5
				puck->applyForce(rand() % 5 + 4);
				//score panel above the puck
				Gui3D::Panel* panel = PuckManager::instance()->getScorePanel();
				panel->mNode->getParent()->removeChild(panel->mNode);
				mSceneMgr->getSceneNode("node" + puck->getName())->addChild(panel->mNode);
				Ogre::Vector3 pos = panel->mNode->getPosition();
				pos.y = 1;
				panel->mNode->setPosition(pos);
				updateCaption("0");

				mUpdateScore = true;
			}
			//Advanced AI
			else if(AI::instance()->getEasy() == false)
			{
				Ogre::Real centreX = ( GameConstants::MAX_X - GameConstants::MIN_X ) / 2 + GameConstants::MIN_X;
				Ogre::Vector3 launchPos( centreX, GameConstants::LAUNCH_Y, GameConstants::LAUNCH_Z );
				puck->activate(launchPos);

				//bool notInThree = true;
				//Ogre::Vector3 pDir;
				//Ogre::Vector3 pPuckPos;

				std::vector<Puck *> usedPucks = PuckManager::instance()->getUsedPucks();

				if(usedPucks.empty() == false)
				{
					//used to sort the pucks based on their z value
					// so this allows me to find the furthest puck down the board

					//keep a reference to the last used puck before sorting the list
					Puck * lastUsedPuck = usedPucks.at(usedPucks.size() - 1);
					//now sort the list
					sort(usedPucks.begin(), usedPucks.end(), PuckComparer());
					Puck::PuckType puckType = usedPucks.at(0)->getPuckType();

					std ::vector<Puck *>::iterator curr = usedPucks.begin();
					std::vector<Puck *>::iterator end = usedPucks.end();

					for( ; curr!= end && puckType == (*curr)->getPuckType() ; ++curr)
					{
						puckType = (*curr)->getPuckType();
					}
					//the furthest puck
					Puck * furthestPuck = usedPucks.at(0);
					//if the furthest puck is red, visible and in the '3' zone
					//the AI aims at it and knocks it off the end of the board
					//use's the arrow to aim at the puck
					if (furthestPuck->getPuckType() == Puck::REDSTONE && furthestPuck->isVisible() && furthestPuck->getPosition().z < GameConstants::SCORING_LINE_3)
					{
						Ogre::Vector3 newVect1 = furthestPuck->getNode()->getPosition() - puck->getNode()->getPosition();
						Ogre::Radian angle = Ogre::Math::ATan(newVect1.x / newVect1.z);
						mArrow->yaw(Ogre::Degree(angle));
						puck->setDirection(mArrow->getOrientation() * Vector3::UNIT_X);
						puck->applyForce(9);
					}
					//or if the furthest puck is in the centre of the board between
					//the '1' zone and '2' zone the AI will go out around it
					//either left or right
					else if(furthestPuck->getPosition().x >= (centreX - 1) && furthestPuck->getPosition().x <= (centreX + 1) && furthestPuck->getPosition().z < GameConstants::SCORING_LINE_1 && furthestPuck->getPosition().z > GameConstants::SCORING_LINE_3)
					{
						srand (time(NULL));
						float x;
						x = rand() % 1 + 0;
						if(x == 0)
							puck->setDirection(Ogre::Vector3(-0.07f, 0, -1));
						else if(x==1)
							puck->setDirection(Ogre::Vector3(0.07f, 0, -1));

							puck->applyForce(6.5);
					}
					//or the AI just scores 3
					else puck->applyForce(7);
				}
				//score panel above the puck
				Gui3D::Panel* panel = PuckManager::instance()->getScorePanel();
				panel->mNode->getParent()->removeChild(panel->mNode);
				mSceneMgr->getSceneNode("node" + puck->getName())->addChild(panel->mNode);
				Ogre::Vector3 pos = panel->mNode->getPosition();
				pos.y = 1;
				panel->mNode->setPosition(pos);
				updateCaption("0");

				resetArrowOrientation();

				mUpdateScore = true;
			}
		}
	}

}


bool Shuffleboard::getInputForce()
{
	bool getInputForce = true;

	mForce += (Ogre::Math::Pow(1.5f, mForce) * GameConstants::PUCK_FORCE_MULTIPLIER);

	if(mForce >= GameConstants::MAX_FORCE)
	{
		getInputForce = false;
		mForce = GameConstants::MAX_FORCE;
	}
	return getInputForce;
}
Ogre::Real Shuffleboard::getPuckForce()const
{
	return mForce;
}
void Shuffleboard::resetArrowOrientation()
{
	Ogre::Vector3 direction = mArrow->getOrientation() * Vector3::UNIT_X;
	Ogre::Quaternion quat = direction.getRotationTo(Ogre::Vector3(0,0,-1));
	mArrow->rotate(quat);
}

void Shuffleboard::updateCaption(Ogre::String const & str)
{
	Gui3D::Panel* panel = PuckManager::instance()->getScorePanel();
	Gui3D::Caption* caption = PuckManager::instance()->getScorePanelCaption();
	caption->text(str);
}

void Shuffleboard::updateScore()
{
	std::vector<Puck *> usedPucks = PuckManager::instance()->getUsedPucks();
	int score = 0;

	if(usedPucks.empty() == false)
	{
		//keep a reference to the last used puck before sorting the list
		Puck * lastUsedPuck = usedPucks.at(usedPucks.size() - 1);
		//now sort the list
		sort(usedPucks.begin(), usedPucks.end(), PuckComparer());
		Puck::PuckType puckType = usedPucks.at(0)->getPuckType();

		std ::vector<Puck *>::iterator curr = usedPucks.begin();
		std::vector<Puck *>::iterator end = usedPucks.end();

		for( ; curr!= end && puckType == (*curr)->getPuckType() ; ++curr)
		{
			score+= getScore( (*curr)->getPosition().z);
			puckType = (*curr)->getPuckType();
		}
		//update the score on the label
		updateCaption(Ogre::StringConverter::toString(score));
		Puck * furthestPuck = usedPucks.at(0);
		if (furthestPuck->getPuckType() != lastUsedPuck->getPuckType() || (lastUsedPuck->isVisible() == false && furthestPuck->isVisible()))
		{
			Gui3D::Panel* panel = PuckManager::instance()->getScorePanel();
			panel->mNode->getParent()->removeChild(panel->mNode);
			mSceneMgr->getSceneNode("node" + furthestPuck->getName() )->addChild(panel->mNode);
		}
		/*currentScore = score;*/
	}
	currentScore = score;
	mUpdateScore = false;
}
//gets the score based on the pucks position
//int getScore(Ogre::Real const & zPos);
int Shuffleboard::getScore(Ogre::Real const & zPos)
{
	int score = 0;

	if (zPos <= GameConstants::SCORING_LINE_1 && zPos> GameConstants::SCORING_LINE_2)
		score = 1;
	else if (zPos <= GameConstants::SCORING_LINE_2 && zPos > GameConstants::SCORING_LINE_3)
		score = 2;
	else if (zPos <= GameConstants::SCORING_LINE_3)
		score = 3;
	return score;
}

bool Shuffleboard::scoreNeedsUpdating()
{
	return mUpdateScore;
}

	/*Puck getCurrentPuck();*/
Puck * Shuffleboard::getCurrentPuck()
{
	return currPuck;
}

	/*void setCurrentPuck(Puck myPuck);*/
void Shuffleboard::setCurrentPuck(Puck * current)
{
	currPuck = current;
}
//gets the winning puck
Puck * Shuffleboard::getWinningPuck()
{
	std::vector<Puck *> usedPucks = PuckManager::instance()->getUsedPucks();
	Puck * lastUsedPuck = usedPucks.at(usedPucks.size() - 1);
	//now sort the list
	sort(usedPucks.begin(), usedPucks.end(), PuckComparer());

	Puck * furthestPuck = usedPucks.at(0);
	return furthestPuck;
}
