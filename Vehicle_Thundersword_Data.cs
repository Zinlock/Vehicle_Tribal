// -- SOUNDS --

datablock AudioProfile(T2ThunderswordActivateSound)
{
	filename    = "./wav/Thundersword_activate.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordIdleSound)
{
	filename    = "./wav/Thundersword_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordEngineSound)
{
	filename    = "./wav/Thundersword_boost.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordFireSound)
{
	filename    = "./wav/Thundersword_blaster.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordImpactSound)
{
	filename    = "./wav/Thundersword_blaster_impact.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordBombBaySound)
{
	filename    = "./wav/Thundersword_bomb_bay.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordBombActivateSound)
{
	filename    = "./wav/Thundersword_bomb_activate.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordBombSound)
{
	filename    = "./wav/Thundersword_bomb.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2ThunderswordBombImpactSound)
{
	filename    = "./wav/Thundersword_bomb_impact.wav";
	description = AudioExplosionFar3D;
	preload = true;
};


// -- WEAPON FX --

datablock ParticleData(T2ThunderswordBlasterImpactParticle)
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

datablock ParticleEmitterData(T2ThunderswordBlasterImpactEmitter)
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

	particles = "T2ThunderswordBlasterImpactParticle";
};

// -- WEAPON --

// datablock ExplosionData(T2ThunderswordBlasterExplosion : gunExplosion)
// {
// 	soundProfile = T2ThunderswordImpactSound;

// 	lifeTimeMS = 150;

// 	particleEmitter = T2ThunderswordBlasterImpactEmitter;
// 	particleDensity = 10;
// 	particleRadius = 0.0;

// 	damageRadius = 3;
// 	radiusDamage = 15;
// };

// datablock ProjectileData(T2ThunderswordBlasterProjectile)
// {
// 	projectileShapeName = "base/data/shapes/empty.dts";

// 	directDamage = 10;
// 	directDamageType    = $DamageType::Gun;
// 	radiusDamageType    = $DamageType::Gun;

// 	brickExplosionRadius = 0;
// 	brickExplosionImpact = false;
// 	brickExplosionForce  = 0;
// 	brickExplosionMaxVolume = 0;
// 	brickExplosionMaxVolumeFloating = 0;

// 	impactImpulse	     = 400;
// 	verticalImpulse	   = 400;
// 	explosion = T2ThunderswordBlasterExplosion;
// 	particleEmitter     = "";

// 	muzzleVelocity      = 200;
// 	velInheritFactor    = 0;

// 	armingDelay         = 0;
// 	lifetime            = 4000;
// 	fadeDelay           = 3500;
// 	bounceElasticity    = 0.5;
// 	bounceFriction      = 0.20;
// 	isBallistic         = false;
// 	gravityMod = 0.0;

// 	hasLight    = true;
// 	lightRadius = 3.0;
// 	lightColor  = "0 0 1";
// };

// datablock StaticShapeData(T2ThunderswordBlasterTracer) { shapeFile = "./dts/electric_trail.dts"; };

// function T2ThunderswordBlasterTracer::onAdd(%this,%obj)
// {
//   %obj.schedule(0, playThread, 2, root);
//   %obj.schedule(2000,delete);
// }

datablock ShapeBaseImageData(T2ThunderswordBlasterImage)
{
	class = "WeaponImage";

	shapeFile = "./dts/Thundersword_gun.dts";
	emap = false;

	mountPoint = 0;
	rotation = "0 0 1 0";

	projectile = T2ShrikeBlasterProjectile; //T2ThunderswordBlasterProjectile;
	projectileCount = 1;
	projectileSpread = 0.2;
	projectileSpeed = 200;

	projectileHitscan = true;
	projectileHitscanRange = 600;
	projectileScale = 1;
	tracerSize = 3;
	tracerData = T2ShrikeBlasterTracer; //T2ThunderswordBlasterTracer;

	fireSound = T2ThunderswordFireSound;

	energyDrain = 4;
	minEnergy = 16;

	stateName[0]                    = "Ready";
	stateTransitionOnNotLoaded[0]   = "FireA";
	stateTransitionOnNoAmmo[0]      = "AmmoCheck";
	stateTimeoutValue[0]            = 0.1;
	stateSequence[0]                = "root";
	stateTransitionOnTimeout[0]     = "Ready";
	stateScript[0]                  = "onReadyLoop";

	stateName[1]                    = "FireA";
	stateSequence[1]                = "fireR";
	stateTransitionOnTimeout[1]     = "FireB";
	stateWaitForTimeout[1]          = True;
	stateTimeoutValue[1]            = 0.1;
	stateScript[1]                  = "onFire";
	
	stateName[5]                    = "FireB";
	stateSequence[5]                = "fireL";
	stateTransitionOnTimeout[5]     = "Delay";
	stateWaitForTimeout[5]          = True;
	stateTimeoutValue[5]            = 0.1;
	stateScript[5]                  = "onFire";

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

function T2ThunderswordBlasterImage::onMount(%img, %obj, %slot)
{
	%obj.sourceObject.useExtraPrints = true;
	%obj.sourceObject.extraPrintLabel = "RDS";
	%obj.sourceObject.extraPrintColor = "FFEE44";
}

function T2ThunderswordBlasterImage::onReadyLoop(%img, %obj, %slot)
{
	%obj.sourceObject.extraPrintText = mCeil((%obj.getEnergyLevel() / %obj.getDataBlock().maxEnergy) * 100) @ "%";
	%img.onAmmoCheck(%obj, %slot);
}

function T2ThunderswordBlasterImage::onFire(%img, %obj, %slot)
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

function T2ThunderswordBlasterImage::onAmmoCheck(%img, %obj, %slot)
{
	if(%obj.getEnergyLevel() > %img.minEnergy)
		%obj.setImageAmmo(%slot, 1);
	else
		%obj.setImageAmmo(%slot, 0);
}

datablock ExplosionData(T2ThunderswordBombExplosion : T2BeowulfCannonExplosion)
{
	soundProfile = T2ThunderswordBombImpactSound;

	impulseRadius = 18;
	radiusImpulse = 1000;

	damageRadius = 16;
	radiusDamage = 75;
};

datablock ProjectileData(T2ThunderswordBombProjectile)
{
	projectileShapeName = "./dts/thundersword_bomb.dts";

	directDamage = 125;
	directDamageType    = $DamageType::Direct;
	radiusDamageType    = $DamageType::Radius;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	soundProfile = T2ThunderswordBombSound;

	impactImpulse	     = 400;
	verticalImpulse	   = 400;
	explosion = T2ThunderswordBombExplosion;
	particleEmitter     = "";

	muzzleVelocity      = 0;
	velInheritFactor    = 0;

	armingDelay         = 64;
	lifetime            = 10000;
	fadeDelay           = 9900;
	bounceElasticity    = 0.5;
	bounceFriction      = 0.20;
	isBallistic         = true;
	gravityMod = 1.0;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 1";
};

datablock ShapeBaseImageData(T2ThunderswordBomberImage : T2ThunderswordBlasterImage)
{
	class = "WeaponImage";

	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = 0;
	rotation = "0 0 1 0";

	fireSound = T2ThunderswordBombBaySound;

	projectile = T2ThunderswordBombProjectile;
	projectileCount = 1;
	projectileSpread = 0;
	projectileSpeed = 8;

	energyDrain = 20;
	minEnergy = 20;

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
	stateTimeoutValue[1]            = 0.35;
	stateScript[1]                  = "onFire";

	stateName[2]                    = "Delay";
	stateTransitionOnLoaded[2]     = "AmmoCheck";
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

function T2ThunderswordBomberImage::onMount(%img, %obj, %slot) { T2ThunderswordBlasterImage::onMount(%img, %obj, %slot); }

function T2ThunderswordBomberImage::onReadyLoop(%img, %obj, %slot) { T2ThunderswordBlasterImage::onReadyLoop(%img, %obj, %slot); }

function T2ThunderswordBomberImage::onFire(%img, %obj, %slot)
{
	%obj.sourceObject.stopAudio(1);
	%obj.sourceObject.playAudio(1, %img.fireSound);
	%obj.sourceObject.playThread(1, bomb);

	if(isObject(%pl = %obj.lastMountedPlayer))
	{
		%obj.setEnergyLevel(%obj.getEnergyLevel() - %img.energyDrain);
		serverPlay3D(%obj.sourceObject.getSlotTransform(3), T2ThunderswordBombActivateSound);

		%img.onReadyLoop(%obj, %slot);

		%so = %obj.sourceObject;
		ProjectileFire(%img.projectile, vectorAdd(%so.getSlotTransform(3), vectorScale(%so.getForwardVector(), -4)), vectorNormalize(vectorAdd(%obj.getForwardVector(), %obj.getVelocity())),
		%img.projectileSpread, %img.projectileCount, %slot, %pl, %pl.Client, vectorLen(%so.getVelocity()));
	}
}

function T2ThunderswordBomberImage::onAmmoCheck(%img, %obj, %slot) { T2ThunderswordBlasterImage::onAmmoCheck(%img, %obj, %slot); }

// -- JET FX --

datablock ParticleData(T2ThunderswordJetParticle)
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

datablock ParticleEmitterData(T2ThunderswordJetEmitter)
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
	particles = "T2ThunderswordJetParticle";
};

datablock ShapeBaseImageData(T2ThunderswordJetImage : T2Contrail2Image)
{
	mountPoint = 3;
	stateEmitter[1]					= T2ThunderswordJetEmitter;
	stateEmitter[2]					= T2ThunderswordJetEmitter;
};


// -- FINAL EXPLOSION --

datablock DebrisData(T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_tube.dts";
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

datablock ExplosionData(T2ThunderswordFinalExplosionE)
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

	debris = T2ThunderswordFinalExplosionDebrisE;
	debrisNum = 2;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2ThunderswordFinalExplosionDebrisF : T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_bodyTop.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ThunderswordFinalExplosionF : T2ThunderswordFinalExplosionE)
{
	debris = T2ThunderswordFinalExplosionDebrisF;
	debrisNum = 1;
};

datablock DebrisData(T2ThunderswordFinalExplosionDebrisD : T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_thruster.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ThunderswordFinalExplosionD : T2ThunderswordFinalExplosionE)
{
	debris = T2ThunderswordFinalExplosionDebrisD;
	debrisNum = 1;
};

datablock DebrisData(T2ThunderswordFinalExplosionDebrisC : T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_wing.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ThunderswordFinalExplosionC : T2ThunderswordFinalExplosionE)
{
	debris = T2ThunderswordFinalExplosionDebrisC;
	debrisNum = 2;
};

datablock DebrisData(T2ThunderswordFinalExplosionDebrisB : T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_wingHolder.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ThunderswordFinalExplosionB : T2ThunderswordFinalExplosionE)
{
	debris = T2ThunderswordFinalExplosionDebrisB;
	debrisNum = 2;

	subExplosion[0] = T2ThunderswordFinalExplosionD;
	subExplosion[1] = T2ThunderswordFinalExplosionE;
};

datablock DebrisData(T2ThunderswordFinalExplosionDebrisA : T2ThunderswordFinalExplosionDebrisE)
{
	shapeFile = "./dts/Thundersword_wreck_body.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ThunderswordFinalExplosion : T2ThunderswordFinalExplosionE)
{
	subExplosion[0] = T2ThunderswordFinalExplosionB;
	subExplosion[1] = T2ThunderswordFinalExplosionC;

	debris = T2ThunderswordFinalExplosionDebrisA;
	debrisNum = 1;
};

datablock ProjectileData(T2ThunderswordFinalExplosionProjectile)
{
	explosion         = T2ThunderswordFinalExplosion;

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