exec("./Vehicle_Thundersword_Data.cs");

// -- VEHICLE --

datablock PlayerData(T2ThunderswordHead : TankTurretPlayer)
{
	shapeFile = "./dts/Thundersword_head.dts";
	
	boundingBox = vectorScale("2 2 1", 4);
	boundingBox = vectorScale("2 2 1", 4);

	minLookAngle = 0;
	maxLookAngle = $pi;

	useEyePoint = true;
	firstPersonOnly = true;

	maxEnergy = 100;
	rechargeRate = 12.5 / 31.25;

	gunHideInventory = false;
	gunImageSlots = 2;
	gunImage[0, 0] = T2ThunderswordBlasterImage;
	gunImage[0, 1] = T2ThunderswordBomberImage;
	gunItem[0] = gunItem;
	gunTriggerSlot[0] = 0;

	gunImage[1, 0] = T2ThunderswordBlasterImage;
	gunImage[1, 1] = T2ThunderswordBomberImage;
	gunItem[1] = rocketLauncherItem;
	gunTriggerSlot[1] = 1;

	uiName = "";
};

function T2ThunderswordHead::onGunMount(%db, %obj, %pl)
{
	%obj.playAudio(2, T2ThunderswordActivateSound);
}

function T2ThunderswordHead::onGunUnMount(%db, %obj, %pl)
{
	%obj.setImageLoaded(0, true);
	%obj.setImageLoaded(1, true);
}

function T2ThunderswordHead::onGunEquip(%db, %obj, %pl, %old, %new)
{
	%obj.setImageLoaded(%db.gunTriggerSlot[%old], true);

	if(%new == 1)
		%obj.bombing = true;
	else
		%obj.bombing = false;
}

function T2ThunderswordHead::onGunTrigger(%db, %obj, %pl, %val)
{
	%obj.setImageLoaded(%db.gunTriggerSlot[%obj.currSlot], !%val);
}

function T2ThunderswordHead::onDriverLeave(%this,%obj)
{
	if(%obj.isMounted())
		%obj.setTransform("0 0 0 0 0 0 0");
}

datablock FlyingVehicleData(T2ThunderswordVehicle : T2WildcatVehicle)
{
	shapeFile = "./dts/Thundersword.dts";

	defaultColor = "0.1 0.6 0.9";

	cameraMaxDist = 12;
	cameraOffset = 6;
	cameraTilt = 0;
	cameraRoll = true;

	gunHideInventory = true;

	maxDamage = 250.00;
	destroyedLevel = 250.00;
	
	rechargeRate = 7.5 / 31.25;
	maxEnergy = 150;
	energyShield = 1.0;
	energyScale = 5;
	energyDelay = 3;

	useVehiclePrints[0] = true;
	useVehiclePrints[1] = true;
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

	numMountPoints = 3;
	mountThread[0] = "sit";
	blockImages[0] = true;
	mountThread[1] = "sit";
	mountThread[2] = "root";
	mountToNearest = true;
	overridePrevSeat = true;
	overrideNextSeat = true;
	
	numTurretHeads = 1;
	turretHeadData[0] = T2ThunderswordHead;
	turretMountPoint[0] = 3;
	turretControlPoint[0] = 1;
	
  collDamageThresholdVel = 20;
  collDamageMultiplier = 0.02;

	bodyFriction = 0.6;
	bodyRestitution = 0.6;

	softImpactSpeed = 15;
	softImpactSound = T2VehicleImpactSound;
	hardImpactSpeed = 25;
	hardImpactSound = T2VehicleImpactHardSound;

	minImpactSpeed = 25;
	minImpactDamage = 50;
	maxImpactSpeed = 150;
	maxImpactDamage = 400;

	groundImpactMinSpeed = 10.0;
	speedDamageScale = 0.010;
	
	damageLevelTolerance[0] = 0.65;

	finalExplosionProjectile = T2ThunderswordFinalExplosionProjectile;

	useEngineSounds = true;
	engineSlot = 2;
	engineIdleSound = T2ThunderswordIdleSound;
	engineMoveSound = T2ThunderswordEngineSound;
	engineMoveSpeed = 75;
	engineBoostSound = T2ThunderswordEngineSound;
	engineBoostSpeed = 120;

	rollForce		= -5;
	rotationalDrag		= 12;

	maxAutoSpeed = 300;
	autoInputDamping = 0.98;
	autoAngularForce = 300;
	autoLinearForce = 100;

	steeringForce = 1000;
	steeringRollForce = 100;
	
	maneuveringForce = 8000;
	horizontalSurfaceForce = 20;
	verticalSurfaceForce = 20;

	hoverHeight = 6;
	createHoverHeight = 4;

	protectPassengersRadius = true;
	
	rideable = 1;

	isHoverVehicle = false;

	hoverBrakes = true;
	flyingHover = true;

	minHoverDist = 10;
	maxHoverSpeed = 15;
	hoverDownForce = 50;

	boostEnergyDrain = 25 / 31.25;
	minBoostEnergy = 10;
	
	maxBoostUpSpeed = 30;
	boostUpForce = 400;
	maxBoostSpeed = 90;
	boostForce = 500;

	uiName = "T2: Thundersword Bomber";
};

if($Version $= "20" || $Version $= "21")
{
	T2ThunderswordVehicle.horizontalSurfaceForce = 1000;
	T2ThunderswordVehicle.verticalSurfaceForce = 800;
}

function T2ThunderswordVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(2);
	%obj.unMountImage(3);
}

function T2ThunderswordVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.mountImage(T2ThunderswordJetImage, 2);
	%obj.mountImage(T2ThunderswordJet2Image, 3);
}

function T2ThunderswordVehicle::onEngineHighSpeed(%db, %obj)
{
	%obj.mountImage(T2ThunderswordJetImage, 2);
	%obj.mountImage(T2ThunderswordJet2Image, 3);
}