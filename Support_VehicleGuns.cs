if(!isFunction(FlyingVehicleData, onAdd))
	eval("function FlyingVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(WheeledVehicleData, onAdd))
	eval("function WheeledVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(Armor, onAdd))
	eval("function Armor::onAdd(%db, %pl) { }");

if(!isFunction(Armor, onMount))
	eval("function Armor::onMount(%db, %pl, %col, %node) { }");
	
if(!isFunction(Armor, onUnMount))
	eval("function Armor::onUnMount(%db, %pl, %col, %node) { }");

if(!isFunction(Armor, onTrigger))
	eval("function Armor::onTrigger(%db, %pl, %trig, %val) { }");

// todo: add support for aiplayer based guns
// todo: make aiplayer guns redirect damage to the vehicle
// todo: make it so vehicle mountpoints can control ai guns

package T2VehicleGuns
{
	function createUINameTable()
	{
		Parent::createUINameTable();

		%cts = DataBlockGroup.getCount();
		for(%i = 0; %i < %cts; %i++)
		{
			%db = DataBlockGroup.getObject(%i);

			if(%db.getClassName() $= "ItemData" && %db.hideFromSpawnMenu)
				$uiNameTable_Items[%db.uiName] = "";
		}
	}

	function Armor::Damage(%db, %pl, %src, %pos, %dmg, %type)
	{
		if(%pl.isVehicleTurret)
		{
			%pl.sourceObject.damage(%src, %pos, %dmg, %type);
			return;
		}

		return Parent::Damage(%db, %pl, %src, %pos, %dmg, %type);
	}

	function ShapeBase::setNodeColor(%obj, %node, %color)
	{
		Parent::setNodeColor(%obj, %node, %color);

		%obj.lastNodeColor[%node] = %color;

		%odb = %obj.getDataBlock();
		if(%odb.numTurretHeads > 0)
		{
			for(%i = 0; %i < %odb.numTurretHeads; %i++)
			{
				if(isObject(%obj.turret[%i]))
					%obj.turret[%i].setNodeColor("ALL", %color);
			}
		}
	}

	function T2VGOnAdd(%db, %obj)
	{
		if(isObject(%obj))
		{
			if(%db.gunImageSlots > 0)
				%obj.setVehicleGuns(0);
			
			if(%db.numTurretHeads > 0)
				%obj.schedule(0, setVehicleTurrets);
		}
	}
	
	function FlyingVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		T2VGOnAdd(%db, %obj);
	}

	function WheeledVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		T2VGOnAdd(%db, %obj);
	}
	
	function Armor::onAdd(%db, %pl)
	{
		Parent::onAdd(%db, %pl);

		T2VGOnAdd(%db, %pl);
	}

	function serverCmdPrevSeat(%cl)
	{
		if(!isObject(%pl = %cl.player))
			return Parent::serverCmdPrevSeat(%cl);
		
		if(isObject(%obj = %pl.getObjectMount()) && (%odb = %obj.getDataBlock()).overridePrevSeat)
		{
			%odb.onPrevSeat(%obj, %pl);
			return;
		}

		Parent::serverCmdPrevSeat(%cl);
	}

	function serverCmdNextSeat(%cl)
	{
		if(!isObject(%pl = %cl.player))
			return Parent::serverCmdNextSeat(%cl);
		
		if(isObject(%obj = %pl.getObjectMount()) && (%odb = %obj.getDataBlock()).overrideNextSeat)
		{
			%odb.onNextSeat(%obj, %pl);
			return;
		}

		Parent::serverCmdNextSeat(%cl);
	}

	function T2VGMountCheck(%db, %obj, %col)
	{
		if((miniGameCanUse(%col, %obj) == 1 || !isObject(findClientByBL_ID(%obj.spawnBrick.getGroup().bl_id)) || getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle) && %col.getClassName() $= "Player" && %db.mountToNearest)
		{
			%time = %col.lastMountTime;
			%mount = true;
			%col.lastMountTime = getSimTime();
		}
		else
			%time = -1;

		return %time;
	}

	function T2VGMount(%db, %obj, %col, %time)
	{
		%col.lastMountTime = %time;

		if(%col.getDataBlock().canRide && %db.rideAble && %db.nummountpoints > 0)
		{
			if(getSimTime() - %col.lastMountTime > $Game::MinMountTime)
			{
				%colZpos = getWord (%col.getPosition (), 2);
				%objZpos = getWord (%obj.getPosition (), 2);
				if (%colZpos > %objZpos + 0.2)
				{
					%dist = 999;
					%nearest = -1;
					%isSlot = true;
					for (%i = 0; %i < %db.nummountpoints; %i += 1)
					{
						%blockingObj = %obj.getMountNodeObject (%i);

						if (isObject (%blockingObj))
						{
							if (!%blockingObj.getDataBlock ().rideAble || isObject(%blockingObj.getMountedObject(0)))
								continue;
							
							if((%nd = vectorDist(%blockingObj.getPosition(), %col.getPosition())) < %dist)
							{
								%dist = %nd;
								%nearest = %blockingObj;
								%isSlot = false;
							}
							continue;
						}

						if((%nd = vectorDist(%obj.getSlotTransform(%i), %col.getPosition())) < %dist)
						{
							%dist = %nd;
							%nearest = %i;
							%isSlot = true;
						}
					}

					if(!%isSlot && isObject(%nearest))
					{
						%nearest.mountObject(%col, 0);
						
						if(%nearest.getControllingClient() == 0)
							%col.setControlObject(%nearest);
					}
					else if(%isSlot && %nearest >= 0)
					{
						%obj.mountObject(%col, %nearest);

						if(%obj.getControllingClient() == 0 && %nearest == 0)
							%col.setControlObject (%obj);
					}
				}
			}
		}
	}

	function FlyingVehicleData::onCollision (%db, %obj, %col, %vec, %vel)
	{
		%time = T2VGMountCheck(%db, %obj, %col);
		
		Parent::onCollision(%db, %obj, %col, %vec, %vel);

		if(%time >= 0)
			T2VGMount(%db, %obj, %col, %time);
	}

	function WheeledVehicleData::onCollision (%db, %obj, %col, %vec, %vel)
	{
		%time = T2VGMountCheck(%db, %obj, %col);
		
		Parent::onCollision(%db, %obj, %col, %vec, %vel);

		if(%time >= 0)
			T2VGMount(%db, %obj, %col, %time);
	}

	function Armor::onMount(%db, %pl, %col, %node)
	{
		Parent::onMount(%db, %pl, %col, %node);

		if(isObject(%pl))
		{
			%pl.lastMountNode = %node;

			if(%col.getDataBlock().gunImageSlots > 0 && %node == 0)
			{
				%col.lastMountedPlayer = %pl;
				%col.lastMountedClient = %pl.Client;
				%pl.setVehicleGunInventory(%col);
				
				%col.getDataBlock().onGunMount(%col, %pl);
			}
			else if(isObject(%tur = %col.gunTurret[%node]) && %tur.getDataBlock().gunImageSlots > 0)
			{
				%tur.lastMountedPlayer = %pl;
				%tur.lastMountedClient = %pl.Client;
				%pl.setVehicleGunInventory(%tur);
				%pl.setControlObject(%tur);
				
				%tur.getDataBlock().onGunMount(%tur, %pl);
			}
			else if(%col.getDataBlock().blockImages[%node])
			{
				serverCmdUnUseTool(%pl.client);
				%pl.unmountImage(0);
				commandToClient(%pl.client, 'SetScrollMode', -1);
			}
		}
	}

	function Armor::onUnMount(%db, %pl, %col, %node)
	{
		Parent::onUnMount(%db, %pl, %col, %node);

		if(%pl.isVehicleTurret)
		{
			%pl.schedule(0, delete);
			return;
		}

		if(isObject(%pl))
			%pl.setVehicleGunInventory(-1);
		
		if(isObject(%col) && %col.getDataBlock().gunImageSlots > 0 && %node == 0)
		{
			%odb = %col.getDataBlock();

			%odb.onGunUnMount(%col, %pl);
		}
		else if(isObject(%col.gunTurret[%node]))
		{
			%tur = %col.gunTurret[%node];
			%tur.setTransform("0 0 0 0 0 1 0");

			%odb = %tur.getDataBlock();

			%odb.onGunUnMount(%tur, %pl);
		}
	}

	function Armor::onTrigger(%db, %pl, %trig, %val)
	{
		if(%trig == 0 && isObject(%obj = %pl.usingVehicleGuns))
		{
			%odb = %obj.getDataBlock();
			%odb.onGunTrigger(%obj, %pl, %val);

			return;
		}

		Parent::onTrigger(%db, %pl, %trig, %val);
	}

	function serverCmdUnUseTool(%cl)
	{
		%pl = %cl.Player;

		if(isObject(%pl) && isObject(%pl.usingVehicleGuns))
		{
			commandToClient(%cl, 'SetActiveTool', %pl.usingVehicleGuns.currSlot);
			%pl.schedule(0, unMountImage, 0);
			%pl.schedule(0, playThread, 1, root);
		}
		else
			Parent::serverCmdUnUseTool(%cl);
	}

	function serverCmdUseTool(%cl, %idx)
	{
		%pl = %cl.Player;

		if(isObject(%pl))
		{
			if(isObject(%pl.usingVehicleGuns))
			{
				%obj = %pl.usingVehicleGuns;
				%obj.setVehicleGuns(%idx);
				return;
			}
			else if(isObject(%obj = %pl.getObjectMount()) && %obj.getDataBlock().blockImages[%pl.lastMountNode])
				return;
		}
		
		Parent::serverCmdUseTool(%cl, %idx);
	}
};
schedule(0, 0, activatePackage, T2VehicleGuns);

function VehicleData::onPrevSeat(%db, %obj, %pl) { }
function Armor::onPrevSeat(%db, %obj, %pl) { }
function VehicleData::onNextSeat(%db, %obj, %pl) { }
function Armor::onNextSeat(%db, %obj, %pl) { }

function VehicleData::onGunMount(%db, %obj, %pl) { }
function Armor::onGunMount(%db, %obj, %pl) { }
function VehicleData::onGunUnMount(%db, %obj, %pl) { }
function Armor::onGunUnMount(%db, %obj, %pl) { }
function VehicleData::onGunTrigger(%db, %obj, %pl, %val) { }
function Armor::onGunTrigger(%db, %obj, %pl, %val) { }
function VehicleData::onGunEquip(%db, %obj, %pl, %old, %new) { }
function Armor::onGunEquip(%db, %obj, %pl, %old, %new) { }

function AIPlayer::setVehicleTurrets(%obj) { Vehicle::setVehicleTurrets(%obj); }

function Vehicle::setVehicleTurrets(%obj)
{
	%odb = %obj.getDataBlock();

	if(%odb.numTurretHeads <= 0)
		return;

	for(%i = 0; %i < %odb.numTurretHeads; %i++)
	{
		%t = new AIPlayer()
		{
			dataBlock = %odb.turretHeadData[%i];

			isVehicleTurret = true;
			sourceObject = %obj;
			spawnBrick = %obj.spawnBrick;
			brickGroup = %obj.brickGroup;
		};

		if(isObject(%t))
		{
			MissionCleanup.add(%t);

			%obj.mountObject(%t, %odb.turretMountPoint[%i]);
			%obj.gunTurret[%odb.turretControlPoint[%i]] = %t;
			%obj.turret[%i] = %t;

			if(%obj.lastNodeColor["ALL"] !$= "")
				%t.setNodeColor("ALL", %obj.lastNodeColor["ALL"]);
		}
	}
}

function AIPlayer::setVehicleGuns(%obj, %idx) { Vehicle::setVehicleGuns(%obj, %idx); }

function Vehicle::setVehicleGuns(%obj, %idx)
{
	%odb = %obj.getDataBlock();
	
	if(%idx >= %odb.gunImageSlots)
		return;

	if(%idx != %obj.currSlot && %obj.currSlot !$= "")
		%odb.onGunEquip(%obj, %obj.lastMountedPlayer, %obj.currSlot, %idx);

	for(%i = 0; %i < 4; %i++)
	{
		if(isObject(%img = %odb.gunImage[%idx, %i]))
		{
			if(%obj.getMountedImage(%i) != %img.getId())
				%obj.mountImage(%img, %i);
		}
		else if(%obj.currSlot !$= "" && isObject(%odb.gunImage[%obj.currSlot, %i]))
			%obj.unMountImage(%i);
	}

	%obj.currSlot = %idx;
}

function AIPlayer::setVehicleGunInventory(%pl, %val) { }

function Player::setVehicleGunInventory(%pl, %obj)
{
	%cl = %pl.Client;
	%db = %pl.getDataBlock();

	if(isObject(%obj))
	{
		%pl.usingVehicleGuns = %obj;
		
		serverCmdUnUseTool(%cl);
		%pl.unMountImage(0);

		%odb = %obj.getDataBlock();
		%slots = %odb.gunImageSlots;

		if(!%odb.gunHideInventory)
		{
			commandToClient(%cl, 'SetPaintingDisabled', 1);
			commandToClient(%cl, 'SetBuildingDisabled', 1);

			commandToClient(%cl, 'PlayGui_CreateToolHud', %slots);

			for(%i = 0; %i < %slots; %i++)
			{
				if(isObject(%itm = %odb.gunItem[%i]))
					messageClient(%cl, 'MsgItemPickup', "", %i, %itm.getId());
				else
					messageClient(%cl, 'MsgItemPickup', "", %i, -1);
			}
			
			commandToClient(%cl, 'SetScrollMode', 2);
			commandToClient(%cl, 'SetActiveTool', %obj.currSlot);
		}
		else
			commandToClient(%cl, 'PlayGui_CreateToolHud', 0);
	}
	else if(%pl.usingVehicleGuns)
	{
		%pl.usingVehicleGuns = "";
		
		commandToClient(%cl, 'SetPaintingDisabled', (isObject(%mg = %cl.minigame) ? !%mg.enablePainting : false));
		commandToClient(%cl, 'SetBuildingDisabled', (isObject(%mg = %cl.minigame) ? !%mg.enableBuilding : false));

		commandToClient(%cl, 'PlayGui_CreateToolHud', %db.maxTools);

		for(%i = 0; %i < %db.maxTools; %i++)
			messageClient(%cl, 'MsgItemPickup', "", %i, %pl.tool[%i]);
	}
}