#include "PuckManager.h"

PuckManager * PuckManager::mInstance = 0;

PuckManager::PuckManager( Ogre::SceneManager *sceneMan, Gui3D::Gui3D* gui3D, int pucks ) :
	mSceneMgr( sceneMan ), 
	mGui3D(gui3D),
	mPucks( pucks ) {	
	createPucks(pucks);	 
	createScorePanel();
}

void PuckManager::createPucks(int pucks) {
	int i = 0;
	for ( int i = 0; i < pucks ; i++ ) {
		if ( ( i % 2 ) == 0 ) {
			mFreePucks.push_back( new Puck( mSceneMgr, "Yellowstone" + Ogre::StringConverter::toString(i+1), Puck::YELLOWSTONE) );
		}
		else {
			mFreePucks.push_back( new Puck( mSceneMgr, "Redstone" + Ogre::StringConverter::toString(i+1), Puck::REDSTONE) );
		}
	}
}

void PuckManager::update( Ogre::Real const & timeSinceLastFrame ) {
	std::vector<Puck *>::iterator curr = mUsedPucks.begin();
	std::vector<Puck *>::iterator end = mUsedPucks.end();

	for ( ; curr != end ; ++curr ) {
		(*curr)->update( timeSinceLastFrame );
	}
}


// Get a puck from the free list.
// Warning...returns 0 if no more pucks available.
Puck * PuckManager::getNextPuck() {
	Puck * puck = 0;			// returns 0 if no free pucks available.
	
	int size;
	size = mFreePucks.size();
	if (size != 0)
	{
		puck = mFreePucks.at(mFreePucks.size() - 1);
		mUsedPucks.push_back(puck);
		mFreePucks.pop_back();
	}

	return puck;
}



std::vector<Puck *> & PuckManager::getUsedPucks() {
	return mUsedPucks;
}


PuckManager * PuckManager::instance(Ogre::SceneManager * sceneMgr, Gui3D::Gui3D* gui3D, int pucks ) {
	if (mInstance == 0)  {  
		mInstance = new PuckManager( sceneMgr, gui3D, pucks );
	}
    return mInstance;
}

Puck * PuckManager::getPuckByName(Ogre::String const & puckName) const
{
	std ::vector<Puck *>::const_iterator curr = mUsedPucks.begin();
	std::vector<Puck *>::const_iterator end = mUsedPucks.end();

	for ( ; curr != end; curr++)
	{
		if ( (*curr)->getEntity()->getName().compare( puckName ) == 0)
		{
			return *curr;
		}
	}

	//curr == end, i.e. no matching puck, so return null/
	return 0;
}
bool PuckManager::allPucksStopped() const
{
	std::vector<Puck *>::iterator curr = PuckManager::instance()->getUsedPucks().begin();
	std::vector<Puck *>::iterator end = PuckManager::instance()->getUsedPucks().end();

	for ( ; curr != end ; ++curr ) 
	{
		if ((*curr)->getVelocity() > 0)
			return false;
	}
	return true;
}

void PuckManager::createScorePanel()
{
	mPanel = new Gui3D::Panel(mGui3D, mSceneMgr, Ogre::Vector2(50, 30), 10, "purple", "puck_score_panel");
	mPanel->mNode->yaw(Ogre::Degree(180));//otherwise text faces the wrong way
	mPanel->hideInternalMousePointer();
	mCaption = mPanel->makeCaption(5, 5, 50, 30, "", Gorilla::TextAlign_Centre);
}


Gui3D::Panel* PuckManager::getScorePanel()
{
	return mPanel;
}
Gui3D::Caption* PuckManager::getScorePanelCaption()
{
	return mCaption;
}

//Puck * getNextPuckColour()
int PuckManager::getNextPuckColour()
{

	Puck * puck = 0;			// returns 0 if no free pucks available.
	
	int size;
	size = mFreePucks.size();
	if (size != 0)
	{
		puck = mFreePucks.at(mFreePucks.size() - 1);
	}
	return static_cast<int>(puck->getPuckType());
}


bool PuckManager::pucksRemaining() const {
	if(mFreePucks.size() != 0)
	return true;
	else return false;
}
bool PuckManager::isFreeListEmpty()
{
	return(mFreePucks.size() == 0);
}
void PuckManager::restartGame()
{
	//Puck * puck = 0;			
	//std::vector<Puck *> usedPucks = getUsedPucks();

	//std ::vector<Puck *>::iterator curr = usedPucks.begin();
	//std::vector<Puck *>::iterator end = usedPucks.end();

	//for( ; curr!= end; ++curr)
	//{
	//	mFreePucks.push_back((*curr));
	//	mUsedPucks.pop_back();
	//	(*curr)->setPosition(Ogre::Vector3(0, 1, 10));
	//	(*curr)->getNode()->setVisible(true);
	//}
	
	Puck * puck = 0;
	
	int size;
	size = mUsedPucks.size();
	//if (size != 0)
	//{
	//	puck = mUsedPucks.at(mUsedPucks.size() - 1);
	//	mFreePucks.push_back(puck);
	//	mUsedPucks.pop_back();
	//	puck->setVelocity(0);
	//	puck->setPosition(Ogre::Vector3(0, 1, 10));
	//}
	std::vector<Puck *> usedPucks = getUsedPucks();

	std ::vector<Puck *>::iterator curr = usedPucks.begin();
	std::vector<Puck *>::iterator end = usedPucks.end();
	//if(size!=0)
	//{
		for( ; curr!= end; ++curr)
		{
			//mFreePucks.push_back((*curr));
			//mUsedPucks.pop_back();
			//(*curr)->setVelocity(0);
			//(*curr)->setPosition(Ogre::Vector3(0, 1, 10));
			if (size != 0)
			{
				puck = mUsedPucks.at(mUsedPucks.size() - 1);
				mFreePucks.push_back(puck);
				mUsedPucks.pop_back();
				puck->setVelocity(0);
				puck->setPosition(Ogre::Vector3(0, 1, 10));
			}
		}
	//}

	//return puck;
	
}


	