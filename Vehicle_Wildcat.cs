// -- SOUNDS --

datablock AudioProfile(T2WildcatIdleSound)
{
	filename    = "./wav/wildcat_idle.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2WildcatEngineSound)
{
	filename    = "./wav/wildcat_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2WildcatEngineFastSound)
{
	filename    = "./wav/wildcat_boost.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

// -- JET FX --

datablock ParticleData(T2WildcatJetParticle)
{
	dragCoefficient      = 3;
	windCoefficient     = 0;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 0.25;
	constantAcceleration = 0.0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS           = 500;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	useInvAlpha = false;

	colors[0]     = "0.9 0.5 0.1 0.2";
	colors[1]     = "0.2 0.2 0.2 0";
	sizes[0]      = 1;
	sizes[1]      = 5;
};

datablock ParticleEmitterData(T2WildcatJetEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 3;
	ejectionVelocity = -10;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 12;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = true;
	particles = "T2WildcatJetParticle";
};

datablock ShapeBaseImageData(T2WildcatJetImage : T2Contrail2Image)
{
	mountPoint = 1;
	stateEmitter[1]					= T2WildcatJetEmitter;
	stateEmitter[2]					= T2WildcatJetEmitter;
};

// -- FINAL EXPLOSION --

datablock DebrisData(T2WildcatFinalExplosionDebris)
{
	shapeFile = "./dts/wildcat_wreck.dts";
	lifetime = 10.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 400.0;
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	emitters = T2VehicleExplosionShrapnelEmitter;

	gravModifier = 1.5;
};

datablock ExplosionData(T2WildcatFinalExplosion)
{
	lifeTimeMS = 150;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = false;

	lightStartRadius = 0;
	lightEndRadius = 0;
	lightStartColor = "0 0 0";
	lightEndColor = "0 0 0";

	impulseRadius = 0;
	impulseForce = 0;

	damageRadius = 0;
	radiusDamage = 0;

	uiName = "";
	
	debris = T2WildcatFinalExplosionDebris;
	debrisNum = 1;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 20;
	debrisVelocity = 18;
	debrisVelocityVariance = 3;
};

datablock ProjectileData(T2WildcatFinalExplosionProjectile)
{
	explosion         = T2WildcatFinalExplosion;

	muzzleVelocity      = 0;
	velInheritFactor    = 0;
	explodeOnPlayerImpact = false;
	explodeOnDeath        = true;  

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	armingDelay         = 100;
	lifetime            = 100;
	fadeDelay           = 100;
	bounceElasticity    = 0;
	bounceFriction      = 0;
	isBallistic         = false;
};

// -- VEHICLE --

datablock FlyingVehicleData(T2WildcatVehicle)
{
	shapeFile = "./dts/wildcat.dts";

	defaultColor = "0.609 0.539 0.406";
	paintable = true;

	cameraMaxDist = 8.5;
	cameraOffset = 3;
	cameraTilt = 0;
	cameraRoll = true;

	maxDamage = 200.00;
	destroyedLevel = 200.00;
	
	rechargeRate = 10 / 31.25;
	maxEnergy = 100;
	energyShield = 1.0;
	energyShape = Turret_EnergyShieldShape;
	energyScale = 3;
	energyDelay = 3;
	energySound = Turret_ShieldDamagedSound;
	energyBreakSound = Turret_ShieldDestroyedSound;

	useVehiclePrints[0] = true;
	useEnergyPrints = true;
	useHeatPrints = false;

	maxSteeringAngle = $pi/4;
	integration = 8;

	massBox = "0 0 0";
	massCenter = "0 0 -0.6"; // must be centered for hover vehicles

	mass = 200;
	density = 5.0;
	drag = 1.6;

	minDrag = 50;
	maxDrag = 40;

	numMountPoints = 1;
	mountThread[0] = "sit";
	
  collDamageThresholdVel = 20;
  collDamageMultiplier = 0.02;
	
	splash = vehicleSplash;
	splashVelocity = 4.0;
	splashAngle = 67.0;
	splashFreqMod = 300.0;
	splashVelEpsilon = 0.60;

	bubbleEmitTime = 1.4;
	splashEmitter[0] = vehicleFoamDropletsEmitter;
	splashEmitter[1] = vehicleFoamEmitter;
	splashEmitter[2] = vehicleBubbleEmitter;

	softSplashSoundVelocity = 5.0;
	mediumSplashSoundVelocity = 30.0;
	hardSplashSoundVelocity = 60.0;
	exitSplashSoundVelocity = 5.0;

	// sounds:
	exitingWater = T2VehicleSplashExitSound;
	impactWaterEasy = T2VehicleSplashLightSound;
	impactWaterMedium = T2VehicleSplashMediumSound;
	impactWaterHard = T2VehicleSplashHeavySound;
	// waterWakeSound; // ? might be the sound for hovering over water

	// todo: test these:
	// ParticleEmitterDataPtr forwardJetEmitter;
  // ParticleEmitterDataPtr backwardJetEmitter;
  // ParticleEmitterDataPtr downJetEmitter;
  // ParticleEmitterDataPtr trailEmitter;
  // float minTrailSpeed;
	// ParticleEmitterDataPtr dustEmitter;
	// float triggerDustHeight;
	// float dustHeight;

	bodyFriction = 0.6;
	bodyRestitution = 0.6;

	softImpactSpeed = 15;
	softImpactSound = T2VehicleImpactSound;
	hardImpactSpeed = 25;
	hardImpactSound = T2VehicleImpactHardSound;

	minImpactSpeed = 25;
	minImpactDamage = 20;
	maxImpactSpeed = 90;
	maxImpactDamage = 300;

	groundImpactMinSpeed = 10.0;
	speedDamageScale = 0.010;
	
	damageEmitter[0] = T2VehicleDamageEmitter;
	damageEmitterOffset[0] = "0.0 -0.5 0.5 ";
	damageLevelTolerance[0] = 0.65;

	damageEmitter[1] = T2VehicleExplosionShrapnelEmitter;
	damageEmitterOffset[1] = "0.0 -0.5 0.5 ";
	damageLevelTolerance[1] = 0.998;

	numDmgEmitterAreas = 3; // how does this work??

	initialExplosionProjectile = T2VehicleInitialExplosionProjectile;
	initialExplosionOffset = 0;

	burnTime = 32;

	finalExplosionProjectile = T2WildcatFinalExplosionProjectile;
	finalExplosionOffset = 0;

	// engineSound = T2WildcatEngineSound;
	// jetSound = hammerHitSound;

	useEngineSounds = true;
	engineIdleSound = T2WildcatIdleSound;
	engineMoveSound = T2WildcatEngineSound;
	engineMoveSpeed = 25;
	engineBoostSound = T2WildcatEngineFastSound;
	engineBoostSpeed = 60;
	
	rollForce		= 20;
	rotationalDrag		= 15;

	maxAutoSpeed = 300;
	autoInputDamping = 0.9;
	autoAngularForce = 2200;
	autoLinearForce = 30;

	jetForce = 3000;
	minJetEnergy = 0;
	jetEnergyDrain = 0;

	steeringForce = 4500;
	steeringRollForce = 0;
	
	maneuveringForce = 5000;
	horizontalSurfaceForce = 20;
	verticalSurfaceForce = 200;

	hoverHeight = 2;
	createHoverHeight = 2;
	vertThrustMultiple = 0;

	rideable = 1;

	isHoverVehicle = true;
	hoverOverWater = true;
	hoverBrakes = true;
	hoverBrakeForce = 0.05;

	// we're too close to the ground; apply up forces
	hoverCloseDistance = 1;
	hoverCloseUpForce = 50;

	// we're falling; apply up forces
	hoverRecoverSpeed = 40; // min speed to apply up forces
	hoverRecoverDistance = 24; // max obstacle detection distance
	hoverRecoverAngle = 70; // max obstacle angle
	hoverRecoverForce = 1000; // recovery up force

	// we're too high; apply down forces
	hoverMaxDownTime = 2; // slowly apply down force over x seconds
	hoverMinDownForce = 50; // weakest down force
	hoverMaxDownForce = 900; // strongest down force
	hoverMinDistance = 4; // distance for weak down force
	hoverMaxDistance = 24; // distance for strong down force
	hoverMaxFallSpeed = 200; // z velocity to stop applying down forces at

	// we're running into something; try going above it
	hoverClimbSpeed = 16; // min speed to climb
	hoverClimbDistance = 20; // max detection distance
	hoverClimbAngle = 80; // max angle to climb above
	hoverClimbForce = 500; // climb up force

	uiName = "T2: Wildcat Gravcycle";
};

function T2WildcatVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(0);
	%obj.unMountImage(1);
	%obj.unMountImage(2);
}

function T2WildcatVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.unMountImage(0);
	%obj.unMountImage(1);
	%obj.mountImage(T2WildcatJetImage, 2);
}

function T2WildcatVehicle::onEngineHighSpeed(%db, %obj)
{
	%obj.mountImage(T2Contrail2Image, 0);
	%obj.mountImage(T2Contrail3Image, 1);
	%obj.mountImage(T2WildcatJetImage, 2);
}