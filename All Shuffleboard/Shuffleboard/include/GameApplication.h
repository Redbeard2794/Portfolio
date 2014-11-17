#ifndef GAMEAPPLICATION_H
#define GAMEAPPLICATION_H

#include <OIS/OIS.h>

#include "BaseApplication.h"
#include "GameConstants.h"
#include "Shuffleboard.h"
#include "PuckManager.h"
#include "Gui3D.h"
#include "Gui3DPanel.h"
#include "MyPurplePanelColors.h"
#include "AI.h"
#include "SoundManager.h"
#include "MyParticleSystem.h"
#include <stdlib.h>     /* srand, rand */

class GameApplication : public BaseApplication {
public:
	GameApplication();
	virtual ~GameApplication();
protected:
	virtual void createScene();
	virtual void destroyScene();
	virtual void createCamera();
	void resetCamera();
	bool frameRenderingQueued(const Ogre::FrameEvent& evt);
	
	// OIS::KeyListener
	bool keyPressed( const OIS::KeyEvent& evt );
	bool keyReleased( const OIS::KeyEvent& evt );

	// OIS::MouseListener
	bool mouseMoved( const OIS::MouseEvent& evt );
	bool mousePressed( const OIS::MouseEvent& evt, OIS::MouseButtonID id );
	bool mouseReleased( const OIS::MouseEvent& evt, OIS::MouseButtonID id );


	void createGamePanel();

	void createGameOverPanel();
	void createAIselectPanel();
	void createAvancedAIPanel();

private:	
	bool mShutDown;		
	bool mFreeLookCam;
	Puck * mPuck;
	Ogre::Real mElapsedTime;
	Ogre::Real mCurrentTime; 

	//Gui3D main object
	Gui3D::Gui3D* mGui3D;
	//the main panel (display in 3D)
	Gui3D::Panel* mPanel;
	Gui3D::ProgressBar* mForceProgressbar;
	MyPurplePanelColors mMyPurplePanelColors;

	bool mGetInputForce;//indicator for when input force is being applied

	Gui3D::Panel* gameOverPanel;
	

	Gui3D::Panel* AIselectPanel;
	Gui3D::Panel* advancedAIPanel;

	bool isAITurn;
	bool gameOver;

	SoundManager *mSoundMgr;
	int mySound1;
	int channel1;
	int mySound2;
	int channel2;

	MyParticleSystem * smoke;

	MyParticleSystem *emitters[2];

	MyParticleSystem *fireWorks;

	int currentState;
	int turnsTaken;

	bool AIEasyMode;
};

#endif 