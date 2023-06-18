exec("./Vehicle_Beowulf_Data.cs");

// -- VEHICLE --

datablock PlayerData(T2BeowulfHead : TankTurretPlayer)
{
	shapeFile = "./dts/Beowulf_head.dts";
	
	boundingBox = vectorScale("2 2 1", 4);
	boundingBox = vectorScale("2 2 1", 4);

	minLookAngle = -$pi/2;
	maxLookAngle = $pi/6;

	useEyePoint = true;
	firstPersonOnly = true;

	maxEnergy = 100;
	rechargeRate = 10 / 31.25;

	gunHideInventory = false;
	gunImageSlots = 2;
	gunImage[0, 0] = T2BeowulfCannonImage;
	gunImage[0, 1] = T2BeowulfGunImage;
	gunItem[0] = rocketLauncherItem;
	gunTriggerSlot[0] = 0;

	gunImage[1, 0] = T2BeowulfCannonImage;
	gunImage[1, 1] = T2BeowulfGunImage;
	gunItem[1] = gunItem;
	gunTriggerSlot[1] = 1;
	
	uiName = "";
};

function T2BeowulfHead::onGunMount(%db, %obj, %pl)
{
	%obj.playAudio(2, T2BeowulfActivateSound);
}

function T2BeowulfHead::onGunUnMount(%db, %obj, %pl)
{
	%obj.setImageLoaded(0, true);
	%obj.setImageLoaded(1, true);
}

function T2BeowulfHead::onGunEquip(%db, %obj, %pl, %old, %new)
{
	%obj.setImageLoaded(%db.gunTriggerSlot[%old], true);
}

function T2BeowulfHead::onGunTrigger(%db, %obj, %pl, %val)
{
	%obj.setImageLoaded(%db.gunTriggerSlot[%obj.currSlot], !%val);
}

function T2BeowulfHead::onDriverLeave(%this,%obj)
{
	if(%obj.isMounted())
		%obj.setTransform("0 0 0 0 0 0 0");
}

datablock FlyingVehicleData(T2BeowulfVehicle : T2WildcatVehicle)
{
	shapeFile = "./dts/Beowulf.dts";

	defaultColor = "0.609 0.539 0.406";
	paintable = true;

	cameraMaxDist = 10;
	cameraOffset = 6;
	cameraTilt = 0;
	cameraRoll = true;

	maxDamage = 300.00;
	destroyedLevel = 300.00;
	
	rechargeRate = 15 / 31.25;
	maxEnergy = 100;
	energyShield = 1.0;
	energyShape = Turret_EnergyShieldShape;
	energyScale = 3;
	energyDelay = 3;
	energySound = Turret_ShieldDamagedSound;
	energyBreakSound = Turret_ShieldDestroyedSound;

	useVehiclePrints[0] = true;
	useVehiclePrints[1] = true;
	useEnergyPrints = true;

	mass = 300;
	density = 5.0;
	drag = 1.6;

	minDrag = 50;
	maxDrag = 40;

	numMountPoints = 2;
	mountThread[0] = "sit";
	blockImages[0] = true;
	mountThread[1] = "sit";
	blockImages[1] = false;
	mountToNearest = true;
	overridePrevSeat = true;
	overrideNextSeat = true;

	numTurretHeads = 1; // how many controllable turrets
	turretHeadData[0] = T2BeowulfHead; // turret datablock
	turretMountPoint[0] = 2; // point to mount turret to
	turretControlPoint[0] = 1; // mount point to link turret controls to
	
  collDamageThresholdVel = 20;
  collDamageMultiplier = 0.02;
	
	bodyFriction = 0.6;
	bodyRestitution = 0.6;

	softImpactSpeed = 10;
	softImpactSound = T2VehicleImpactSound;
	hardImpactSpeed = 25;
	hardImpactSound = T2VehicleImpactHardSound;

	minImpactSpeed = 25;
	minImpactDamage = 40;
	maxImpactSpeed = 80;
	maxImpactDamage = 300;

	groundImpactMinSpeed = 10.0;
	speedDamageScale = 0.010;

	finalExplosionProjectile = T2BeowulfFinalExplosionProjectile;
	finalExplosionOffset = 0;

	useEngineSounds = true;
	engineSlot = 1;
	engineIdleSound = T2BeowulfIdleSound;
	engineMoveSound = T2BeowulfEngineSound;
	engineMoveSpeed = 15;
	engineBoostSound = "";
	engineBoostSpeed = 300;
	
	rollForce		= -10;
	rotationalDrag		= 15;

	maxAutoSpeed = 300;
	autoInputDamping = 0.9;
	autoAngularForce = 2200;
	autoLinearForce = 50;

	jetForce = 3000;
	minJetEnergy = 0;
	jetEnergyDrain = 0;

	steeringForce = 3000;
	steeringRollForce = 500;
	
	maneuveringForce = 3000;
	horizontalSurfaceForce = 20;
	verticalSurfaceForce = 200;

	hoverHeight = 3;
	createHoverHeight = 3;
	vertThrustMultiple = 0;

	rideable = 1;

	isHoverVehicle = true;
	hoverOverWater = true;
	hoverBrakes = true;
	hoverBrakeForce = 0.05;

	hoverCloseDistance = 2;
	hoverCloseUpForce = 100;

	hoverRecoverSpeed = 20;
	hoverRecoverDistance = 12;
	hoverRecoverAngle = 70;
	hoverRecoverForce = 1000;

	hoverMaxDownTime = 2;
	hoverMinDownForce = 50;
	hoverMaxDownForce = 900;
	hoverMinDistance = 6;
	hoverMaxDistance = 24;
	hoverMaxFallSpeed = 200;

	hoverClimbSpeed = 8;
	hoverClimbDistance = 14;
	hoverClimbAngle = 80;
	hoverClimbForce = 500;

	uiName = "T2: Beowulf Assault Tank";
};

function T2BeowulfVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(2);
	%obj.unMountImage(3);
}

function T2BeowulfVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.mountImage(T2BeowulfJetAImage, 2);
	%obj.mountImage(T2BeowulfJetBImage, 3);
}