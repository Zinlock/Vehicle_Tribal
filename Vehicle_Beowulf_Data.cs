// -- SOUNDS --

datablock AudioProfile(T2BeowulfActivateSound)
{
	filename    = "./wav/Beowulf_activate.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(T2BeowulfIdleSound)
{
	filename    = "./wav/Beowulf_idle.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2BeowulfEngineSound)
{
	filename    = "./wav/Beowulf_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2BeowulfMGFireSound)
{
	filename    = "./wav/Beowulf_gun.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(T2BeowulfFireSound)
{
	filename    = "./wav/Beowulf_cannon.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(T2BeowulfImpactSound)
{
	filename    = "./wav/Beowulf_cannon_impact.wav";
	description = AudioExplosionFar3D;
	preload = true;
};


// -- WEAPON FX --

datablock ParticleData(T2BeowulfGunImpactParticle)
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
	colors[0]     = "0.6 0.6 0.2 0.9";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 4;
	sizes[1]      = 0;

	useInvAlpha = false;
};

datablock ParticleEmitterData(T2BeowulfGunImpactEmitter)
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

	particles = "T2BeowulfGunImpactParticle";
};

// -- WEAPON --

datablock ExplosionData(T2BeowulfGunExplosion : gunExplosion)
{
	damageRadius = 3;
	radiusDamage = 10;

	lightStartColor = "1.0 0.9 0.0 1.0";
};

datablock ProjectileData(T2BeowulfGunProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	directDamage = 5;
	directDamageType    = $DamageType::Gun;
	radiusDamageType    = $DamageType::Gun;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	impactImpulse	     = 400;
	verticalImpulse	   = 100;
	explosion = T2BeowulfGunExplosion;
	particleEmitter     = "";

	muzzleVelocity      = 200;
	velInheritFactor    = 0;

	armingDelay         = 0;
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = false;
	gravityMod = 0.0;

	hasLight    = true;
	lightRadius = 3.0;
	lightColor  = "1 1 0";
};

datablock StaticShapeData(T2BeowulfGunTracer) { shapeFile = "./dts/bullet_trail.dts"; };

function T2BeowulfGunTracer::onAdd(%this,%obj)
{
  %obj.schedule(0, playThread, 2, root);
  %obj.schedule(2000,delete);
}

datablock ShapeBaseImageData(T2BeowulfGunImage)
{
	class = "WeaponImage";

	shapeFile = "./dts/Beowulf_gun.dts";
	emap = false;

	mountPoint = 1;
	rotation = eulerToMatrix("-90 0 0");

	projectile = T2BeowulfGunProjectile;
	projectileCount = 1;
	projectileSpread = 0.5;
	projectileSpeed = 200;

	projectileHitscan = true;
	projectileHitscanRange = 400;
	projectileScale = 1;
	tracerSize = 2;
	tracerData = T2BeowulfGunTracer;

	fireSound = T2BeowulfMGFireSound;

	energyDrain = 3;
	minEnergy = 10;

	stateName[0]                    = "Ready";
	stateTransitionOnNotLoaded[0]   = "Fire";
	stateTransitionOnNoAmmo[0]      = "AmmoCheck";
	stateTimeoutValue[0]            = 0.1;
	stateSequence[0]                = "root";
	stateTransitionOnTimeout[0]     = "Ready";
	stateScript[0]                  = "onReadyLoop";

	stateName[1]                    = "Fire";
	stateSequence[1]                = "fire";
	stateTransitionOnTimeout[1]     = "Delay";
	stateWaitForTimeout[1]          = True;
	stateTimeoutValue[1]            = 0.065;
	stateScript[1]                  = "onFire";

	stateName[2]                    = "Delay";
	stateTransitionOnTimeout[2]     = "AmmoCheck";
	stateWaitForTimeout[2]          = True;
	stateTimeoutValue[2]            = 0.0;
	stateScript[2]                  = "onDelay";

	stateName[3]                    = "AmmoCheck";
	stateTransitionOnTimeout[3]     = "AmmoCheckB";
	stateWaitForTimeout[3]          = True;
	stateTimeoutValue[3]            = 0.01;
	stateScript[3]                  = "onAmmoCheck";
	
	stateName[4]                    = "AmmoCheckB";
	stateTransitionOnAmmo[4]        = "Ready";
	stateTransitionOnNoAmmo[4]      = "AmmoCheck";
	stateTransitionOnTimeout[4]     = "Ready";
	stateTimeoutValue[4]            = 0.033;
};

function T2BeowulfGunImage::onMount(%img, %obj, %slot)
{
	%obj.sourceObject.useExtraPrints = true;
	%obj.sourceObject.extraPrintLabel = "RDS";
	%obj.sourceObject.extraPrintColor = "FFEE44";
}

function T2BeowulfGunImage::onReadyLoop(%img, %obj, %slot)
{
	%obj.sourceObject.extraPrintText = mCeil((%obj.getEnergyLevel() / %obj.getDataBlock().maxEnergy) * 100) @ "%";
	%img.onAmmoCheck(%obj, %slot);
}

function T2BeowulfGunImage::onFire(%img, %obj, %slot)
{
	if(isObject(%pl = %obj.lastMountedPlayer))
	{
		%obj.setEnergyLevel(%obj.getEnergyLevel() - %img.energyDrain);
		%obj.stopAudio(0);
		%obj.playAudio(0, %img.fireSound);

		%img.onReadyLoop(%obj, %slot);

		if(%img.projectileHitscan)
			RaycastFire(%img.projectile, %obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot), %img.projectileSpread, %img.projectileCount, %slot, %pl, %pl.Client, %img, %img.projectileHitscanRange);
		else
			ProjectileFire(%img.projectile, %obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot), %img.projectileSpread, %img.projectileCount, %slot, %pl, %pl.Client, %img.projectileSpeed);
	}
}

function T2BeowulfGunImage::onDelay(%img, %obj, %slot)
{
	
}

function T2BeowulfGunImage::onAmmoCheck(%img, %obj, %slot)
{
	if(%obj.getEnergyLevel() > %img.minEnergy)
		%obj.setImageAmmo(%slot, 1);
	else
		%obj.setImageAmmo(%slot, 0);
}

datablock ParticleData(T2BeowulfCannonTrailParticle)
{
	windCoefficient      = 0.5;
	dragCoefficient      = 3;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 0.1;
	constantAcceleration = 0;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 200;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 1000.0;
	spinRandomMin		= -1000.0;
	spinRandomMax		= 1000.0;
	colors[0]     = "1.0 0.9 0.2 0.2";
	colors[1]     = "1.0 1.0 1.0 0.0";
	sizes[0]      = 1;
	sizes[1]      = 3;

	useInvAlpha = true;
};

datablock ParticleEmitterData(T2BeowulfCannonTrailEmitter)
{
	ejectionPeriodMS = 15;
	periodVarianceMS = 5;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 0;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;

	particles = "T2BeowulfCannonTrailParticle";
};

datablock ExplosionData(T2BeowulfCannonExplosion : T2VehicleInitialExplosion)
{
	soundProfile = T2BeowulfImpactSound;

	debris = "";
	debrisNum = 0;
	debrisNumVariance = 0;

	damageRadius = 12;
	radiusDamage = 90;
};

datablock ProjectileData(T2BeowulfCannonProjectile)
{
	projectileShapeName = "Add-Ons/Vehicle_Tank/tankbullet.dts";

	directDamage = 0;
	directDamageType = $DamageType::TankShellDirect;
	radiusDamageType = $DamageType::TankShellRadius;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	impactImpulse	     = 0;
	verticalImpulse	   = 0;
	explosion          = T2BeowulfCannonExplosion;
	particleEmitter    = T2BeowulfCannonTrailEmitter;

	explodeOnDeath = true;
	explodeOnPlayerImpact = false;

	muzzleVelocity      = 200;
	velInheritFactor    = 0;

	armingDelay         = 1000;
	lifetime            = 10000;
	fadeDelay           = 9900;
	bounceElasticity    = 0.15;
	bounceFriction      = 0.5;
	isBallistic         = true;
	gravityMod = 1.0;

	hasLight    = true;
	lightRadius = 3.0;
	lightColor  = "1 1 0";
};

datablock ShapeBaseImageData(T2BeowulfCannonImage : T2BeowulfGunImage)
{
	shapeFile = "./dts/Beowulf_mortar.dts";
	
	mountPoint = 0;

	fireSound = T2BeowulfFireSound;
 
	projectileSpread = 0.1;
	projectileSpeed = 100;
	projectile = T2BeowulfCannonProjectile;
	projectileHitscan = false;
	
	energyDrain = 20;
	minEnergy = 25;

	rotation = eulerToMatrix("-90 0 0");
	
	stateName[0]                    = "Ready";
	stateTransitionOnNotLoaded[0]   = "Fire";
	stateTransitionOnNoAmmo[0]      = "AmmoCheck";
	stateTimeoutValue[0]            = 0.1;
	stateSequence[0]                = "root";
	stateTransitionOnTimeout[0]     = "Ready";
	stateScript[0]                  = "onReadyLoop";

	stateName[1]                    = "Fire";
	stateSequence[1]                = "fire";
	stateTransitionOnTimeout[1]     = "Delay";
	stateWaitForTimeout[1]          = True;
	stateTimeoutValue[1]            = 0.4;
	stateScript[1]                  = "onFire";

	stateName[2]                    = "Delay";
	stateTransitionOnTimeout[2]     = "AmmoCheck";
	stateWaitForTimeout[2]          = True;
	stateTimeoutValue[2]            = 0.6;
	stateScript[2]                  = "onDelay";

	stateName[3]                    = "AmmoCheck";
	stateTransitionOnTimeout[3]     = "AmmoCheckB";
	stateWaitForTimeout[3]          = True;
	stateTimeoutValue[3]            = 0.1;
	stateScript[3]                  = "onAmmoCheck";
	
	stateName[4]                    = "AmmoCheckB";
	stateTransitionOnAmmo[4]        = "Ready";
	stateTransitionOnNoAmmo[4]      = "AmmoCheck";
	stateTransitionOnTimeout[4]     = "Ready";
	stateTimeoutValue[4]            = 0.033;
};

function T2BeowulfCannonImage::onReadyLoop(%img, %obj, %slot) { T2BeowulfGunImage::onReadyLoop(%img, %obj, %slot); }

function T2BeowulfCannonImage::onFire(%i, %o, %s) { T2BeowulfGunImage::onFire(%i, %o, %s); }

function T2BeowulfCannonImage::onDelay(%i, %o, %s)
{
	
}

function T2BeowulfCannonImage::onAmmoCheck(%i, %o, %s) { T2BeowulfGunImage::onAmmoCheck(%i, %o, %s); }


// -- JET FX --

datablock ParticleData(T2BeowulfJetParticle)
{
	dragCoefficient      = 3;
	windCoefficient     = 0;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 0.25;
	constantAcceleration = 0.0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS           = 300;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	useInvAlpha = false;

	colors[0]     = "0.9 0.5 0.1 0.2";
	colors[1]     = "0.2 0.2 0.2 0";
	sizes[0]      = 2;
	sizes[1]      = 5;
};

datablock ParticleEmitterData(T2BeowulfJetEmitter)
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
	particles = "T2BeowulfJetParticle";
};

datablock ShapeBaseImageData(T2BeowulfJetAImage : T2Contrail2Image)
{
	mountPoint = 3;
	stateEmitter[1]					= T2BeowulfJetEmitter;
	stateEmitter[2]					= T2BeowulfJetEmitter;
};

datablock ShapeBaseImageData(T2BeowulfJetBImage : T2Contrail2Image)
{
	mountPoint = 4;
	stateEmitter[1]					= T2BeowulfJetEmitter;
	stateEmitter[2]					= T2BeowulfJetEmitter;
};


// -- FINAL EXPLOSION --

datablock DebrisData(T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_head.dts";
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

datablock ExplosionData(T2BeowulfFinalExplosionE)
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

	debris = T2BeowulfFinalExplosionDebrisE;
	debrisNum = 1;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2BeowulfFinalExplosionDebrisF : T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_bodyBack.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2BeowulfFinalExplosionF : T2BeowulfFinalExplosionE)
{
	debris = T2BeowulfFinalExplosionDebrisF;
	debrisNum = 1;
};

datablock DebrisData(T2BeowulfFinalExplosionDebrisD : T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_bodyFront.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2BeowulfFinalExplosionD : T2BeowulfFinalExplosionE)
{
	debris = T2BeowulfFinalExplosionDebrisD;
	debrisNum = 1;
};

datablock DebrisData(T2BeowulfFinalExplosionDebrisC : T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_wing.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2BeowulfFinalExplosionC : T2BeowulfFinalExplosionE)
{
	debris = T2BeowulfFinalExplosionDebrisC;
	debrisNum = 2;
};

datablock DebrisData(T2BeowulfFinalExplosionDebrisB : T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_thruster.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2BeowulfFinalExplosionB : T2BeowulfFinalExplosionE)
{
	debris = T2BeowulfFinalExplosionDebrisB;
	debrisNum = 2;

	subExplosion[0] = T2BeowulfFinalExplosionD;
	subExplosion[1] = T2BeowulfFinalExplosionE;
};

datablock DebrisData(T2BeowulfFinalExplosionDebrisA : T2BeowulfFinalExplosionDebrisE)
{
	shapeFile = "./dts/Beowulf_wreck_bodyBack.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2BeowulfFinalExplosion : T2BeowulfFinalExplosionE)
{
	subExplosion[0] = T2BeowulfFinalExplosionB;
	subExplosion[1] = T2BeowulfFinalExplosionC;

	debris = T2BeowulfFinalExplosionDebrisA;
	debrisNum = 1;
};

datablock ProjectileData(T2BeowulfFinalExplosionProjectile)
{
	explosion         = T2BeowulfFinalExplosion;

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