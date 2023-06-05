registerOutputEvent("Bot", "vpadLinkSpawner", "list Nearest 0 Name 1" TAB "string 200 200", false); // link this pad to a vehicle spawner. [nearest/named brick] [spawner brick name]
registerOutputEvent("Bot", "vpadClearVehicleList", "", false);                                      // clear this pad's spawnable vehicle list.
registerOutputEvent("Bot", "vpadAddVehicleList", "datablock Vehicle" TAB "int 0 99 1", false);      // add a spawnable vehicle to this pad's list. can only have (limit) spawned at a time. [vehicle datablock] [limit]
registerOutputEvent("Bot", "vpadSpawn", "datablock Vehicle", false);                                // spawn a vehicle on a spawner right away. [vehicle datablock]

datablock ParticleData(T2VehiclePadParticle)
{
	dragCoefficient      = 0;
	windCoefficient     = 0;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin = -90;
	spinRandomMax = 90;
	lifetimeMS           = 2000;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/ring";
	useInvAlpha = false;
	
	colors[0]     = "0.0 0.3 0.6 0.2";
	colors[1]     = "0.0 0.1 0.2 0.1";
	colors[2]     = "0.0 0.0 0.0 0";
	sizes[0]      = 1;
	sizes[1]      = 26;
	sizes[2]      = 38;
	times[0]      = 0;
	times[1]      = 0.5;
	times[2]      = 1.0;
};

datablock ParticleEmitterData(T2VehiclePadEmitter)
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
	particles = "T2VehiclePadParticle";
	useEmitterColors = false;
};

datablock ParticleData(T2VehiclePadSpinParticle)
{
	dragCoefficient      = 0;
	windCoefficient     = 0;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin = 0;
	spinRandomMax = 0;
	lifetimeMS           = 1000;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/dot";
	useInvAlpha = false;
	
	colors[0]     = "0.0 0.3 0.7 0.5";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 0.5;
	sizes[1]      = 0.5;
};

datablock ParticleEmitterData(T2VehiclePadSpinEmitter)
{
	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 12.0;
	thetaMin         = 90;
	thetaMax         = 180;
	phiReferenceVel  = 360;
	phiVariance      = 0;
	overrideAdvance = false;
	particles = "T2VehiclePadSpinParticle";
	useEmitterColors = false;
};

datablock ParticleData(T2VehiclePadSmokeParticle)
{
	dragCoefficient      = 2;
	windCoefficient     = 0;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.0;
	constantAcceleration = 0.0;
	spinRandomMin = -300;
	spinRandomMax = 300;
	lifetimeMS           = 3000;
	lifetimeVarianceMS   = 0;
	textureName          = "base/data/particles/cloud";
	useInvAlpha = false;
	
	colors[0]     = "0.0 0.2 0.4 0.2";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 4;
	sizes[1]      = 6;
};

datablock ParticleEmitterData(T2VehiclePadSmokeEmitter)
{
	ejectionPeriodMS = 2;
	periodVarianceMS = 0;
	ejectionVelocity = 24;
	velocityVariance = 8.0;
	ejectionOffset   = 0.0;
	thetaMin         = 90;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "T2VehiclePadSmokeParticle";
	useEmitterColors = false;
};

datablock AudioProfile(Station_VehicleOnSound)
{
	fileName = "./wav/vehiclepad_on.wav";
	description = AudioClosest3D;
	preload = true;
};

datablock AudioProfile(Station_VehicleOffSound)
{
	fileName = "./wav/vehiclepad_off.wav";
	description = AudioClosest3D;
	preload = true;
};

datablock AudioProfile(Station_VehicleUseSound)
{
	fileName = "./wav/vehiclepad_activate.wav";
	description = AudioDefault3D;
	preload = true;
};

datablock StaticShapeData(Station_VehicleSpawner)
{
	defaultScale = "2.0 2.0 2.0";
	shapeFile = "./dts/vehicle_spawn.dts";
};

datablock PlayerData(Station_VehicleSpawnerPlaceholder : PlayerStandardArmor)
{
	className = Armor;
	shapeFile = "base/data/shapes/empty.dts";

	isTurretArmor = true;
	isTurretHead = true;
	TurretPersistent = true;
	TurretLookRange = 0;
	TurretLookMask = 0;
	TurretLookTime = 200;
	TurretThinkTime = 200;

	maxEnergy = 10000;
	rechargeRate = 999;
	energyShield = 1.0;

	mass = 999999;
	maxDamage = 999999;

	boundingBox = vectorScale("1 1 1", 4);
	crouchBoundingBox = vectorScale("1 1 1", 4);

	rideable = true;
	UIName = "T2: Base Vehicle Spawner";
	
	useCustomPainEffects = true;
	painSound = "";
	deathSound = "";
};

function AIPlayer::vpadLinkSpawner(%obj, %mode, %targ)
{
	if(!isObject(%obj.spawnBrick))
		return;
	
	%obj.linkTime = getSimTime();
	%obj.linkMode = %mode;
	%obj.linkTarg = %targ;
	%obj.linkedPad = -1;

	%group = %obj.spawnBrick.getGroup();
	if(%mode == 0) // * nearest
	{
		%nearest = -1;
		%nearDist = 999;
		initContainerRadiusSearch(%obj.getPosition(), 512, $TypeMasks::StaticShapeObjectType);
		while(isObject(%col = containerSearchNext()))
		{
			if(!isObject(%col.sourceObject))
				continue;
			
			if(%col.sourceObject.vPad != %col)
				continue;
			
			if((%dist = vectorDist(%obj.getPosition(), %col.getPosition())) > %nearDist)
				continue;
			
			%nearDist = %dist;
			%nearest = %col;
		}

		if(isObject(%nearest))
			%obj.linkedPad = %nearest.sourceObject;
	}
	else if(%mode == 1) // * named
	{
		%group = %obj.spawnBrick.getGroup();

		%name = "_" @ %targ;
		%cts = %group.NTObjectCount[%name];

		for(%i = 0; %i < %cts; %i++)
		{
			%brk = %group.NTObject[%name, %i];

			if(isObject(%veh = %brk.vehicle) && isObject(%veh.vPad))
				%obj.linkedPad = %veh;
		}
	}
}

function AIPlayer::vpadClearVehicleList(%obj)
{
	%obj.vpadVehicles = 0;
}

function AIPlayer::vpadAddVehicleList(%obj, %data, %max)
{
	%v = %obj.vpadVehicles;
	for(%i = 0; %i < %v; %i++)
	{
		if(%obj.vpadVehicle[%v] == %data)
		{
			%obj.vpadLimit[%v] = %max;
			return;
		}
	}

	%obj.vpadVehicle[%v] = %data;
	%obj.vpadLimit[%v] = %max;
	%obj.vpadVehicles++;
}

function AIPlayer::vpadSpawn(%obj, %data)
{
	if(!isObject(%brk = %obj.spawnBrick) || !isObject(%pad = %obj.vPad))
		return;
	
	%obj.vpadWorking = true;

	%obj.twVehSEffect();
	%obj.schedule(3000, vpadCreate, %data);
}

function AIPlayer::vpadCreate(%obj, %data)
{
	if(!isObject(%brk = %obj.spawnBrick) || !isObject(%pad = %obj.vPad))
		return;

	initContainerRadiusSearch(%pad.getPosition(), 15, $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType);
	while(isObject(%col = containerSearchNext()))
	{
		if(%col.getClassName() $= "Player")
			continue;

		if(%col.spawnBrick != %obj.spawnBrick)
			continue;

		%col.delete();
	}
	
	%veh = -1;
	%class = %data.getClassName();

	// i would use a switch, but they destroy my syntax highlighting for some reason
	     if(%class $= "PlayerData")         %veh = new       AIPlayer(){ dataBlock = %data; };
	else if(%class $= "WheeledVehicleData") %veh = new WheeledVehicle(){ dataBlock = %data; };
	else if(%class $= "FlyingVehicleData")  %veh = new  FlyingVehicle(){ dataBlock = %data; };
	else if(%class $= "HoverVehicleData")   %veh = new   HoverVehicle(){ dataBlock = %data; };

	%veh.spawnBrick = %brk;
	%veh.brickGroup = %brk.getGroup();

	%off = getWord(%veh.getObjectBox(), 5);
	%pos = %pad.getPosition();
	%rot = getWords(%pad.getTransform(), 3, 6);

	if(%class $= "PlayerData")
		%off /= 2;

	%veh.setTransform(vectorAdd(%pos, "0 0 " @%off) SPC %rot);

	%veh.setDamageLevel(0);
	%veh.setEnergyLevel(%data.maxEnergy);

	%v = %brk.vehicle;
	%brk.vehicle = %veh;
	%brk.colorVehicle();
	%brk.vehicle = %v;
	
	%p = new Projectile ("")
	{
		dataBlock = spawnProjectile;
		initialVelocity = "0 0 0";
		initialPosition = %veh.getPosition();
		sourceObject = %veh;
		sourceSlot = 0;
	};

	if(!isObject(%obj.vpadSet))
		%obj.vpadSet = new SimSet();
	
	%obj.vpadSet.add(%veh);

	%veh.vpadDespawnTimer();
	
	%obj.vpadWorking = false;
}

function Vehicle::vpadDespawnTimer(%veh)
{
	cancel(%veh.vpadTimer);

	if(!isObject(%veh))
		return;

	%db = %veh.getDatablock();

	if(%db.vpadNeverTimeOut)
		return;

	if(%veh.vpTime $= "")
		%veh.vpTime = getSimTime();
	
	for(%i = 0; %i < %db.numMountPoints; %i++)
	{
		if(isObject(%veh.getMountNodeObject(%i)))
		{
			%veh.vpTime = getSimTime();
			break;
		}
	}

	if(getSimTime() - %veh.vpTime > 60 * 1000)
	{
		%veh.spawnBrick = "";

		if(%veh.getType() & $TypeMasks::PlayerObjectType)
		{
			%veh.kill();

			if(isObject(%veh))
				%veh.kill();
		}
		else
		{
			%veh.destroy();

			if(isObject(%veh))
				%veh.destroy();
		}

		return;
	}

	%veh.vpadTimer = %veh.schedule(1000, vpadDespawnTimer);
}

function AIPlayer::vpadDespawnTimer(%veh) { Vehicle::vpadDespawnTimer(%veh); }

function AIPlayer::vpadCount(%obj, %data)
{
	if(!isObject(%obj.linkedPad))
		return;

	if(!isObject(%obj.linkedPad.vpadSet))
		%obj.linkedPad.vpadSet = new SimSet();

	%set = %obj.linkedPad.vpadSet;

	%amt = 0;
	%cts = %set.getCount();
	for(%i = 0; %i < %cts; %i++)
	{
		if(%set.getObject(%i).getDataBlock() == %data)
			%amt++;
	}

	return %amt;
}

function AIPlayer::twVehSCreatePad(%obj)
{
	if(!isObject(%obj.vPad))
	{
		%obj.vPad = new StaticShape()
		{
			dataBlock = Station_VehicleSpawner;
			sourceObject = %obj;
		};

		%obj.vPad.setScale(Station_VehicleSpawner.defaultScale);
		%obj.vPad.setTransform(%obj.getTransform());
		%obj.vPad.setNodeColor("ALL", "1 1 1 1");

		MissionCleanup.add(%obj.vPad);

		%obj.setTransform("0 0 -8192");
	}
}

function AIPlayer::twVehSEffect(%obj)
{
	if(isObject(%pad = %obj.vPad))
	{
		%pad.playThread(0, use);
		serverPlay3D(Station_VehicleUseSound, %pad.getPosition());

		%pos = vectorAdd(%pad.getPosition(), "0 0 0.5");

		%node = new ParticleEmitterNode()
		{
			datablock = GenericEmitterNode;
			emitter = T2VehiclePadEmitter;
			scale = "0 0 0";
		};

		%node.setTransform(%pos);
		%node.setColor("1 1 1 1");
		%node.inspectPostApply();

		%node.schedule(3000, delete);
		
		%node = new ParticleEmitterNode()
		{
			datablock = GenericEmitterNode;
			emitter = T2VehiclePadSpinEmitter;
			scale = "0 0 0";
		};

		%node.setTransform(%pos SPC "1 0 0 " @ $pi);
		%node.setColor("1 1 1 1");
		%node.inspectPostApply();

		%node.schedule(3000, delete);
		
		%node = new ParticleEmitterNode()
		{
			datablock = GenericEmitterNode;
			emitter = T2VehiclePadSmokeEmitter;
			scale = "0 0 0";
		};

		%node.setTransform(%pos);
		%node.setColor("1 1 1 1");
		%node.inspectPostApply();

		%node.schedule(3000, delete);
	}
}

function Station_VehicleSpawnerPlaceholder::onAdd(%db, %obj)
{
	Parent::onAdd(%db, %obj);

	%obj.schedule(0, twVehSCreatePad);
}

function Station_VehicleSpawnerPlaceholder::onRemove(%db, %obj)
{
	if(isObject(%obj.vPad))
		%obj.vPad.delete();
		
	if(isObject(%obj.vpadSet))
	{
		%obj.vpadSet.deleteAll();
		%obj.vpadSet.delete();
	}

	Parent::onRemove(%db, %obj);
}

datablock PlayerData(Station_VehiclePad : Turret_TribalBaseStand) // root use
{
	paintable = 1;
	defaultScale = "0.8 0.8 1.0";

	isTurretArmor = true;
	isTurretHead = true;
	TurretHeadData = "";
	TurretPersistent = true;
	TurretLookRange = 0;
	TurretLookMask = 0;
	TurretLookTime = 200;
	TurretThinkTime = 200;

	disabledLevel = 1.0;

	shapeFile = "./dts/vehicle_pad.dts";
	
	rechargeRate = 5 / 31.25;
	maxEnergy = 100;
	energyShield = 1.0;
	energyShape = Turret_EnergyShieldShape;
	energyScale = 1.0;
	energyDelay = 2;
	energySound = Turret_ShieldDamagedSound;
	energyBreakSound = Turret_ShieldDestroyedSound;

	maxDamage = 200;
	
	enterCooldown = 3000;
	minExitTime = 1000;

	useRadius = 3;
	leaveRadius = 2;
	escapeVelocity = 10;

	idleSound = Station_IdleSound;

	boundingBox = vectorScale("3 3 1", 4);
	crouchboundingBox = vectorScale("3 3 1", 4);

	UIName = "T2: Base Vehicle Station";
};

function Station_VehiclePad::turretOnPowerLost(%db, %obj)
{
	%obj.twVehSReset();
	Turret_TribalBaseGenerator::turretOnPowerLost(%db, %obj);
}

function Station_VehiclePad::turretOnPowerRestored(%db, %obj)
{
	Turret_TribalBaseGenerator::turretOnPowerRestored(%db, %obj);
}

function Station_VehiclePad::turretOnDisabled(%db, %obj, %src)
{
	%obj.twVehSReset();
	Turret_TribalBaseGenerator::turretOnDisabled(%db, %obj, %src);
}

function Station_VehiclePad::turretOnDestroyed(%db, %obj, %src)
{
	%obj.twVehSReset();
	Turret_TribalBaseGenerator::turretOnDestroyed(%db, %obj, %src);
}

function Station_VehiclePad::turretOnRecovered(%db, %obj, %src)
{
	Turret_TribalBaseGenerator::turretOnRecovered(%db, %obj, %src);
}

function Station_VehiclePad::turretOnRepaired(%db, %obj, %src)
{
	Turret_TribalBaseGenerator::turretOnRepaired(%db, %obj, %src);
}

function VehiclePadBSM::onUserStart(%obj, %cl)
{
	if(isObject(%src = %obj.sourceObject))
	{
		%obj.entryCount = 0;

		for(%i = 0; %i < %src.vpadVehicles; %i++)
		{
			if(isObject(%data = %src.vpadVehicle[%i]))
			{
				%id = "veh" SPC %data;
				%obj.highlight[%id] = true;

				%lim = "";
				if(%src.vpadLimit[%i] > 0)
				{
					%cts = %src.vpadCount(%data);
					%lim = " [" @ %cts @ " / " @ %src.vpadLimit[%i] @ "]";

					if(%cts >= %src.vpadLimit[%i])
						%obj.highlight[%id] = false;
				}

				%obj.entry[%obj.entryCount] = %data.uiName @ %lim TAB %id;
				%obj.entryCount++;
			}
		}

		%obj.entry[%obj.entryCount] = "Cancel" TAB "closeMenu";
		%obj.entryCount++;
	}

	Parent::onUserStart(%obj, %cl);
}

function VehiclePadBSM::onUserMove(%obj, %cl, %id, %move, %val)
{
	if(%move == $BSM::PLT)
	{
		if(firstWord(%id) $= "veh" && %obj.highlight[%id])
		{
			%obj.sourceObject.linkedPad.vpadSpawn(restWords(%id));

			%move = $BSM::CLR;
		}
	}

	Parent::onUserMove(%obj, %cl, %id, %move, %val);
}

function VehiclePadBSM::onUserEnd(%obj, %cl)
{
	Parent::onUserEnd(%obj, %cl);

	if(%obj.sourceObject.stationInUse)
	{
		%cl.usedVPad = true;
		%obj.sourceObject.twVehSReset();
	}
}

function AIPlayer::twVehSUse(%obj, %pl)
{
	if(!%obj.stationInUse)
	{
		%obj.stationUser = %pl;
		%obj.stationInUse = true;
		%pl.usingStation = %obj;
		%pl.setTransform(vectorAdd(%obj.getPosition(), "0 0 0.1"));
		%pl.setVelocity("0 0 0");
		serverPlay3D(Station_VehicleOnSound, %obj.getPosition());

		if(!isObject(%obj.vehBSM))
		{
			%obj.vehBSM = new ScriptObject()
			{
				superClass = "BSMObject";
				class = "VehiclePadBSM";
				
				title = "<just:left><font:impact:20>\c2Vehicles";
				format = "arial:16" TAB "\c2" TAB "<div:1>\c6" TAB "<div:1>\c2" TAB "\c7";

				disableSelect = true;
				hideOnDeath = true;

				sourceObject = %obj;
			};
		}

		%pl.client.brickShiftMenuEnd();
		%pl.client.brickShiftMenuStart(%obj.vehBSM);
	}
}

function AIPlayer::twVehSReset(%obj)
{
	if(%obj.stationInUse)
	{
		if(isObject(%pl = %obj.stationUser) && %pl.getDamagePercent() < 1.0)
		{
			%pl.usingStation = -1;
			%pl.stationLeaveTime = getSimTime();

			if(%pl.client.usedVPad)
				%pl.client.usedVPad = false;
			else
				%pl.client.brickShiftMenuEnd();
		}
		
		%obj.stationUser = -1;
		%obj.stationInUse = false;
	}
}

function Station_VehiclePad::turretOnIdleTick(%db, %pl)
{
	if(%pl.linkMode !$= "" && getSimTime() - %pl.linkTime > 1000)
		%pl.vpadLinkSpawner(%pl.linkMode, %pl.linkTarg);

	if(!%pl.isDisabled && %pl.isPowered && isObject(%pl.linkedPad))
	{
		if(!%pl.stationInUse && !%pl.linkedPad.vpadWorking)
		{
			initContainerRadiusSearch(%pl.getHackPosition(), %db.useRadius, $TypeMasks::PlayerObjectType);
			while(isObject(%col = containerSearchNext()))
			{
				if(%col == %pl)
					continue;

				if(%col.getDamagePercent() >= 1.0)
					continue;

				if(%col.isMounted())
					continue;

				if(%col.getDataBlock().isTurretArmor)
					continue;

				if(isObject(%col.usingStation) || getSimTime() - %col.stationLeaveTime < %db.enterCooldown)
					continue;

				if(!%pl.getDataBlock().turretCanSee(%pl, %col))
					continue;

				if(vectorLen(%col.getVelocity()) > %db.escapeVelocity)
					continue;

				if(vectorDist(%pl.getPosition(), %col.getPosition()) > %db.useRadius)
					continue;

				if(!turretIsFriendly(%pl, %col))
				{
					if(isObject(%col.client) && getSimTime() - %col.stationWarnTime > 3000)
					{
						%col.stationWarnTime = getSimTime();
						%col.client.play2D(Station_DeniedSound);
						%col.client.centerPrint("Your team can not use this vehicle station.", 2);
					}
					continue;
				}

				%pl.twVehSUse(%col);
				break;
			}
		}
		else if(!isObject(%pl.stationUser) || %pl.stationUser.getDamagePercent() >= 1.0)
			%pl.twVehSReset();
		else
		{
			%user = %pl.stationUser;

			%dist = vectorDist(%pl.getPosition(), %user.getPosition());

			if(%dist >= %db.leaveRadius)
			{
				serverPlay3D(Station_VehicleOffSound, %pl.getPosition());
				%pl.twVehSReset();
				return;
			}
		}
	}
}

function Station_VehiclePad::onAdd(%db, %obj)
{
	if(!isObject(%obj.client))
	{
		%obj.setNodeColor("ALL", "1 1 1 1");		
		%obj.playAudio(3, %db.idleSound);
	}

	Parent::onAdd(%db, %obj);
}

function Station_VehiclePad::onRemove(%db, %obj)
{
	if(isObject(%obj.vehBSM))
		%obj.vehBSM.delete();

	Parent::onRemove(%db, %obj);
}

function Station_VehiclePad::turretCanMount(%db, %pl, %img)
{
	return false;
}

package VPadCrashFix
{
	function FlyingVehicleData::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if(%obj.spawnBrick.vehicle != %obj)
			$ignoreNextRespawn = true;

		Parent::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
		$ignoreNextRespawn = false;
	}
	
	function WheeledVehicleData::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if(%obj.spawnBrick.vehicle != %obj)
			$ignoreNextRespawn = true;

		Parent::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
		$ignoreNextRespawn = false;
	}
	
	function Armor::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if(%obj.spawnBrick.vehicle != %obj)
			$ignoreNextRespawn = true;

		Parent::Damage(%this, %obj, %sourceObject, %position, %damage, %damageType);
		$ignoreNextRespawn = false;
	}

	function fxDTSBrick::spawnVehicle(%brk, %time)
	{
		if($ignoreNextRespawn)
		{
			$ignoreNextRespawn = false;
			return;
		}

		Parent::spawnVehicle(%brk, %time);
	}
};
activatePackage(VPadCrashFix);