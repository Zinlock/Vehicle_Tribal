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

datablock DebrisData(T2WildcatFinalExplosionDebrisE)
{
	shapeFile = "./dts/wildcat_wreck_tube.dts";
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

datablock ExplosionData(T2WildcatFinalExplosionE)
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

	debris = T2WildcatFinalExplosionDebrisE;
	debrisNum = 2;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2WildcatFinalExplosionDebrisD : T2WildcatFinalExplosionDebrisE)
{
	shapeFile = "./dts/wildcat_wreck_thruster.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2WildcatFinalExplosionD : T2WildcatFinalExplosionE)
{
	debris = T2WildcatFinalExplosionDebrisD;
	debrisNum = 1;
};

datablock DebrisData(T2WildcatFinalExplosionDebrisC : T2WildcatFinalExplosionDebrisE)
{
	shapeFile = "./dts/wildcat_wreck_handle.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2WildcatFinalExplosionC : T2WildcatFinalExplosionE)
{
	debris = T2WildcatFinalExplosionDebrisC;
	debrisNum = 2;
};

datablock DebrisData(T2WildcatFinalExplosionDebrisB : T2WildcatFinalExplosionDebrisE)
{
	shapeFile = "./dts/wildcat_wreck_engine.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2WildcatFinalExplosionB : T2WildcatFinalExplosionE)
{
	debris = T2WildcatFinalExplosionDebrisB;
	debrisNum = 1;

	subExplosion[0] = T2WildcatFinalExplosionD;
	subExplosion[1] = T2WildcatFinalExplosionE;
};

datablock DebrisData(T2WildcatFinalExplosionDebrisA : T2WildcatFinalExplosionDebrisE)
{
	shapeFile = "./dts/wildcat_wreck_body.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2WildcatFinalExplosion : T2WildcatFinalExplosionE)
{
	subExplosion[0] = T2WildcatFinalExplosionB;
	subExplosion[1] = T2WildcatFinalExplosionC;

	debris = T2WildcatFinalExplosionDebrisA;
	debrisNum = 1;
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