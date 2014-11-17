#ifndef PARTICLESYSTEM_H
#define PARTICLESYSTEM_H

#include <Ogre.h>
#include "GameConstants.h"



using namespace Ogre;

class MyParticleSystem
{
private:
	Ogre::ParticleSystem *mSunParticle;
	Ogre::SceneNode *mParticleNode;

public:
	MyParticleSystem(SceneManager *sceneMgr, int i, Ogre::String type, Ogre::Real scale);

	void setPosition(Ogre::Vector3 myPos);

	Ogre::SceneNode * getNode();

	
};
#endif