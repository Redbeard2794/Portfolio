#ifndef SHUFFLEBOARD_H
#define SHUFFLEBOARD_H

#include <Ogre.h>
#include <OIS/OIS.h>
#include "GameConstants.h"
#include "PuckManager.h"
#include "PuckComparer.h"
#include<algorithm>
#include"Puck.h"
#include "GameApplication.h"
#include <stdlib.h>     /* srand, rand */

using namespace Ogre;

class Shuffleboard {
public:	
	
	static Shuffleboard * instance(SceneManager * sceneMgr = 0 );

	void initialise();	

	void setArrowVisible(bool visible);

	void rotateArrow(OIS::MouseEvent const & e);

	void Shuffleboard::launchPuck();

	bool getInputForce();

	Ogre::Real getPuckForce() const;

	void resetArrowOrientation();

	void updateCaption(Ogre::String const & str);

	void updateScore();

	int getScore(Ogre::Real const & zPos);

	bool scoreNeedsUpdating();

	Puck * getCurrentPuck();

	void setCurrentPuck(Puck * myPuck);

	Puck * getWinningPuck();
	int currentScore;

protected:
	Shuffleboard( SceneManager *sceneMan );

	SceneManager *mSceneMgr;		// The overall scene manager.
	
	static Shuffleboard * mInstance;

	Ogre::SceneNode *mArrow;

	//the input force to apply to the puck
	Ogre::Real mForce;

	bool mUpdateScore;

	Puck * currPuck;

	

};



#endif

