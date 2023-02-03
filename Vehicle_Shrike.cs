exec("./Vehicle_Shrike_Data.cs");

// -- VEHICLE --

datablock FlyingVehicleData(T2ShrikeVehicle : T2WildcatVehicle)
{
	shapeFile = "./dts/Shrike.dts";

	defaultColor = "0.1 0.6 0.9";

	cameraMaxDist = 12;
	cameraOffset = 6;
	cameraTilt = 0;
	cameraRoll = true;

	gunHideInventory = true;
	gunImageSlots = 1;
	gunImage[0, 0] = T2ShrikeBlasterAImage;
	gunImage[0, 1] = T2ShrikeBlasterBImage;
	gunItem[0] = -1; // item to use as icon

	maxDamage = 200.00;
	destroyedLevel = 200.00;
	
	rechargeRate = 7.5 / 31.25;
	maxEnergy = 100;
	energyShield = 1.0;
	energyScale = 5;
	energyDelay = 3;

	useVehiclePrints[0] = true;
	useEnergyPrints = true;

	maxSteeringAngle = $pi/6;
	integration = 8;

	massBox = "0 0 0";
	massCenter = "0 0 -0.25";

	mass = 300;
	density = 5.0;
	drag = 1.6;

	minDrag = 50;
	maxDrag = 40;

	numMountPoints = 1;
	mountThread[0] = "sit";
	
  collDamageThresholdVel = 20;
  collDamageMultiplier = 0.02;

	bodyFriction = 0.6;
	bodyRestitution = 0.6;

	softImpactSpeed = 15;
	softImpactSound = T2VehicleImpactSound;
	hardImpactSpeed = 25;
	hardImpactSound = T2VehicleImpactHardSound;

	minImpactSpeed = 25;
	minImpactDamage = 80;
	maxImpactSpeed = 100;
	maxImpactDamage = 500;

	groundImpactMinSpeed = 10.0;
	speedDamageScale = 0.010;
	
	damageLevelTolerance[0] = 0.65;

	finalExplosionProjectile = T2ShrikeFinalExplosionProjectile;

	useEngineSounds = true;
	engineSlot = 1;
	engineIdleSound = T2ShrikeIdleSound;
	engineMoveSound = T2ShrikeEngineSound;
	engineMoveSpeed = 60;
	engineBoostSound = T2ShrikeEngineSound;
	engineBoostSpeed = 120;
	
	rollForce		= -10;
	rotationalDrag		= 12;

	maxAutoSpeed = 300;
	autoInputDamping = 0.95;
	autoAngularForce = 500;
	autoLinearForce = 60;

	steeringForce = 2000;
	steeringRollForce = 100;
	
	maneuveringForce = 10000;
	horizontalSurfaceForce = 200;
	verticalSurfaceForce = 200;

	hoverHeight = 5;
	createHoverHeight = 3;
	vertThrustMultiple = 100;

	rideable = 1;

	isHoverVehicle = false;

	uiName = "T2: Shrike Scout Flier";
};

function T2ShrikeVehicle::onGunUnMount(%db, %obj, %pl)
{
	%obj.setImageLoaded(0, true);
}

function T2ShrikeVehicle::onGunTrigger(%db, %obj, %pl, %val)
{
	%obj.setImageLoaded(0, !%val);
}

function T2ShrikeVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(2);
}

function T2ShrikeVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.mountImage(T2ShrikeJetImage, 2);
}

function T2ShrikeVehicle::onEngineHighSpeed(%db, %obj)
{
	%obj.mountImage(T2ShrikeJetImage, 2);
}