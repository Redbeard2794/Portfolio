
#include "MyParticleSystem.h"
// i is just a number passed in when creating a particle system
//it is added to its name to prevnt name clashes
//type is a string that tells the it which file and script to use
//scale is used to change the size of each particle
//only really used for the falmes/smoke coming from the ogres mouth
MyParticleSystem::MyParticleSystem(SceneManager *sceneMgr, int i, Ogre::String type, Ogre::Real scale)
{		
	mSunParticle = sceneMgr->createParticleSystem("particles" + Ogre::StringConverter::toString(i+1), type);
	Ogre::Real height = mSunParticle->getDefaultHeight();
	Ogre::Real width = mSunParticle->getDefaultWidth();

	mSunParticle->setDefaultDimensions(height / scale, width / scale);

    mParticleNode = sceneMgr->getRootSceneNode()->createChildSceneNode("ParticleNode" + Ogre::StringConverter::toString(i+1));	
		
    mParticleNode->attachObject(mSunParticle);
}
	
void MyParticleSystem::setPosition(Ogre::Vector3 myPos)
{mParticleNode->setPosition(myPos);}

Ogre::SceneNode* MyParticleSystem::getNode()
{return mParticleNode;}

