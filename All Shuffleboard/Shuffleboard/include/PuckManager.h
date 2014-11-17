#ifndef PUCKMANAGER_H
#define PUCKMANAGER_H

#include "Puck.h"
#include <vector>
#include "Gui3D.h"
#include "Gui3DPanel.h"
#include "MyPurplePanelColors.h"

using namespace std;

class PuckManager {
public:
	
	int getNextPuckColour();

	void update( Ogre::Real const & timeSinceLastFrame );
	
	Puck * getNextPuck();	

	std::vector<Puck *> & getUsedPucks();
	
	static PuckManager * instance(Ogre::SceneManager * sceneMgr = 0, Gui3D::Gui3D* gui3D = 0, int pucks = GameConstants::sNUMBER_PUCKS );

	Puck * getPuckByName(Ogre::String const & puckName) const;

	bool allPucksStopped() const;

	bool pucksRemaining() const;

	void createScorePanel();
	Gui3D::Panel* getScorePanel();
	Gui3D::Caption* getScorePanelCaption();

	
	bool isFreeListEmpty();

	void restartGame();


protected:
	PuckManager( Ogre::SceneManager *sceneMan, Gui3D::Gui3D* gui3D, int pucks );
	void createPucks(int pucks);

	// Member variables...

	Ogre::SceneManager *mSceneMgr;				// The overall scene manager.
	std::vector<Puck *> mFreePucks;				// A container for all the available pucks.
	std::vector<Puck *> mUsedPucks;				// A container for all the pucks currently in use.
	int mPucks;									// Total number of pucks.

	static PuckManager * mInstance;

	Gui3D::Gui3D* mGui3D;//the gui3D main object
	Gui3D::Panel* mPanel;//the panel to show the scores
	Gui3D::Caption* mCaption;//the caption for a panel
};



#endif

