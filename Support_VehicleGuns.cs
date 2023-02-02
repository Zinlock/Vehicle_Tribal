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

	function FlyingVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj) && %db.gunImageSlots > 0)
			%obj.setVehicleGuns(0);
	}

	function WheeledVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj) && %db.gunImageSlots > 0)
			%obj.setVehicleGuns(0);
	}
	
	function Armor::onAdd(%db, %pl)
	{
		Parent::onAdd(%db, %pl);

		if(isObject(%pl) && %db.gunImageSlots > 0 && %pl.getClassName() $= "AIPlayer")
			%pl.setVehicleGuns(0);
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

	function WheeledVehicleData::onCollision (%db, %obj, %col, %vec, %vel)
	{
		if((miniGameCanUse(%col, %obj) == 1 || !isObject(findClientByBL_ID(%obj.spawnBrick.getGroup().bl_id)) || getTrustLevel(%col, %obj) >= $TrustLevel::RideVehicle) && %col.getClassName() $= "Player" && %db.mountToNearest)
		{
			%time = %col.lastMountTime;
			%mount = true;
			%col.lastMountTime = getSimTime();
		}
		
		Parent::onCollision(%db, %obj, %col, %vec, %vel);

		if(%mount)
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
						// if (%canUse)
						// {

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
									// %blockingObj.mountObject (%col, 0);
									// if(%blockingObj.getControllingClient () == 0)
									// {
									// 	%col.setControlObject (%blockingObj);
									// }
									continue;
								}

								if((%nd = vectorDist(%obj.getSlotTransform(%i), %col.getPosition())) < %dist)
								{
									%dist = %nd;
									%nearest = %i;
									%isSlot = true;
								}
								// %obj.mountObject (%col, %i);

								// if (%i == 0)
								// {
								// 	if (%obj.getControllingClient () == 0)
								// 	{
								// 		%col.setControlObject (%obj);
								// 	}
								// }
								// break;
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
						// }
						// else 
						// {
						// 	%ownerName = %obj.spawnBrick.getGroup ().name;
						// 	%msg = %ownerName @ " does not trust you enough to do that";
						// 	if ($lastError == $LastError::Trust)
						// 	{
						// 		%msg = %ownerName @ " does not trust you enough to ride.";
						// 	}
						// 	else if ($lastError == $LastError::MiniGameDifferent)
						// 	{
						// 		if (isObject (%col.client.miniGame))
						// 		{
						// 			%msg = "This vehicle is not part of the mini-game.";
						// 		}
						// 		else 
						// 		{
						// 			%msg = "This vehicle is part of a mini-game.";
						// 		}
						// 	}
						// 	else if ($lastError == $LastError::MiniGameNotYours)
						// 	{
						// 		%msg = "You do not own this vehicle.";
						// 	}
						// 	else if ($lastError == $LastError::NotInMiniGame)
						// 	{
						// 		%msg = "This vehicle is not part of the mini-game.";
						// 	}
						// 	commandToClient (%col.client, 'CenterPrint', %msg, 1);
						// }
					}
				}
			}
		}
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
			}
			else if(isObject(%col.gunTurret[%node]) && %col.gunTurret[%node].getDataBlock().gunImageSlots > 0)
			{
				%col.gunTurret[%node].lastMountedPlayer = %pl;
				%col.gunTurret[%node].lastMountedClient = %pl.Client;
				%pl.setVehicleGunInventory(%col.gunTurret[%node]);
				%pl.setControlObject(%col.gunTurret[%node]);
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

		if(isObject(%pl))
			%pl.setVehicleGunInventory(-1);
		
		if(isObject(%col) && %col.getDataBlock().gunImageSlots > 0 && %node == 0)
		{
			%odb = %col.getDataBlock();

			if(%odb.gunTriggerLoad[%col.currSlot])
				%col.setImageLoaded(%odb.gunTriggerSlot[%col.currSlot], 1);
			else
				%col.setImageTrigger(%odb.gunTriggerSlot[%col.currSlot], 0);
		}
		else if(isObject(%col.gunTurret[%node]))
		{
			%tur = %col.gunTurret[%node];
			%tur.setTransform("0 0 0 0 0 1 0");

			%odb = %tur.getDataBlock();

			if(%odb.gunTriggerLoad[%tur.currSlot])
				%tur.setImageLoaded(%odb.gunTriggerSlot[%tur.currSlot], 1);
			else
				%tur.setImageTrigger(%odb.gunTriggerSlot[%tur.currSlot], 0);
		}
	}

	function Armor::onTrigger(%db, %pl, %trig, %val)
	{
		if(%trig == 0 && isObject(%obj = %pl.usingVehicleGuns))
		{
			%odb = %obj.getDataBlock();

			if(%odb.gunTriggerLoad[%obj.currSlot])
				%obj.setImageLoaded(%odb.gunTriggerSlot[%obj.currSlot], !%val);
			else
				%obj.setImageTrigger(%odb.gunTriggerSlot[%obj.currSlot], %val);

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
activatePackage(T2VehicleGuns);

function VehicleData::onPrevSeat(%db, %obj, %pl) { }
function VehicleData::onNextSeat(%db, %obj, %pl) { }

function VehicleData::onGunMount(%db, %obj, %pl) { }
function VehicleData::onGunUnMount(%db, %obj, %pl) { }
function VehicleData::onGunTrigger(%db, %obj, %pl, %val) { }

function AIPlayer::setVehicleGuns(%obj, %idx) { Vehicle::setVehicleGuns(%obj, %idx); }

function Vehicle::setVehicleGuns(%obj, %idx)
{
	%odb = %obj.getDataBlock();
	
	if(%idx >= %odb.gunImageSlots)
		return;

	if(%idx != %obj.currSlot)
	{
		if(%odb.gunTriggerLoad[%obj.currSlot])
			%obj.setImageLoaded(%odb.gunTriggerSlot[%obj.currSlot], 1);
		else
			%obj.setImageTrigger(%odb.gunTriggerSlot[%obj.currSlot], 0);
	}

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