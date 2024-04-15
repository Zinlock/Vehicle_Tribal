exec("./Vehicle_Havoc_Data.cs");

// -- VEHICLE --

// mounts:
// 0 driver
// 1-5 passenger
// 6-7 jet
// 8-9 contrail
// 10-11 up jet

datablock FlyingVehicleData(T2HavocVehicle : T2WildcatVehicle)
{
	shapeFile = "./dts/Havoc.dts";

	defaultColor = "0.1 0.6 0.9";

	cameraMaxDist = 14;
	cameraOffset = 10;
	cameraTilt = 0;
	cameraRoll = true;

	maxDamage = 350.00;
	destroyedLevel = 350.00;
	
	rechargeRate = 7.5 / 31.25;
	maxEnergy = 50;
	energyShield = 1.0;
	energyScale = 5;
	energyDelay = 3;

	useVehiclePrints[0] = true;
	useEnergyPrints = true;

	maxSteeringAngle = $pi/6;
	integration = 8;

	massBox = "0 0 0";
	massCenter = "0 0 -0.25";

	mass = 400;
	density = 5.0;
	drag = 1.6;

	minDrag = 50;
	maxDrag = 40;

	numMountPoints = 6;
	mountThread[0] = "sit";
	blockImages[0] = true;
	mountThread[1] = "root";
	mountThread[2] = "sit";
	mountThread[3] = "sit";
	mountThread[4] = "sit";
	mountThread[5] = "sit";
	mountToNearest = true;
	
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

	finalExplosionProjectile = T2HavocFinalExplosionProjectile;

	useEngineSounds = true;
	engineSlot = 1;
	engineIdleSound = T2HavocIdleSound;
	engineMoveSound = T2HavocEngineSound;
	engineMoveSpeed = 40;
	engineBoostSound = T2HavocEngineSound;
	engineBoostSpeed = 120;
	
	rollForce		= -10;
	rotationalDrag		= 12;

	maxAutoSpeed = 300;
	autoInputDamping = 0.95;
	autoAngularForce = 300;
	autoLinearForce = 60;

	steeringForce = 1500;
	steeringRollForce = 100;
	
	maneuveringForce = 4000;
	horizontalSurfaceForce = 30;
	verticalSurfaceForce = 30;

	hoverHeight = 7;
	createHoverHeight = 4;
	vertThrustMultiple = 100;

	protectPassengersRadius = true;
	
	rideable = 1;

	isHoverVehicle = false;

	hoverBrakes = true;
	flyingHover = true;

	minHoverDist = 10;
	maxHoverSpeed = 10;
	hoverDownForce = 50;

	boostEnergyDrain = 15 / 31.25;
	minBoostEnergy = 2;
	
	maxBoostUpSpeed = 20;
	boostUpForce = 200;
	maxBoostSpeed = 60;
	boostForce = 150;

	uiName = "T2: Havoc Armored Transport";
};

if($Version $= "20" || $Version $= "21")
{
	T2HavocVehicle.horizontalSurfaceForce = 200;
	T2HavocVehicle.verticalSurfaceForce = 200;
}

function T2HavocVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(1);
	%obj.unMountImage(2);
}

function T2HavocVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.mountImage(T2HavocJetAImage, 1);
	%obj.mountImage(T2HavocJetBImage, 2);
}

function T2HavocVehicle::onEngineHighSpeed(%db, %obj)
{
	%obj.mountImage(T2HavocJetAImage, 1);
	%obj.mountImage(T2HavocJetBImage, 2);
}