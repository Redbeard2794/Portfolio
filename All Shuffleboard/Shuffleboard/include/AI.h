#ifndef AI_H
#define AI_H
#include "GameConstants.h"
#include "PuckManager.h"
#include"Puck.h"
#include "Shuffleboard.h"
class AI
{
public:

	static AI * instance(bool easyMode = 1);
	void takeTurn();
	//gets
	bool getEasy()const;

	//sets
	void setEasy(bool myMode);
private:
	AI(bool easyMode);
	
	static AI * mInstance;

	bool easy;
	
};
#endif