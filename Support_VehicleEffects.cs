if(!isFunction(FlyingVehicleData, onAdd))
	eval("function FlyingVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(WheeledVehicleData, onAdd))
	eval("function WheeledVehicleData::onAdd(%db, %obj) { }");

package T2VehicleEffects
{
	function FlyingVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj) && %db.useEngineSounds)
		{
			%obj.engineLevel = -1;
			%obj.engineLoop();
		}

		if(%db.defaultColor !$= "")
		{
			%obj.color = setWord(%db.defaultColor, 3, "1");
			
			if(!isObject(%obj.spawnBrick))
				%obj.setNodeColor("ALL", %obj.color);
		}
	}
	
	function WheeledVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj) && %db.useEngineSounds)
		{
			%obj.engineLevel = -1;
			%obj.engineLoop();
		}
		
		if(%db.defaultColor !$= "")
		{
			%obj.color = setWord(%db.defaultColor, 3, "1");

			if(!isObject(%obj.spawnBrick))
				%obj.setNodeColor("ALL", %obj.color);
		}
	}

	function FxDtsBrick::unColorVehicle(%brk)
	{
		Parent::unColorVehicle(%brk);

		if(isObject(%veh = %brk.vehicle) && (%db = %veh.getDataBlock()).defaultColor !$= "")
		{
			%veh.color = setWord(%db.defaultColor, 3, "1");
			%veh.setNodeColor("ALL", %veh.color);
		}
	}
};
schedule(0, 0, activatePackage, T2VehicleEffects);

function FlyingVehicleData::onEngineLowSpeed(%db, %obj) { }
function FlyingVehicleData::onEngineMedSpeed(%db, %obj) { }
function FlyingVehicleData::onEngineHighSpeed(%db, %obj) { }

function WheeledVehicleData::onEngineLowSpeed(%db, %obj) { }
function WheeledVehicleData::onEngineMedSpeed(%db, %obj) { }
function WheeledVehicleData::onEngineHighSpeed(%db, %obj) { }

function FlyingVehicle::engineLoop(%obj) { WheeledVehicle::engineLoop(%obj); }

function WheeledVehicle::engineLoop(%obj)
{
	cancel(%obj.engine);

	%db = %obj.getDataBlock();

	if(!%db.useEngineSounds)
		return;

	%vel = vectorLen(%obj.getVelocity());

	if(%vel < %db.engineMoveSpeed)
	{
		if(%obj.engineLevel != 0)
		{
			%obj.engineLevel = 0;
			%obj.stopAudio(%db.engineSlot);
			
			if(isObject(%db.engineIdleSound))
				%obj.playAudio(%db.engineSlot, %db.engineIdleSound);
			
			%db.onEngineLowSpeed(%obj);
		}
	}
	else if(%vel < %db.engineBoostSpeed)
	{
		if(%obj.engineLevel != 1)
		{
			%obj.engineLevel = 1;
			%obj.stopAudio(%db.engineSlot);
			
			if(isObject(%db.engineMoveSound))
				%obj.playAudio(%db.engineSlot, %db.engineMoveSound);
			
			%db.onEngineMedSpeed(%obj);
		}
	}
	else
	{
		if(%obj.engineLevel != 2)
		{
			%obj.engineLevel = 2;
			%obj.stopAudio(%db.engineSlot);
			
			if(isObject(%db.engineBoostSound))
				%obj.playAudio(%db.engineSlot, %db.engineBoostSound);

			%db.onEngineHighSpeed(%obj);
		}
	}

	%obj.engine = %obj.schedule(250, engineLoop);
}
