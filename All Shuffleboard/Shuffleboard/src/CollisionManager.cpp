#include "CollisionManager.h"

CollisionManager * CollisionManager::mInstance = 0;

// Protected constructor
CollisionManager::CollisionManager( SceneManager * sceneMgr ) : mSceneMgr( sceneMgr ) {
}


CollisionManager * CollisionManager::instance(SceneManager * sceneMgr) {
	if (mInstance == 0)  {  
		mInstance = new CollisionManager( sceneMgr );
	}
    return mInstance;
}

/**
 * Checks for a collision between the supplied entity and the entity for each Puck in the used list.
 *
 *  @param movingEntity The entity that represents the moving puck.
 *  @returns a std::pair<bool, Ogre::String>, the bool indicates hit (true) or miss (false)....the String is the name of the entity
 *     that was intersected or an empty String if no entity was intersected.
 */

pair<bool,Ogre::String> CollisionManager::checkCollisions(  Entity const * const movingEntity ) {
	String targetPuckName = "";
	bool collided = false;
	AxisAlignedBox puckBox = movingEntity->getWorldBoundingBox(true);	
	std::vector<Puck *> usedPucks = PuckManager::instance()->getUsedPucks();
	for ( std::vector<Puck *>::iterator curr = usedPucks.begin(); curr != usedPucks.end() && collided == false ; ++curr ) {
		// Complete this for loop...
		Puck * p = (*curr);
		Entity const * currentEntity;
		currentEntity = p->getEntity();
		if (currentEntity != movingEntity) {
			if (currentEntity->getWorldBoundingBox(true).intersects(puckBox) == true)
			{
				collided = true;
				targetPuckName = currentEntity->getName();
			}
		} // end if currentEntity != movingEntity
	}
	return make_pair(collided, targetPuckName);	
}


/**
* Implements a "placeholder" collision response when originPuck has collided with targetPuck.
* The correct collision respose will be developed in tutorial 5.
*
* @param originPuck The colliding puck.
* @param targetPuck The puck on the receiving end of the collision (collidee).
*/

void CollisionManager::handleCollisions( Puck * originPuck, Puck * targetPuck ) {	
	Vector3 uOrigin = originPuck->getVelocity() * originPuck->getDirection();
	Vector3 uTarget = targetPuck->getVelocity() * targetPuck->getDirection();

	Vector3 nPlane = targetPuck->getPosition() - originPuck->getPosition();

	Real distance = nPlane.length(); 

	// Next step here is to separate the objects by translating targetPuck
	// Calculate the minimum translation distance to separate the two pucks.
	// http://stackoverflow.com/questions/345838/ball-to-ball-collision-detection-and-handling

    // minimum translation distance to push balls apart after intersecting
	Vector3 mtd = nPlane * ( ( ( originPuck->getRadius() + targetPuck->getRadius() ) - distance ) / distance );
	// push-pull them apart based off their mass
	originPuck->setPosition( originPuck->getPosition() + mtd * ( 0.5f ) ) ;
	targetPuck->setPosition( targetPuck->getPosition() - mtd * ( 0.5f ) ) ;

	nPlane.normalise();

	Vector3 vOrigin = uOrigin - ((1 + .9) / 2) * ((uOrigin - uTarget).dotProduct(nPlane)) * nPlane;
	originPuck->setVelocity(vOrigin.length());
	vOrigin.normalise();
	originPuck->setDirection(vOrigin, false);

	Vector3 vTarget = uTarget + ((1 + .9) / 2) * ((uOrigin - uTarget).dotProduct(nPlane)) * nPlane;
	targetPuck->setVelocity(vTarget.length());
	vTarget.normalise();
	targetPuck->setDirection(vTarget, false);
	int mySound1 = SoundManager::getSingleton().CreateStream(Ogre::String("puck collide.wav") );
	int channel1 = 1;
	SoundManager::getSingleton().PlaySound(mySound1, originPuck->getNode(), &channel1);
}

