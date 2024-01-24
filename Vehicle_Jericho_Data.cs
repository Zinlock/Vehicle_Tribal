// -- SOUNDS --

datablock AudioProfile(T2JerichoIdleSound)
{
	filename    = "./wav/jericho_idle.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2JerichoEngineSound)
{
	filename    = "./wav/jericho_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2JerichoStationDeploySound)
{
	filename    = "./wav/jericho_deploy_station.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2JerichoStationUseSound)
{
	filename    = "./wav/jericho_inv_station.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2JerichoTurretDeploySound)
{
	filename    = "./wav/jericho_deploy_turret.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2JerichoTurretUndeploySound)
{
	filename    = "./wav/jericho_undeploy_turret.wav";
	description = AudioClose3d;
	preload = true;
};

// -- TIRES AND SPRINGS --

datablock WheeledVehicleTire(T2JerichoTire)
{
	// Tires act as springs and [...] - jeep vehicle comment
	// oxy: kinda funny that tires act as springs yet you also need separate springs for them

	shapeFile = "./dts/jericho_tire.dts";

	mass = 10;
	radius = 1.8;
	staticFriction = 5;
	kineticFriction = 5;
	restitution = 0.5;

	lateralForce = 18000;
	lateralDamping = 4000;
	lateralRelaxation = 0.01;

	longitudinalForce = 14000;
	longitudinalDamping = 2000;
	longitudinalRelaxation = 0.01;
};

datablock WheeledVehicleSpring(T2JerichoSpring) // why cant these fields be put in the tire datablock anyway?
{
	length = 1.2; // Suspension travel
	force = 12000; // Spring force
	damping = 800; // Spring damping
	antiSwayForce = 6; // Lateral anti-sway force
};

// -- FINAL EXPLOSION --

datablock DebrisData(T2JerichoFinalExplosionDebrisE)
{
	shapeFile = "./dts/jericho_wreck_body.dts";
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

datablock ExplosionData(T2JerichoFinalExplosionE)
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

	debris = T2JerichoFinalExplosionDebrisE;
	debrisNum = 1;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2JerichoFinalExplosionDebrisD : T2JerichoFinalExplosionDebrisE)
{
	shapeFile = "./dts/jericho_wreck_body_back.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2JerichoFinalExplosionD : T2JerichoFinalExplosionE)
{
	debris = T2JerichoFinalExplosionDebrisD;
	debrisNum = 1;
};

datablock DebrisData(T2JerichoFinalExplosionDebrisC : T2JerichoFinalExplosionDebrisE)
{
	shapeFile = "./dts/jericho_wreck_cover.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2JerichoFinalExplosionC : T2JerichoFinalExplosionE)
{
	debris = T2JerichoFinalExplosionDebrisC;
	debrisNum = 1;
};

datablock DebrisData(T2JerichoFinalExplosionDebrisB : T2JerichoFinalExplosionDebrisE)
{
	shapeFile = "./dts/jericho_wreck_invwall.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2JerichoFinalExplosionB : T2JerichoFinalExplosionE)
{
	debris = T2JerichoFinalExplosionDebrisB;
	debrisNum = 1;

	subExplosion[0] = T2JerichoFinalExplosionD;
	subExplosion[1] = T2JerichoFinalExplosionE;
};

datablock DebrisData(T2JerichoFinalExplosionDebrisA : T2JerichoFinalExplosionDebrisE)
{
	shapeFile = "./dts/jericho_wreck_tire.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2JerichoFinalExplosion : T2JerichoFinalExplosionE)
{
	subExplosion[0] = T2JerichoFinalExplosionB;
	subExplosion[1] = T2JerichoFinalExplosionC;

	debris = T2JerichoFinalExplosionDebrisA;
	debrisNum = 6;
};

datablock ProjectileData(T2JerichoFinalExplosionProjectile)
{
	explosion         = T2JerichoFinalExplosion;

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