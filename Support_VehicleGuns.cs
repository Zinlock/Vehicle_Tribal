if(!isFunction(FlyingVehicleData, onAdd))
	eval("function FlyingVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(WheeledVehicleData, onAdd))
	eval("function WheeledVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(Armor, onMount))
	eval("function Armor::onMount(%db, %pl, %col, %node) { }");
	
if(!isFunction(Armor, onUnMount))
	eval("function Armor::onUnMount(%db, %pl, %col, %node) { }");

if(!isFunction(Armor, onTrigger))
	eval("function Armor::onTrigger(%db, %pl, %trig, %val) { }");

package T2VehicleGuns
{
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

	function Armor::onMount(%db, %pl, %col, %node)
	{
		Parent::onMount(%db, %pl, %col, %node);

		if(isObject(%pl) && %col.getDataBlock().gunImageSlots > 0)
		{
			%col.lastMountedPlayer = %pl;
			%col.lastMountedClient = %pl.Client;
			%pl.setVehicleGunInventory(%col);
		}
	}
	
	function Armor::onUnMount(%db, %pl, %col, %node)
	{
		Parent::onUnMount(%db, %pl, %col, %node);

		if(isObject(%pl))
			%pl.setVehicleGunInventory(-1);
		
		if(isObject(%col) && %col.getDataBlock().gunImageSlots > 0)
			%col.setImageLoaded(0, 1);
	}

	function Armor::onTrigger(%db, %pl, %trig, %val)
	{
		if(%trig == 0 && isObject(%obj = %pl.getObjectMount()) && %pl.usingVehicleGuns)
		{
			if(%obj.getControllingObject() == %pl)
			{
				%obj.setImageLoaded(0, !%val);
				return;
			}
		}

		Parent::onTrigger(%db, %pl, %trig, %val);
	}

	function serverCmdUseTool(%cl, %idx)
	{
		%pl = %cl.Player;

		if(isObject(%pl) && %pl.isMounted() && %pl.usingVehicleGuns)
		{
			%obj = %pl.getObjectMount();
			%obj.setVehicleGuns(%idx);
		}
		else
			Parent::serverCmdUseTool(%cl, %idx);
	}
};
activatePackage(T2VehicleGuns);

function Vehicle::setVehicleGuns(%obj, %idx)
{
	%odb = %obj.getDataBlock();
	
	if(%idx >= %odb.gunImageSlots)
		return;

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
		%pl.usingVehicleGuns = true;
		
		serverCmdUnUseTool(%cl);
		%pl.unMountImage(0);

		%odb = %obj.getDataBlock();
		%slots = %odb.gunImageSlots;

		if(!%odb.gunHideInventory)
		{
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
		%pl.usingVehicleGuns = false;

		commandToClient(%cl, 'PlayGui_CreateToolHud', %db.maxTools);

		for(%i = 0; %i < %db.maxTools; %i++)
			messageClient(%cl, 'MsgItemPickup', "", %i, %pl.tool[%i]);
	}
}