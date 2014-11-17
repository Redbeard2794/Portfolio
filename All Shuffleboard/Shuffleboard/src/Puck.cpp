#include "Puck.h"



Puck::Puck( Ogre::SceneManager *sceneMan, Ogre::String const & puckName, PuckType type) : 
	mSceneMgr( sceneMan ),
	mForceKineticFriction( GameConstants::COEFF_KINETIC_FRICTION * GameConstants::MASS * GameConstants::GRAVITY ),
	mDeceleration( mForceKineticFriction / GameConstants::MASS ),	
	mAcceleration(0),
	mVelocity(0),
	mDirection(0,0,-1),
	mPuckName(puckName),
	mIsVisible( false ),
	mPuckType( type ) {

    // Start here...
		if (type == YELLOWSTONE)
		{
			mEntity = mSceneMgr->createEntity(mPuckName + "Entity", "Yellowstone.mesh");
		}
		else if (type == REDSTONE)
		{
			mEntity = mSceneMgr->createEntity(mPuckName + "Entity", "Redstone.mesh");
		}
		mNode = mSceneMgr->getRootSceneNode()->createChildSceneNode("node" + mPuckName);

		mNode->scale(1.5f,1.5f,1.5f);//scales the node/entity
		mNode->attachObject(mEntity);
		mNode->setPosition(0, 1, 10);

		AxisAlignedBox box = mEntity->getWorldBoundingBox(true);
		Vector3 boxSize(box.getSize());//get dimensions of bounding box
		mRadius = boxSize.x / 2;//puck radius is x component / 2
		mRadius -= 0.1f;
}


void Puck::update( Ogre::Real const & timeSinceLastFrame ) {
	// Only update if this puck is visible and moving.
	if (mIsVisible && mVelocity > 0)
	{
		// When node is yawed 180, TS_LOCAL switches z directions!
		 mNode->translate( mDirection * GameConstants::UPDATE_GRANULARITY *  mVelocity, Ogre::Node::TS_PARENT ); 

		 //check for collision
		 std::pair<bool, Ogre::String> result = CollisionManager::instance()->checkCollisions(mEntity);
		 //if a collision occurred
		 if (result.first == true)
		 {
			 string nameCollidee;
			 nameCollidee = result.second;
			 Puck * collidee = PuckManager::instance()->getPuckByName(nameCollidee);
			 //carries out the collision detection
			 CollisionManager::instance()->handleCollisions(this,collidee);
		 }
		 updateMotion();
	}
	
}



void Puck::activate( Ogre::Vector3 const & pos ) {	
	
	mNode->setPosition(pos);
	mIsVisible = true;
}


void Puck::applyForce( Ogre::Real const & force ) {
	mVelocity = force / GameConstants::MASS;
}

// This method rotates the Puck to the direction it is travelling
void Puck::rotate() {		
	Ogre::Vector3 directionFacing = mNode->getOrientation() * Ogre::Vector3::UNIT_Z;
	if ((1.0f + directionFacing.dotProduct(mDirection)) < 0.0001f) 
	{
			mNode->yaw(Ogre::Degree(180));
	}
	else 
	{
		Ogre::Quaternion quat = directionFacing.getRotationTo(mDirection);
		mNode->rotate(quat);
	} 
}


void Puck::removeFromBoard() {
	mIsVisible = false;
	mAcceleration = 0;
	mVelocity = 0;
	mNode->setPosition(0,0,1000);
}


Ogre::String Puck::getName() const {
	return mPuckName;
}

Ogre::Vector3 Puck::getPosition() const {	
	return mNode->getPosition();	
}

Ogre::Vector3 Puck::getDirection() const {
	return mDirection;
}

Ogre::Real Puck::getVelocity() const {
	return mVelocity;
}

Puck::PuckType Puck::getPuckType() const {
	return mPuckType;
}


bool Puck::isVisible() const {
	return mIsVisible;
}

void Puck::setVelocity( Ogre::Real const & velocity ) {
	mVelocity = velocity;
}


void Puck::setDirection( Ogre::Vector3 const & dir, bool rotate) {
	mDirection = dir;
	if (rotate)
	{
		this->rotate();
	}
}
void Puck::updateMotion()
{
	mAcceleration = -mDeceleration; 
	Ogre::Vector3 pos = this->getPosition(); 
	// Add acceleration to objects velocity. 

	if ( Ogre::Math::IFloor(mVelocity * 10) > 0 ) {
		mVelocity = mVelocity + ( mAcceleration * GameConstants::UPDATE_GRANULARITY );
	
		// Check if puck is out of bounds. 
		if ( pos.x < GameConstants::MIN_X || pos.x > GameConstants::MAX_X || pos.z < GameConstants::MIN_Z ) 
		{
			removeFromBoard(); 
		}
	}
	else {
		 mVelocity = mAcceleration = 0; 
		 mDirection = Vector3(0,0,0);
		 if ( pos.z > GameConstants::FOUL_LINE) 
			{
				removeFromBoard();
			}
	}
}
Ogre::Entity * Puck::getEntity() const
{
	return mEntity;
}

void Puck::setPosition(Ogre::Vector3 const & position)
{
	mNode->setPosition(position);
}
Ogre::Real Puck::getRadius() const
{
	return mRadius;
}
//Ogre::SceneNode * getNode();
Ogre::SceneNode* Puck::getNode()
{return mNode;}
