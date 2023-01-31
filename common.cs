// -- SOUNDS --

datablock AudioProfile(T2VehicleImpactSound)
{
	filename    = "./wav/crash_soft.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleImpactHardSound)
{
	filename    = "./wav/crash_hard.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleSplashLightSound)
{
	filename    = "./wav/splash_light.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleSplashMediumSound)
{
	filename    = "./wav/splash_medium.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleSplashHeavySound)
{
	filename    = "./wav/splash_heavy.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleSplashExitSound)
{
	filename    = "./wav/splash_exit.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(T2VehicleDestroySound)
{
	filename    = "./wav/vehicle_explosion.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock AudioProfile(Turret_ShieldDamagedSound)
{
	fileName = "./wav/base_shield_damage.wav";
	description = AudioClose3D;
	preload = true;
};

datablock AudioProfile(Turret_ShieldDestroyedSound)
{
	fileName = "./wav/base_shield_destroy.wav";
	description = AudioClose3D;
	preload = true;
};


// -- SHAPES --

datablock StaticShapeData(Turret_EnergyShieldShape) { shapeFile = "./dts/energy_shield.dts"; };


// -- CONTRAIL --

datablock ParticleData(T2ContrailParticle)
{
	dragCoefficient      = 3;
	windCoefficient     = 0;
	gravityCoefficient   = -1;
	inheritedVelFactor   = 1.0;
	constantAcceleration = 0.0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS           = 500;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	useInvAlpha = false;
	
	colors[0]     = "0.3 0.3 0.3 0.4";
	colors[1]     = "0.0 0.0 0.1 0";
	sizes[0]      = 0.3;
	sizes[1]      = 1.0;
};

datablock ParticleEmitterData(T2ContrailEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2ContrailParticle";
};

datablock ShapeBaseImageData(T2Contrail2Image)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = 2;
	rotation = "0 0 1 0";

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "A";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "A";
	stateTransitionOnTimeout[1]		= "B";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 10;
	stateEmitter[1]					= T2ContrailEmitter;
	stateEmitterTime[1]				= 10;

	stateName[2]					= "B";
	stateTransitionOnTimeout[2]		= "A";
	stateWaitForTimeout[2]			= True;
	stateTimeoutValue[2]			= 10;
	stateEmitter[2]					= T2ContrailEmitter;
	stateEmitterTime[2]				= 10;
};

datablock ShapeBaseImageData(T2Contrail3Image : T2Contrail2Image)
{
	mountPoint = 3;
};


// -- EXPLOSION --

datablock ParticleData(T2VehicleExplosionHazeParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = 0;
	windCoefficient		= 0.5;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 1200;
	lifetimeVarianceMS   = 500;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "1 1 1 0.0";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "0.1 0.05 0.025 0.1";
	colors[3]     = "0.1 0.05 0.025 0.0";

	sizes[0]	= 7.0;
	sizes[1]	= 9.3;
	sizes[2]	= 7.5;
	sizes[3]	= 9.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = false;
};

datablock ParticleEmitterData(T2VehicleExplosionHazeEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 18;
	velocityVariance = 4.0;
	ejectionOffset   = 0.4;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehicleExplosionHazeParticle";
};

datablock ParticleData(T2VehicleExplosionCloudParticle)
{
	dragCoefficient		= 0.3;
	windCoefficient		= 1.0;
	gravityCoefficient	= 0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 1500;
	lifetimeVarianceMS	= 500;
	spinSpeed		= 5.0;
	spinRandomMin		= -5.0;
	spinRandomMax		= 5.0;
	useInvAlpha		= false;
	animateTexture		= false;

	textureName		= "base/data/particles/cloud";
	
	colors[0]     = "0.1 0.1 0.1 0.9";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 4;
	sizes[1]      = 6;
};

datablock ParticleEmitterData(T2VehicleExplosionCloudEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   lifeTimeMS	   = 90;
   ejectionVelocity = 7;
   velocityVariance = 1.0;
   ejectionOffset   = 1.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "T2VehicleExplosionCloudParticle";
};

datablock ParticleData(T2VehicleExplosionSmokeParticle)
{
	dragCoefficient      = 1.78;
	gravityCoefficient   = -0.35;
	windCoefficient		= 2.5;
	inheritedVelFactor   = 0;
	constantAcceleration = 0.0;
	lifetimeMS           = 4000;
	lifetimeVarianceMS   = 1000;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	colors[0]     = "0.4 0.4 0.4 0.4";
	colors[1]     = "0.1 0.1 0.1 0.2";
	colors[2]     = "0.05 0.05 0.05 0.1";
	colors[3]     = "0.05 0.05 0.05 0.0";

	sizes[0]	= 6.0;
	sizes[1]	= 5.3;
	sizes[2]	= 10.5;
	sizes[3]	= 9.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(T2VehicleExplosionSmokeEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 15;
	velocityVariance = 1.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehicleExplosionSmokeParticle";
};

datablock ParticleData(T2VehicleExplosionDustParticle)
{
	dragCoefficient		= 1.0;
	windCoefficient		= 1.0;
	gravityCoefficient	= 0.4;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 2000;
	lifetimeVarianceMS	= 1000;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/dot";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]     = "1 0.5 0.0 1";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "1 1 1 0.0";

	sizes[0]	= 0.6;
	sizes[1]	= 0.5;
	sizes[2]	= 0.4;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(T2VehicleExplosionDustEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	lifetimeMS       = 140;
	ejectionVelocity = 12;
	velocityVariance = 12.0;
	ejectionOffset   = 1.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehicleExplosionDustParticle";
};

datablock ParticleData(T2VehicleExplosionFlashParticle)
{
	dragCoefficient		= 0.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 128;
	lifetimeVarianceMS	= 32;
	spinSpeed		= 100.0;
	spinRandomMin		= -100.0;
	spinRandomMax		= 100.0;
	useInvAlpha		= false;
	animateTexture		= false;

	textureName		= "base/data/particles/star1";

	// Interpolation variables
	colors[0]     = "1 0.5 0.0 1";
	colors[1]     = "0.9 0.5 0.0 0.9";
	colors[2]     = "1 1 1 0.0";

	sizes[0]	= 16;
	sizes[1]	= 17;
	sizes[2]	= 18;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(T2VehicleExplosionFlashEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	lifetimeMS       = 6;
	ejectionVelocity = 0;
	velocityVariance = 0;
	ejectionOffset   = 0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehicleExplosionFlashParticle";
};

datablock ParticleData(T2VehicleExplosionShrapnelParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -1.5;
	windCoefficient		= 2;
	inheritedVelFactor   = 0.5;
	constantAcceleration = 0.0;
	lifetimeMS           = 3500;
	lifetimeVarianceMS   = 1000;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	colors[0]     = "0.3 0.3 0.3 0.2";
	colors[1]     = "0.1 0.1 0.1 0.1";
	colors[2]     = "0.05 0.05 0.05 0.05";
	colors[3]     = "0.05 0.05 0.05 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 3.0;
	sizes[2]	= 5.0;
	sizes[3]	= 7.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(T2VehicleExplosionShrapnelEmitter)
{
	ejectionPeriodMS = 10;
	periodVarianceMS = 5;
	ejectionVelocity = 4;
	velocityVariance = 3.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehicleExplosionShrapnelParticle";
};

datablock DebrisData(T2VehicleExplosionShrapnelDebris)
{
	shapeFile = "base/data/shapes/empty.dts";
	lifetime = 0.5;
	minSpinSpeed = -200.0;
	maxSpinSpeed = -100.0;
	elasticity = 1.0;
	friction = 0.1;
	numBounces = 2;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	emitters = T2VehicleExplosionShrapnelEmitter;

	gravModifier = 0.5;
};

datablock ExplosionData(T2VehicleInitialExplosion)
{
	explosionShape = "Add-Ons/Weapon_Rocket_Launcher/explosionSphere1.dts";
	lifeTimeMS = 150;

	soundProfile = T2VehicleDestroySound;

	particleEmitter = T2VehicleExplosionSmokeEmitter;
	particleDensity = 250;
	particleRadius = 3.0;

	emitter[0] = T2VehicleExplosionCloudEmitter;
	emitter[1] = T2VehicleExplosionDustEmitter;
	emitter[2] = T2VehicleExplosionHazeEmitter;
	emitter[3] = T2VehicleExplosionFlashEmitter;

	debris = T2VehicleExplosionShrapnelDebris;
	debrisNum = 8;
	debrisNumVariance = 2;
	debrisPhiMin = 0;
	debrisPhiMax = 360;
	debrisThetaMin = 0;
	debrisThetaMax = 180;
	debrisVelocity = 20;
	debrisVelocityVariance = 10;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "4.0 4.0 4.0";
	camShakeAmp = "19.8 19.8 19.8";
	camShakeDuration = 3.8;
	camShakeRadius = 20.0;

	// Dynamic light
	lightStartRadius = 2;
	lightEndRadius = 8;
	lightStartColor = "0.35 0.2 0.1";
	lightEndColor = "0 0 0";

	//impulse
	impulseRadius = 16;
	impulseForce = 600;

	damageRadius = 10;
	radiusDamage = 70;

	uiName = "";
};

datablock ProjectileData(T2VehicleInitialExplosionProjectile)
{
	directDamageType  = $DamageType::VehicleExplosion;
	radiusDamageType  = $DamageType::VehicleExplosion;
	explosion         = T2VehicleInitialExplosion;

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

datablock ParticleData(T2VehicleDamageParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -2;
	windCoefficient		= 2;
	inheritedVelFactor   = 0.5;
	constantAcceleration = 0.0;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 1000;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= true;
	colors[0]     = "0.3 0.3 0.3 0.0";
	colors[1]     = "0.1 0.1 0.1 0.1";
	colors[2]     = "0.05 0.05 0.05 0.05";
	colors[3]     = "0.05 0.05 0.05 0.0";

	sizes[0]	= 1.0;
	sizes[1]	= 3.0;
	sizes[2]	= 5.0;
	sizes[3]	= 7.5;

	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 0.8;
	times[3]	= 1.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(T2VehicleDamageEmitter)
{
	ejectionPeriodMS = 27;
	periodVarianceMS = 6;
	ejectionVelocity = 5;
	velocityVariance = 3.0;
	ejectionOffset   = 0.0;
	thetaMin         = 0;
	thetaMax         = 180;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = true;
	particles = "T2VehicleDamageParticle";
};


// -- SUPPORT FUNCTIONS --

function RaycastFire(%db, %start, %dir, %spd, %amt, %srcSlot, %srcObj, %srcCli, %srcImg, %range)
{
	%spread = %spd / 1000;
	%shellcount = %amt;

	%shells = -1;
	
	%dir = vectorNormalize(%dir);

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%velocity = VectorScale(%dir, 200);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%vel = vectorNormalize(MatrixMulVector(%mat, %velocity));

		%end = vectorAdd(%start, vectorScale(%vel, %range));
		%mask = $TypeMasks::FxBrickObjectType |
						$TypeMasks::PlayerObjectType |
						$TypeMasks::VehicleObjectType |
						$TypeMasks::TerrainObjectType |
						$TypeMasks::StaticObjectType;
		%start = vectorAdd(%start, vectorScale(%vel, -%range/1000));

		%dist = vectorDist(%start, %end);
		%int = 100;
		%cts = mFloor(%dist / %int);

		for(%i = 0; %i < %cts; %i++)
		{
			%point = vectorAdd(%start, vectorScale(%vel, %int * %i));
			%ray = containerRayCast(%point, vectorAdd(%point, vectorScale(%vel, %int)), %mask, %srcObj, %srcObj.getObjectMount());
			%col = getWord(%ray, 0);

			if(!isObject(%col))
				%pos = %end;
			else
			{
				%pos = getWords(%ray, 1, 3);
			
				%realProjectile = new Projectile()
				{
					datablock = %db;
					initialPosition = vectorAdd(%pos, vectorScale(%vel, -0.1));
					initialVelocity = vectorScale(%vel, 200);
					client = %srcCli;
					sourceObject = %srcObj;
					minigame = %srcCli.minigame;
					sourceSlot = %srcSlot;
					scale = %srcImg.projectileScale;
				};

				%shells = %shells TAB %realProjectile;

				%realProjectile.dontHurtShooter = true;
				break;
			}
		}

		if(%srcImg.tracerSize != 0)
		{
			%shape = new StaticShape()
			{
				datablock = %srcImg.tracerData;
			};
			MissionCleanup.add(%shape);

			if(%srcImg.tracerSize > 0)
			{
				%x = getWord(%vel,0) / 2;
				%y = (getWord(%vel,1) + 1) / 2;
				%z = getWord(%vel,2) / 2;

				%shape.setTransform(%start SPC VectorNormalize(%x SPC %y SPC %z) SPC mDegToRad(180));
			}
			else
			{
				%x = (-1*getWord(%vel,0)) / 2;
				%y = ((-1*getWord(%vel,1)) + 1) / 2;
				%z = (-1*getWord(%vel,2)) / 2;

				%shape.setTransform(%pos SPC VectorNormalize(%x SPC %y SPC %z) SPC mDegToRad(180));
			}
			
			%shape.setScale(mAbs(%srcImg.tracerSize) SPC vectorDist(%start,%pos) SPC mAbs(%srcImg.tracerSize));
		}
	}

	return removeField(%shells, 0);
}

function ProjectileFire(%db, %pos, %vec, %spd, %amt, %srcSlot, %srcObj, %srcCli, %vel)
{	
	%projectile = %db;
	%spread = %spd / 1000;
	%shellcount = %amt;

	if(%vel $= "")
		%vel = %projectile.muzzleVelocity;

	%shells = -1;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%velocity = VectorScale(%vec, %vel);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new Projectile()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %pos;
			sourceObject = %srcObj;
			sourceSlot = %srcSlot;
			sourceInv = %srcObj.currTool;
			client = %srcCli;
		};
		MissionCleanup.add(%p);

		%shells = %shells TAB %p;
	}

	return removeField(%shells, 0);
}