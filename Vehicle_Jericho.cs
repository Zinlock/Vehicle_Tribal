exec("./Vehicle_Jericho_Data.cs");

// -- VEHICLE --

datablock WheeledVehicleData(T2JerichoVehicle : JeepVehicle)
{
	shapeFile = "./dts/jericho.dts";

	defaultColor = "0.609 0.539 0.406";
	paintable = true;

	cameraMaxDist = 12;
	cameraOffset = 9;
	cameraTilt = 0.4;
	cameraRoll = false;

	maxDamage = 400.00;
	destroyedLevel = 400.00;

	rechargeRate = 8 / 31.25;
	maxEnergy = 150;
	energyShield = 1.0;
	energyShape = Turret_EnergyShieldShape;
	energyScale = 6;
	energyDelay = 3;
	energySound = Turret_ShieldDamagedSound;
	energyBreakSound = Turret_ShieldDestroyedSound;

	useVehiclePrints[0] = true;
	useEnergyPrints = true;

	maxSteeringAngle = 0.9785;
	integration = 8;

	massBox = "0 0 0";
	massCenter = "0 0 0";

	mass = 800;
	density = 5.0;
	drag = 1.6;

	minDrag = 50;
	maxDrag = 40;

	numMountPoints = 1;
	mountThread[0] = "sit";
	maxMountDistance[0] = 3;
	mountToNearest = true;

	overrideNextSeat = true;
	overridePrevSeat = true;

	collDamageThresholdVel = 20.0;
	collDamageMultiplier   = 0.02;
	
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

	bodyFriction = 0.6;
	bodyRestitution = 0.6;
	
	softImpactSpeed = 15;
	softImpactSound = T2VehicleImpactSound;
	hardImpactSpeed = 25;
	hardImpactSound = T2VehicleImpactHardSound;

	minImpactSpeed = 25;
	minImpactDamage = 20;
	maxImpactDamage = 90;
	maxImpactDamage = 550;

	groundImpactMinSpeed = 10.0;
	speedDamageScale = 0.010;

	damageEmitter[0] = T2VehicleDamageEmitter;
	damageEmitterOffset[0] = "0.0 0.0 0.0";
	damageLevelTolerance[0] = 0.65;

	damageEmitter[1] = T2VehicleExplosionShrapnelEmitter;
	damageEmitterOffset[1] = "0.0 0.0 0.0";
	damageLevelTolerance[1] = 0.998;

	numDmgEmitterAreas = 3; // how does this work??

	initialExplosionProjectile = T2VehicleInitialExplosionProjectile;
	initialExplosionOffset = 0;

	burnTime = 32;

	finalExplosionProjectile = T2JerichoFinalExplosionProjectile;
	finalExplosionOffset = 0;

	useEngineSounds = true;
	engineSlot = 0;
	engineIdleSound = T2JerichoIdleSound;
	engineMoveSound = T2JerichoEngineSound;
	engineMoveSpeed = 15;
	engineBoostSound = T2JerichoEngineSound;
	engineBoostSpeed = 60;

	tireEmitter = VehicleTireEmitter; // All the tires use the same dust emitter

	useEyePoint = false;	

	defaultTire	= T2JerichoTire;
	defaultSpring	= T2JerichoSpring;

	numWheels = 6;

	engineTorque = 20000; // Engine power
	engineBrake = 4000; // Braking when throttle is 0
	brakeTorque = 50000; // When brakes are applied
	maxWheelSpeed = 10; // Engine scale by current speed / max speed

	rollForce = 900;
	yawForce = 600;
	pitchForce = 1000;
	rotationalDrag = 0.2;

	steeringAutoReturn = true;
	steeringAutoReturnRate = 0.9;
	steeringAutoReturnMaxSpeed = 10;
	steeringUseStrafeSteering = true;
	steeringStrafeSteeringRate = 0.1;

	jetForce = 0;
	minJetEnergy = 0;
	jetEnergyDrain = 0;

	lookUpLimit = 0.65;
	lookDownLimit = 0.45;

	protectPassengersRadius = true;

	rideable = 1;

	uiName = "T2: Jericho M.P.B.";

	vpadNeverTimeOut = true;

	isMPB = true;
	mpbDeployDelay = 3000;
	mpbTurretData = Turret_TribalBaseStand;

	mpbEnterCooldown = 3000;
	mpbMinExitTime = 1000;

	mpbHealthRegen = 3;
	mpbEnergyRegen = 6;

	mpbUseRadius = 1.5;
	mpbEscapeVelocity = 10;
};

function T2JerichoVehicle::onAdd(%db, %obj)
{
	Parent::onAdd(%db, %obj);

	%obj.mpbDeployed = false;
	%obj.mpbDriven = false;

	%obj.MPBDeployLoop();
}

function T2JerichoVehicle::onRemove(%db, %obj)
{
	Parent::onRemove(%db, %obj);

	if(isObject(%obj.mpbTurret))
		%obj.mpbTurret.delete();
}

function WheeledVehicle::MPBDeployLoop(%obj)
{
	%db = %obj.getDatablock();
	cancel(%obj.mdl);

	%vel = vectorLen(%obj.getVelocity());
	%ctrl = isObject(%obj.getControllingObject());

	if(%ctrl && !%obj.mpbDriven)
		%obj.mpbDriven = true;

	if(%obj.mpbDriven)
	{
		if(%obj.mpbDeployed)
		{
			if(%ctrl)
				%obj.MPBUndeploy();
			else
			{
				if(isObject(%col = %obj.stationUser))
				{
					if(%col.getObjectMount() != %obj.getId() && getSimTime() - %col.stationEnterTime >= %db.mpbMinExitTime)
					{
						%col.usingStation = -1;
						%col.stationLeaveTime = getSimTime();

						%obj.stationUser = -1;
					}
					else
					{
						if(%col.getObjectMount() != %obj.getId())
							%obj.mountObject(%col, 2);

						if(isObject(%col.getMountedImage(0)))
						{
							%col.unMountImage(0);
							%col.schedule(50, unMountImage, 0);
							commandToClient(%col.Client, 'setScrollMode', -1);
						}

						%col.setDamageLevel(%col.getDamageLevel() - %db.mpbHealthRegen);
						%col.setEnergyLevel(%col.getEnergyLevel() + %db.mpbEnergyRegen);
					}
				}
				else
				{
					%point = %obj.getSlotTransform(2);
					initContainerRadiusSearch(%point, %db.mpbUseRadius, $TypeMasks::PlayerObjectType);
					while(isObject(%col = containerSearchNext()))
					{
						if(%col.getDamagePercent() < 1.0 &&
							!isObject(%col.usingStation) &&
							!%col.isMounted() &&
							vectorDist(%point, %col.getPosition()) <= %db.mpbUseRadius &&
							vectorLen(%col.getVelocity()) <= %db.mpbEscapeVelocity &&
							getSimTime() - %col.stationLeaveTime >= %db.mpbEnterCooldown)
						{
							if(!turretIsFriendly(%obj, %col))
							{
								if(isObject(%col.client) && getSimTime() - %col.stationWarnTime > 3000)
								{
									%col.stationWarnTime = getSimTime();
									%col.client.play2D(Station_DeniedSound);
									%col.client.centerPrint("Your team can not use this inventory station.", 2);
								}
							}
							else
							{
								%obj.stationUser = %col;

								%col.usingStation = %obj;
								%col.stationEnterTime = getSimTime();

								%obj.mountObject(%col, 2);

								if(isObject(%cl = %col.Client) && $LOSetCount !$= "")
									%cl.LOApplyLoadout(true, true);

								%obj.stopAudio(3);
								%obj.playAudio(3, T2JerichoStationUseSound);
							}
							break;
						}
					}
				} // so-called "never nesters" stay mad
			}
		}
		else
		{
			if(!%ctrl)
			{
				if(getSimTime() - %obj.mpbReadyTime >= %db.mpbDeployDelay)
					%obj.MPBDeploy();
			}
			else
				%obj.mpbReadyTime = getSimTime();
		}
	}

	%obj.mdl = %obj.schedule(100, MPBDeployLoop);
}

function WheeledVehicle::MPBDeploy(%obj)
{
	%db = %obj.getDatablock();

	if(%obj.mpbDeployed || isEventPending(%obj.undeployTimer))
		return;

	%obj.mpbDeployed = true;
	%obj.deployTimer = schedule(7000, 0, ""); // for some reason this actually does create a schedule

	%obj.playThread(1, coverOpen);
	%obj.stopAudio(1);
	%obj.playAudio(1, T2JerichoTurretDeploySound);

	%obj.schedule(4000, playThread, 1, stationOpen);
	%obj.schedule(4000, stopAudio, 2);
	%obj.schedule(4000, playAudio, 2, T2JerichoStationDeploySound);

	%turret = new AIPlayer(mpbt)
	{
		datablock = %db.mpbTurretData;
		spawnBrick = %obj.spawnBrick;
		brickGroup = %obj.spawnBrick.getGroup();
		mpb = %obj;
	};

	%obj.mpbTurret = %turret;

	%obj.mountObject(%turret, 1);
	%turret.setTransform("0 0 0 0 0 0 0");

	%turret.turretJam(3000);

	%turret.schedule(0, setNodeColor, "ALL", "1 1 1 1");
	%turret.schedule(0, setScale, "1 1 1");
	%turret.turretHead.schedule(0, setScale, "1 1 1");
	%turret.schedule(0, turretMountImage, "TB: Vulcan");

	if(isFunction(MMGTick))
		%obj.SetMMGIcon("marker", "Deployed MPB", %obj.spawnBrick.getColorID());
}

function WheeledVehicle::MPBUndeploy(%obj)
{
	%db = %obj.getDatablock();

	if(!%obj.mpbDeployed || isEventPending(%obj.deployTimer))
		return;

	%obj.mpbDeployed = false;
	%obj.undeployTimer = schedule(8000, 0, "");

	%obj.playThread(1, stationClose);
	%obj.stopAudio(1);
	%obj.playAudio(1, T2JerichoStationDeploySound);

	%obj.schedule(3000, playThread, 1, coverClose);
	%obj.schedule(3000, stopAudio, 2);
	%obj.schedule(3000, playAudio, 2, T2JerichoTurretUndeploySound);
	
	%turret = %obj.mpbTurret;

	if(!isObject(%turret))
		return;

	%turret.turretJam(8000);
	%turret.schedule(8000, delete);

	if(isObject(%col = %obj.getMountNodeObject(2)))
		%obj.unMountObject(%col);
	
	if(isFunction(MMGTick))
		%obj.SetMMGIcon("none", "", 0);
}

function T2JerichoVehicle::MMGCanScopeTo(%db, %obj, %cl)
{
	return turretIsFriendly(%obj, %cl.getControlObject()) == 1;
}