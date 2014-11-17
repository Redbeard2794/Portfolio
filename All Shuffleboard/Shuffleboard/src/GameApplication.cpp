#include "GameApplication.h"



using namespace Ogre;


//-------------------------------------------------------------------------------------
GameApplication::GameApplication() :     	
	mShutDown( false ),
	mElapsedTime(0),
	mCurrentTime(0)
{
	mFreeLookCam = false;
	mGetInputForce = false;
	isAITurn = false;
	gameOver = false;
	currentState = GameConstants::SELECTAI;
	turnsTaken = 0;
}

//-------------------------------------------------------------------------------------
GameApplication::~GameApplication() {
	
}

//-------------------------------------------------------------------------------------
void GameApplication::createCamera() {
	// Create the camera here...
	mCamera = mSceneMgr->createCamera("BoardCamera");
	mCamera->setNearClipDistance(1);
	mCamera->setPosition(5,12,21);
	mCamera->pitch(Ogre::Degree(-35));
	mCameraMan = new OgreBites::SdkCameraMan(mCamera);
	Ogre::Real speed = mCameraMan->getTopSpeed();
	mCameraMan->setTopSpeed(speed/4);
	// mCameraMan->setStyle(SdkCameraMan::CS_FREELOOK);
	
}

//-------------------------------------------------------------------------------------   
void GameApplication::createScene() {
	// Create your scene here...	
	//light
	mSceneMgr->setAmbientLight(Ogre::ColourValue(0.5, 0.5, 0.5));
	Ogre::Light* light = mSceneMgr->createLight("MainLight");
	light->setPosition(15,15,3);


    // Create a plane with a lava texture
 	Ogre::Plane plane;
    plane.normal = Ogre::Vector3::UNIT_Y;
	// horizontal plane with normal up in y-direction
   	plane.d = 1;
    Ogre::MeshManager::getSingleton().createPlane("floor",Ogre::ResourceGroupManager::DEFAULT_RESOURCE_GROUP_NAME, plane,4500,4500,10,10,true,1,10,10,Ogre::Vector3::UNIT_Z);            
   	Ogre::Entity* pPlaneEnt = mSceneMgr->createEntity("plane", "floor");
	pPlaneEnt->setMaterialName("Examples/Lava");
   	pPlaneEnt->setCastShadows(false);
	Ogre::SceneNode* floorNode = mSceneMgr->createSceneNode("floor1");
	mSceneMgr->getRootSceneNode()->addChild(floorNode);
	floorNode->attachObject(pPlaneEnt);
	//ogre head at end of baord
	Ogre::Entity* ogreHead = mSceneMgr->createEntity("Head", "ogrehead.mesh");
    Ogre::SceneNode* headNode = mSceneMgr->getRootSceneNode()->createChildSceneNode();
    headNode->attachObject(ogreHead);
	headNode->setPosition(Ogre::Vector3(5,1,-30));
	headNode->setScale(0.2f, 0.2f,0.2f);
	//particle systems(one either side of ogre head)
	MyParticleSystem *emitters[2];// = new MyParticleSystem[3](mSceneMgr);
	for(int i =0;i< 2;i++)
	{
		emitters[i] = new MyParticleSystem(mSceneMgr, i, "Examples/JetEngine2", 1);
		if(i==0)
		{
			emitters[i]->setPosition(Ogre::Vector3(-8, 8, -20));
			emitters[i]->getNode()->setVisible(true);
		}
		else if(i == 1)
		{
			emitters[i]->setPosition(Ogre::Vector3(16, 8, -20));
			emitters[i]->getNode()->setVisible(true);
		}
	}
	//smoke/fire that comes from the ogres mouth
	smoke = new MyParticleSystem(mSceneMgr, 10, "Examples/JetEngine1", 4);//Examples/PurpleFountain
	smoke->setPosition(Ogre::Vector3(5, 7, -60));//-40
	smoke->getNode()->pitch(Degree(-90));				
	smoke->getNode()->setVisible(false);
	//rocks that are dotted around the board
	Ogre::Entity* rock1 = mSceneMgr->createEntity("rock1", "Cylinder.mesh");
	Ogre::SceneNode* rock1Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock1Node->attachObject(rock1);
	rock1Node->setPosition(Ogre::Vector3(-3,1.6f,-10));
	rock1->setMaterialName("Examples/VolcanicRock");

	Ogre::Entity* rock2 = mSceneMgr->createEntity("rock2", "Cube.001.mesh");
	Ogre::SceneNode* rock2Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock2Node->attachObject(rock2);
	rock2Node->setPosition(Ogre::Vector3(13,1.8f,-1.5f));
	rock2->setMaterialName("Examples/VolcanicRock");

	Ogre::Entity* rock3 = mSceneMgr->createEntity("rock3", "rock3.mesh");
	Ogre::SceneNode* rock3Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock3Node->attachObject(rock3);
	rock3Node->setPosition(Ogre::Vector3(-6,2,0));
	rock3->setMaterialName("Examples/VolcanicRock");

	Ogre::Entity* rock4 = mSceneMgr->createEntity("rock4", "rock3.mesh");
	Ogre::SceneNode* rock4Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock4Node->attachObject(rock4);
	rock4Node->setPosition(Ogre::Vector3(-1,1.6f,22));
	rock4->setMaterialName("Examples/VolcanicRock");

	Ogre::Entity* rock5 = mSceneMgr->createEntity("rock5", "Cube.001.mesh");
	Ogre::SceneNode* rock5Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock5Node->attachObject(rock5);
	rock5Node->setPosition(Ogre::Vector3(6,1.6f,25));
	rock5->setMaterialName("Examples/VolcanicRock");

	Ogre::Entity* rock6 = mSceneMgr->createEntity("rock6", "Cylinder.mesh");
	Ogre::SceneNode* rock6Node = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	rock6Node->attachObject(rock6);
	rock6Node->setPosition(Ogre::Vector3(13,1.6f,2.5f));
	rock6->setMaterialName("Examples/VolcanicRock");
	//rocky platform for the board to rest on
	Ogre::Entity* platform = mSceneMgr->createEntity("platform", "platform.mesh");
	Ogre::SceneNode* platformNode = mSceneMgr->getRootSceneNode()->createChildSceneNode();
	platformNode->attachObject(platform);
	platformNode->setPosition(Ogre::Vector3(5,1,1));
	platform->setMaterialName("Examples/VolcanicRock");
	platformNode->setScale(4.5f,0,8);

	//gui3d and calls to methods to make various panels used throughout the game
	mGui3D = new Gui3D::Gui3D(&mMyPurplePanelColors);
	Ogre::Viewport *viewport = mWindow->getViewport(0);
	mGui3D->createScreen(viewport, "purple", "mainScreen");
	createGamePanel();
	//2 panels for AI selection.(swaps between them depending on currently selected AI)
	createAIselectPanel();
	createAvancedAIPanel();
	createGameOverPanel();
	gameOverPanel->mNode->setVisible(false);

	//puck manager singleton
	PuckManager::instance(mSceneMgr, mGui3D, 10);

	//shuffleboard singleton
	Shuffleboard::instance(mSceneMgr);

	AIEasyMode = true;//true->easy mode. false->hardMode
	//setting the default AI selection for start up
	AIselectPanel->mNode->setVisible(true);
	advancedAIPanel->mNode->setVisible(false);

	//sets up a sound manager
	mSoundMgr = new SoundManager;
	mSoundMgr->Initialize();
	//sets up 2 sounds and 2 channels
	mySound1 = mSoundMgr->CreateStream(Ogre::String("puck slide.wav") );
	channel1 = 0;
	mySound2 = mSoundMgr->CreateStream(Ogre::String("puck collide.wav") );
	channel2 = 1;

	// Create a skybox for the background sky
	mSceneMgr->setSkyBox(true, "Examples/StormySkyBox", 5000, false);
}


//-------------------------------------------------------------------------------------   
void GameApplication::destroyScene() {

}

// KeyListener
bool GameApplication::keyPressed(const OIS::KeyEvent &e) {		
			Ogre::Real centreX = ( GameConstants::MAX_X - GameConstants::MIN_X ) / 2 + GameConstants::MIN_X;
		Ogre::Vector3 launchPos( centreX, GameConstants::LAUNCH_Y, GameConstants::LAUNCH_Z );
	switch (e.key) {
	case OIS::KC_ESCAPE: 
		mShutDown = true;
		break;

	case OIS::KC_T:
		if (mFreeLookCam)
		{
			mFreeLookCam = false;
			resetCamera();
		}
		else
		{
			mFreeLookCam = true;
		}
		break;
	case OIS::KC_SLASH:
		//AIEasyMode = false;//true->easy mode. false->hardMode
		//switches whether the AI is basic or advanced
		if(AIEasyMode == false)
			AIEasyMode = true;
		else if (AIEasyMode == true)
			AIEasyMode = false;
		//alternates between the corresponding panels based on current AI difficulty
		if(AIEasyMode == true)
		{
			AIselectPanel->mNode->setVisible(true);
			advancedAIPanel->mNode->setVisible(false);
		}
		else if(AIEasyMode == false)
		{
			AIselectPanel->mNode->setVisible(false);
			advancedAIPanel->mNode->setVisible(true);
		}
		break;
	case OIS::KC_RSHIFT:
		//creates the AI with the chosen difficulty and starts the game
		AI::instance(AIEasyMode);
		currentState = GameConstants::PLAY;
		break;
	case OIS::KC_R:
		if(currentState = GameConstants::GAMEOVER)
		{
			isAITurn = false;
			//this next part causes a crash. Not sure why(yet!)
			PuckManager::instance()->restartGame();
			currentState = GameConstants::PLAY;
		}
		break;
	case OIS::KC_SPACE:
		if(currentState == GameConstants::PLAY)
		{
			if(PuckManager::instance()->allPucksStopped() && isAITurn == false)
			{
				Shuffleboard::instance()->setArrowVisible(false);
				mGetInputForce = true;
			
			}
		}
		break;
	default:
		break;
	}
	
	mCameraMan->injectKeyDown(e);
	return true;
}

bool GameApplication::keyReleased(const OIS::KeyEvent &e) {
	mCameraMan->injectKeyUp(e);
		switch(e.key)
	{
	case OIS::KC_SPACE:
		if(currentState == GameConstants::PLAY)
		{
			GameApplication::mGetInputForce = false;
			mForceProgressbar->reset();
			if(PuckManager::instance()->allPucksStopped())
			{
				//launches the next puck
				Shuffleboard::instance()->launchPuck();
				//plays the sound for the puck sliding along the board
				mSoundMgr->PlaySound(mySound1, Shuffleboard::instance()->getCurrentPuck()->getNode(), &channel1);
				//tracks the number of turns
				turnsTaken++;
			}
		}
		break;
	default:
		break;
	}
	return true;
}
//for rotating with the mouse
bool GameApplication::mouseMoved(const OIS::MouseEvent &e) {					
     //mCameraMan->injectMouseMove(e);//for rotating with the mouse
	 Shuffleboard::instance()->rotateArrow(e);
	return true;
}

bool GameApplication::mousePressed( const OIS::MouseEvent& evt, OIS::MouseButtonID id ){
	return true;
}

bool GameApplication::mouseReleased( const OIS::MouseEvent& evt, OIS::MouseButtonID id ){
	return true;
}


//-------------------------------------------------------------------------------------
bool GameApplication::frameRenderingQueued(const Ogre::FrameEvent& evt)
{		
	if (mWindow->isClosed()) { 
		return false;
	}
	if (mShutDown) {
		return false;
	}
	
	mKeyboard->capture();
	mMouse->capture();
	
	mTrayMgr->frameRenderingQueued(evt);

	// For free look camera style..
	mCameraMan->frameRenderingQueued(evt);

	if (!mFreeLookCam)
	{
		mCameraMan->manualStop();
	}

	mElapsedTime = evt.timeSinceLastFrame;
	mCurrentTime += evt.timeSinceLastFrame;

	if(currentState == GameConstants::SELECTAI) 
	{
		AIselectPanel->mNode->setVisible(true);
		mPanel->mNode->setVisible(false);
		smoke->getNode()->setVisible(true);
	}
	else if(currentState == GameConstants::PLAY)
	{//set AI select panels to not be visible and make the game panel visible
		mPanel->mNode->setVisible(true);
		AIselectPanel->mNode->setVisible(false);
		smoke->getNode()->setVisible(false);
		advancedAIPanel->mNode->setVisible(false);
		if (mGetInputForce)
		{
			mGetInputForce = Shuffleboard::instance()->getInputForce();
			mForceProgressbar->setValue(Shuffleboard::instance()->getPuckForce() / GameConstants::MAX_FORCE);
		}

		if( mCurrentTime > GameConstants::UPDATE_GRANULARITY)
		{
			mCurrentTime -= GameConstants::UPDATE_GRANULARITY;
			PuckManager::instance()->update(evt.timeSinceLastFrame);
		}
		//if all pucks are stopped reset the camera, make the arrow visible, 
		//stop all sounds and update score if it needs to be updated
		if (PuckManager::instance()->allPucksStopped() == true)
		{
			Shuffleboard::instance()->setArrowVisible(true);
			mPanel->mNode->setVisible(true);
			resetCamera();
			mSoundMgr->StopAllSounds();
			if (Shuffleboard::instance()->scoreNeedsUpdating())
			{
				Shuffleboard::instance()->updateScore();
			}
		}
		//if there is a puck moving, moves the camera to the foul line and has 
		//it focus on the puck(panning camera)
		else if (PuckManager::instance()->allPucksStopped() ==false)
		{
			Shuffleboard::instance()->setArrowVisible(false);
			mPanel->mNode->setVisible(false);
			mCamera->setPosition(8,4,GameConstants::FOUL_LINE);
			Puck * puck = Shuffleboard::instance()->getCurrentPuck();
			mCamera->lookAt(puck->getPosition());
		}
		//detects if the game is over and changes the game state to GAMEOVER
		if(turnsTaken == GameConstants::MAXTURNS && PuckManager::instance()->allPucksStopped() == true && isAITurn == false)
		{
			currentState = GameConstants::GAMEOVER;
			//determines whether the player or AI won and their score
			if(Shuffleboard::instance()->getWinningPuck()->getPuckType() == Puck::YELLOWSTONE)
				gameOverPanel->makeTextZone(10, 50, 420, 30, "Winner = AI");
			else gameOverPanel->makeTextZone(10, 50, 420, 30, "Winner = Player");
			//plays a sound a different sound based on whether the player or Ai won
			int mySound1 = SoundManager::getSingleton().CreateStream(Ogre::String("Evil Laugh Sound Effect.wav") );
			int mySound2 = SoundManager::getSingleton().CreateStream(Ogre::String("PokemonRSE win sound.wav"));
			int channel1 = 1;
			if(Shuffleboard::instance()->getWinningPuck()->getPuckType() == Puck::YELLOWSTONE)
				mSoundMgr->PlaySound(mySound1, gameOverPanel->mNode, &channel1);
			else mSoundMgr->PlaySound(mySound2, gameOverPanel->mNode, &channel1); 
			
			gameOverPanel->makeTextZone(10, 130, 420, 30, "Winning score: " + Ogre::StringConverter::toString(Shuffleboard::instance()->currentScore));
			isAITurn = false;
		}
		//used to determine if it is the AI's turn and then tells it to take its turn
		if ( PuckManager::instance()->allPucksStopped() &&PuckManager::instance()->isFreeListEmpty() == false && gameOver == false) 
		{
			if(PuckManager::instance()->getNextPuckColour() == Puck::YELLOWSTONE)
			{
				//Then it is the AI’s turn
				isAITurn = true;

				// Now calls the AI's take turn method which in turn calls the launchPuck method in shuffleboard
				AI::instance()->takeTurn();
				mSoundMgr->PlaySound(mySound1, Shuffleboard::instance()->getCurrentPuck()->getNode(), &channel1);
		
				// Toggle isAITurn back to false
				isAITurn = false;
				turnsTaken++;
			}
		}
	}

	else if(currentState == GameConstants::GAMEOVER)
	{
		mPanel->mNode->setVisible(false);
		smoke->getNode()->setVisible(true);
		gameOverPanel->mNode->setVisible(true);
	}
	return true;
}
//resets the camera to its starting position and orientation
void GameApplication::resetCamera()
{
	mCamera->setPosition(Ogre::Vector3(5,12,21));
	mCamera->lookAt(5,12,0);
	mCamera->pitch(Ogre::Degree(-35));
}

//methods for creating each gui3D panel used in the game
void GameApplication::createGamePanel()
{
	mPanel = new Gui3D::Panel(mGui3D, mSceneMgr, Ogre::Vector2(400,100), 10, "purple", "test_panel");
	mPanel->makeCaption(5, -30, 390, 30, "Launch Power");
	mPanel->hideInternalMousePointer(); 
	mPanel->mNode->setPosition(10, GameConstants::LAUNCH_Y + 6, GameConstants::LAUNCH_Z - 7);
	mForceProgressbar = mPanel->makeProgressBar(10, 40, 380, 30);
}

void GameApplication::createGameOverPanel()
{
	gameOverPanel = new Gui3D::Panel(mGui3D, mSceneMgr, Ogre::Vector2(440,300), 10, "purple", "game_over_panel");
	gameOverPanel->makeCaption(10, 30, 420, 30, "Game Over");
	gameOverPanel->makeTextZone(10, 210, 420, 30, "Press 'R' to restart (with the same AI type)");
	gameOverPanel->hideInternalMousePointer();
	gameOverPanel->mNode->setPosition(5.1, GameConstants::LAUNCH_Y + 5, GameConstants::LAUNCH_Z - 7);
}

void GameApplication::createAIselectPanel()
{
	if(currentState == GameConstants::SELECTAI)
	{
		AIselectPanel = new Gui3D::Panel(mGui3D, mSceneMgr, Ogre::Vector2(440,180), 10, "purple", "select_ai_panel");
		AIselectPanel->makeCaption(10, 30, 420, 30, "AI selection");
		AIselectPanel->makeTextZone(10, 50, 420, 30, "Press the '/' key to change AI type");
		AIselectPanel->makeTextZone(10, 90, 420, 30, "Basic");
		AIselectPanel->makeTextZone(10, 130, 420, 30, "Press right shift to confirm");
		AIselectPanel->hideInternalMousePointer();
		AIselectPanel->mNode->setPosition(5.1, GameConstants::LAUNCH_Y + 5, GameConstants::LAUNCH_Z - 7);
	}
}
void GameApplication::createAvancedAIPanel()
{
	if(currentState == GameConstants::SELECTAI)
	{
		advancedAIPanel = new Gui3D::Panel(mGui3D, mSceneMgr, Ogre::Vector2(440,180), 10, "purple", "select_ai_panel");
		advancedAIPanel->makeCaption(10, 30, 420, 30, "AI selection");
		advancedAIPanel->makeTextZone(10, 50, 420, 30, "Press the '/' key to change AI type");
		advancedAIPanel->makeTextZone(10, 90, 420, 30, "Advanced");
		advancedAIPanel->makeTextZone(10, 130, 420, 30, "Press right shift to confirm");
		advancedAIPanel->hideInternalMousePointer();
		advancedAIPanel->mNode->setPosition(5.1, GameConstants::LAUNCH_Y + 5, GameConstants::LAUNCH_Z - 7);
	}
}


#if OGRE_PLATFORM == OGRE_PLATFORM_APPLE
#include "macUtils.h"
#endif

#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
#define WIN32_LEAN_AND_MEAN
#include "windows.h"
#endif

#ifdef __cplusplus
extern "C" {
#endif

#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
	INT WINAPI WinMain( HINSTANCE hInst, HINSTANCE, LPSTR strCmdLine, INT )
#else
	int main(int argc, char *argv[])
#endif
	{
		// Create application object
		GameApplication app;

		try {
			app.go();
		} catch( Ogre::Exception& e ) {
#if OGRE_PLATFORM == OGRE_PLATFORM_WIN32
			MessageBox( NULL, e.getFullDescription().c_str(), "An exception has occurred!", MB_OK | MB_ICONERROR | MB_TASKMODAL);
#else
			std::cerr << "An exception has occurred: " <<
				e.getFullDescription().c_str() << std::endl;
#endif
		}

		return 0;
	}

#ifdef __cplusplus
}
#endif
