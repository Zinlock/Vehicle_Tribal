// todo: also have a vehicle station which automatically links to the nearest pad
// todo: make the vehicle spawner pad a static shape so it can be stood on properly

datablock PlayerData(Station_VehiclePad : Turret_TribalBaseStand) // root use
{
	paintable = 1;
	defaultScale = "2.0 2.0 2.0";

	isTurretArmor = true;
	isTurretHead = true;
	TurretPersistent = true;
	TurretLookRange = 0;
	TurretLookMask = 0;
	TurretLookTime = 200;
	TurretThinkTime = 200;

	disabledLevel = 1.0;

	shapeFile = "./dts/vehiclepad.dts";

	maxDamage = 999999;

	energyShield = 0;

	idleSound = Station_IdleSound;

	boundingBox = vectorScale("8 8 1", 4);
	crouchboundingBox = vectorScale("8 8 1", 4);

	UIName = "T2: Base Vehicle Pad";
};

function Station_VehiclePad::turretOnPowerLost(%db, %obj)
{
	Turret_TribalBaseGenerator::turretOnPowerLost(%db, %obj);
}

function Station_VehiclePad::turretOnPowerRestored(%db, %obj)
{
	Turret_TribalBaseGenerator::turretOnPowerRestored(%db, %obj);
}

function Station_VehiclePad::turretOnDisabled(%db, %obj, %src)
{
	Turret_TribalBaseGenerator::turretOnDisabled(%db, %obj, %src);
}

function Station_VehiclePad::turretOnDestroyed(%db, %obj, %src)
{
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

function Station_VehiclePad::onAdd(%db, %obj)
{
	if(!isObject(%obj.client))
	{
		%obj.setNodeColor("ALL", "1 1 1 1");		
		%obj.playAudio(3, %db.idleSound);
	}

	Parent::onAdd(%db, %obj);
}

function Station_VehiclePad::turretCanMount(%db, %pl, %img)
{
	return false;
}