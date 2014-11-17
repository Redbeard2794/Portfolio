#include"AI.h"
#include <cstdlib>//this is for using rand() to generate random numbers

AI * AI::mInstance = 0;

AI::AI(bool easyMode)
{
	setEasy(easyMode);
}

AI * AI::instance(bool easyMode)
{
	if(mInstance == 0)
		mInstance = new AI(easyMode);
	return mInstance;
}

//gets
//bool getEasy()const;
bool AI::getEasy()const
{return easy;}
//sets
//void setEasy(bool myMode);
void AI::setEasy(bool myMode)
{easy = myMode;}

void AI::takeTurn()
{
	//calls the shuffleboard launch puck method
	Shuffleboard::instance()->launchPuck();
}