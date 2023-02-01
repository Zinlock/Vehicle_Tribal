// -- SOUNDS --

datablock AudioProfile(T2ShrikeIdleSound)
{
	filename    = "./wav/Shrike_engine.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2ShrikeEngineSound)
{
	filename    = "./wav/Shrike_boost.wav";
	description = AudioCloseLooping3d;
	preload = true;
};

datablock AudioProfile(T2ShrikeFireSound)
{
	filename    = "./wav/Shrike_blaster.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(T2ShrikeImpactSound)
{
	filename    = "./wav/Shrike_blaster_impact.wav";
	description = AudioClose3d;
	preload = true;
};


// -- WEAPON FX --

datablock ParticleData(T2ShrikeBlasterImpactParticle)
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

datablock ParticleEmitterData(T2ShrikeBlasterImpactEmitter)
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

	particles = "T2ShrikeBlasterImpactParticle";
};

// -- WEAPON --

datablock ExplosionData(T2ShrikeBlasterExplosion : gunExplosion)
{
	soundProfile = T2ShrikeImpactSound;

	lifeTimeMS = 150;

	particleEmitter = T2ShrikeBlasterImpactEmitter;
	particleDensity = 10;
	particleRadius = 0.0;

	damageRadius = 6;
	radiusDamage = 50;
};

datablock ProjectileData(T2ShrikeBlasterProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	directDamage = 10;
	directDamageType    = $DamageType::Gun;
	radiusDamageType    = $DamageType::Gun;

	brickExplosionRadius = 0;
	brickExplosionImpact = false;
	brickExplosionForce  = 0;
	brickExplosionMaxVolume = 0;
	brickExplosionMaxVolumeFloating = 0;

	impactImpulse	     = 400;
	verticalImpulse	   = 400;
	explosion = T2ShrikeBlasterExplosion;
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
	lightColor  = "0 0 1";
};

datablock StaticShapeData(T2ShrikeBlasterTracer) { shapeFile = "./dts/electric_trail.dts"; };

function T2ShrikeBlasterTracer::onAdd(%this,%obj)
{
  %obj.schedule(0, playThread, 2, root);
  %obj.schedule(2000,delete);
}

datablock ShapeBaseImageData(T2ShrikeBlasterAImage)
{
	class = "WeaponImage";

	shapeFile = "./dts/shrike_gun.dts";
	emap = false;

	mountPoint = 1;
	rotation = "0 0 1 0";

	projectile = T2ShrikeBlasterProjectile;
	projectileCount = 1;
	projectileSpread = 0.2;
	projectileSpeed = 200;

	projectileHitscan = true;
	projectileHitscanRange = 600;
	projectileScale = 1;
	tracerSize = -3; // negative value flips the tracer
	tracerData = T2ShrikeBlasterTracer;

	energyDrain = 8;
	minEnergy = 15;

	stateName[0]                    = "Ready";
	stateTransitionOnNotLoaded[0]   = "AmmoCheck"; // using loaded instead of trigger because vehicle image triggers cant be held for some reason
	stateTimeoutValue[0]            = 0.1;            // NotLoaded is trigger down, not the other way around
	stateTransitionOnTimeout[0]     = "Ready";
	stateScript[0]                  = "onReadyLoop";

	stateName[1]                    = "Fire";
	stateSequence[1]                = "fire";
	stateTransitionOnTimeout[1]     = "Delay";
	stateWaitForTimeout[1]          = True;
	stateTimeoutValue[1]            = 0.15;
	stateScript[1]                  = "onFire";

	stateName[2]                    = "Delay";
	stateTransitionOnTimeout[2]     = "Ready";
	stateWaitForTimeout[2]          = True;
	stateTimeoutValue[2]            = 0.3;
	stateScript[2]                  = "onDelay";

	stateName[3]                    = "AmmoCheck";
	stateTransitionOnTimeout[3]     = "AmmoCheckB";
	stateWaitForTimeout[3]          = True;
	stateTimeoutValue[3]            = 0.033;
	stateScript[3]                  = "onAmmoCheck";
	
	stateName[4]                    = "AmmoCheckB";
	stateTransitionOnAmmo[4]        = "Fire";
	stateTransitionOnTimeout[4]     = "Ready";
	stateTimeoutValue[4]            = 0.033;
};

function T2ShrikeBlasterAImage::onMount(%img, %obj, %slot)
{
	// %obj.useExtraPrints = true;
}

function T2ShrikeBlasterAImage::onFire(%img, %obj, %slot)
{
	if(isObject(%pl = %obj.getControllingObject()) || isObject(%pl = %obj.lastMountedPlayer))
	{
		%obj.setEnergyLevel(%obj.getEnergyLevel() - %img.energyDrain);
		%obj.stopAudio(0);
		%obj.playAudio(0, T2ShrikeFireSound);

		if(%img.projectileHitscan)
			RaycastFire(%img.projectile, %obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot), %img.projectileSpread, %img.projectileCount, %slot, %pl, %pl.Client, %img, %img.projectileHitscanRange);
		else
			ProjectileFire(%img.projectile, %obj.getMuzzlePoint(%slot), %obj.getMuzzleVector(%slot), %img.projectileSpread, %img.projectileCount, %slot, %pl, %pl.Client, %img.projectileSpeed);
	}
}

function T2ShrikeBlasterAImage::onDelay(%img, %obj, %slot)
{
	%obj.setImageLoaded(1, 0);
}

function T2ShrikeBlasterAImage::onAmmoCheck(%img, %obj, %slot)
{
	if(%obj.getEnergyLevel() > %img.minEnergy)
		%obj.setImageAmmo(%slot, 1);
	else
	{
		%obj.setImageAmmo(%slot, 0);
		%obj.setImageLoaded(1, 1);
	}
}

datablock ShapeBaseImageData(T2ShrikeBlasterBImage : T2ShrikeBlasterAImage)
{
	mountPoint = 2;
};

function T2ShrikeBlasterBImage::onFire(%i, %o, %s) { T2ShrikeBlasterAImage::onFire(%i, %o, %s); }

function T2ShrikeBlasterBImage::onDelay(%i, %o, %s)
{
	%o.setImageLoaded(%s, 1);
}

function T2ShrikeBlasterBImage::onAmmoCheck(%i, %o, %s) { T2ShrikeBlasterAImage::onAmmoCheck(%i, %o, %s); }


// -- JET FX --

datablock ParticleData(T2ShrikeJetParticle)
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

datablock ParticleEmitterData(T2ShrikeJetEmitter)
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
	particles = "T2ShrikeJetParticle";
};

datablock ShapeBaseImageData(T2ShrikeJetImage : T2Contrail2Image)
{
	mountPoint = 3;
	stateEmitter[1]					= T2ShrikeJetEmitter;
	stateEmitter[2]					= T2ShrikeJetEmitter;
};


// -- FINAL EXPLOSION --

datablock DebrisData(T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_tube.dts";
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

datablock ExplosionData(T2ShrikeFinalExplosionE)
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

	debris = T2ShrikeFinalExplosionDebrisE;
	debrisNum = 2;
	debrisNumVariance = 0;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 35;
	debrisVelocity = 17;
	debrisVelocityVariance = 5;
};

datablock DebrisData(T2ShrikeFinalExplosionDebrisF : T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_bodyTop.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ShrikeFinalExplosionF : T2ShrikeFinalExplosionE)
{
	debris = T2ShrikeFinalExplosionDebrisF;
	debrisNum = 1;
};

datablock DebrisData(T2ShrikeFinalExplosionDebrisD : T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_thruster.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ShrikeFinalExplosionD : T2ShrikeFinalExplosionE)
{
	debris = T2ShrikeFinalExplosionDebrisD;
	debrisNum = 1;
};

datablock DebrisData(T2ShrikeFinalExplosionDebrisC : T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_wing.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ShrikeFinalExplosionC : T2ShrikeFinalExplosionE)
{
	debris = T2ShrikeFinalExplosionDebrisC;
	debrisNum = 2;
};

datablock DebrisData(T2ShrikeFinalExplosionDebrisB : T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_wingHolder.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ShrikeFinalExplosionB : T2ShrikeFinalExplosionE)
{
	debris = T2ShrikeFinalExplosionDebrisB;
	debrisNum = 2;

	subExplosion[0] = T2ShrikeFinalExplosionD;
	subExplosion[1] = T2ShrikeFinalExplosionE;
};

datablock DebrisData(T2ShrikeFinalExplosionDebrisA : T2ShrikeFinalExplosionDebrisE)
{
	shapeFile = "./dts/Shrike_wreck_body.dts";
	elasticity = 0.6;
	friction = 0.1;
	numBounces = 4;
};

datablock ExplosionData(T2ShrikeFinalExplosion : T2ShrikeFinalExplosionE)
{
	subExplosion[0] = T2ShrikeFinalExplosionB;
	subExplosion[1] = T2ShrikeFinalExplosionC;

	debris = T2ShrikeFinalExplosionDebrisA;
	debrisNum = 1;
};

datablock ProjectileData(T2ShrikeFinalExplosionProjectile)
{
	explosion         = T2ShrikeFinalExplosion;

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