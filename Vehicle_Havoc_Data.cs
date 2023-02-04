// -- SOUNDS --

datablock AudioProfile(T2HavocIdleSound)
{
	filename    = "./wav/Havoc_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2HavocEngineSound)
{
	filename    = "./wav/Havoc_boost.wav";
	description = AudioCloseLooping3d;
	preload = true;
};


// -- WEAPON FX --

datablock ParticleData(T2HavocBlasterImpactParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0;
	constantAcceleration = 0;
	lifetimeMS           = 250;
	lifetimeVarianceMS   = 50;
	textureName          = "base/data/particles/dot";
	spinSpeed		= 0.0;
	spinRandomMin		= 0.0;
	spinRandomMax		= 0.0;
	colors[0]     = "0.0 0.6 1.0 0.9";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 4;
	sizes[1]      = 0;

	useInvAlpha = false;
};

datablock ParticleEmitterData(T2HavocBlasterImpactEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 89;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;

	particles = "T2HavocBlasterImpactParticle";
};


// -- JET FX --

datablock ParticleData(T2HavocJetParticle)
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
	sizes[0]      = 4;
	sizes[1]      = 10;
};

datablock ParticleEmitterData(T2HavocJetEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 3;
	ejectionVelocity = -20;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 12;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = true;
	particles = "T2HavocJetParticle";
};

datablock ShapeBaseImageData(T2HavocJetAImage : T2Contrail2Image)
{
	mountPoint = 6;
	stateEmitter[1]					= T2HavocJetEmitter;
	stateEmitter[2]					= T2HavocJetEmitter;
};

datablock ShapeBaseImageData(T2HavocJetBImage : T2Contrail2Image)
{
	mountPoint = 7;
	stateEmitter[1]					= T2HavocJetEmitter;
	stateEmitter[2]					= T2HavocJetEmitter;
};


// -- FINAL EXPLOSION --

datablock DebrisData(T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_wing.dts";
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

datablock ExplosionData(T2HavocFinalExplosionE)
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

	debris = T2HavocFinalExplosionDebrisE;
	debrisNum = 2;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2HavocFinalExplosionDebrisF : T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_head.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2HavocFinalExplosionF : T2HavocFinalExplosionE)
{
	debris = T2HavocFinalExplosionDebrisF;
	debrisNum = 1;
};

datablock DebrisData(T2HavocFinalExplosionDebrisD : T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_thruster.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2HavocFinalExplosionD : T2HavocFinalExplosionE)
{
	debris = T2HavocFinalExplosionDebrisD;
	debrisNum = 2;
};

datablock DebrisData(T2HavocFinalExplosionDebrisC : T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_leg.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2HavocFinalExplosionC : T2HavocFinalExplosionE)
{
	debris = T2HavocFinalExplosionDebrisC;
	debrisNum = 2;
};

datablock DebrisData(T2HavocFinalExplosionDebrisB : T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_seat.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2HavocFinalExplosionB : T2HavocFinalExplosionE)
{
	debris = T2HavocFinalExplosionDebrisB;
	debrisNum = 2;

	subExplosion[0] = T2HavocFinalExplosionD;
	subExplosion[1] = T2HavocFinalExplosionE;
};

datablock DebrisData(T2HavocFinalExplosionDebrisA : T2HavocFinalExplosionDebrisE)
{
	shapeFile = "./dts/Havoc_wreck_back.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2HavocFinalExplosion : T2HavocFinalExplosionE)
{
	subExplosion[0] = T2HavocFinalExplosionB;
	subExplosion[1] = T2HavocFinalExplosionC;

	debris = T2HavocFinalExplosionDebrisA;
	debrisNum = 1;
};

datablock ProjectileData(T2HavocFinalExplosionProjectile)
{
	explosion         = T2HavocFinalExplosion;

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