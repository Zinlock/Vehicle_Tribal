// Support_brickShiftMenu.cs by Oxy (260031)

function GameConnection::longCenterPrint(%cl, %str, %time)
{
	if(strlen(%str) < 240)
		commandToClient(%cl, 'centerPrint', %str, %time);
	else
		commandToClient(%cl, 'centerPrint', '%2%3%4%5%6', %time, getSubStr(%str, 0, 240), getSubStr(%str, 240, 240), getSubStr(%str, 480, 240), getSubStr(%str, 720, 240), getSubStr(%str, 960, 240));
}

$BSM::ROT = 0; // rotate id
$BSM::MOV = 1; // move id
$BSM::PLT = 2; // plant id
$BSM::CLR = 3; // clear id

function bsm()
{
	exec("./support_brickshiftmenu.cs");
}

function idxLoop(%id, %min, %max)
{
	if(%id < %min)
		%id = %max - 1;
	else if(%id >= %max)
		%id = %min;

	return %id;
}

function cleanup(%obj)
{
	while(isObject(%obj))
		%obj.delete();
}

package brickShiftMenuSupport
{
	function serverCmdPlantBrick(%cl, %idx)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuPlant();
			return;
		}
		
		Parent::serverCmdPlantBrick(%cl, %idx);
	}

	function serverCmdCancelBrick(%cl, %idx)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuClear();
			return;
		}
		
		Parent::serverCmdCancelBrick(%cl, %idx);
	}

	function serverCmdShiftBrick(%cl, %x, %y, %z)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuMove(vectorNormalize(%y * -1 SPC %x SPC %z));
			return;
		}

		Parent::serverCmdShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdSuperShiftBrick(%cl, %x, %y, %z)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuMove(vectorNormalize(%y * -1 SPC %x SPC %z));
			return;
		}

		Parent::serverCmdSuperShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdRotateBrick(%cl, %dir)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuRotate(mClamp(%dir, -1, 1));
			return;
		}

		Parent::serverCmdRotateBrick(%cl, %dir);
	}
};
activatePackage(brickShiftMenuSupport);

function GameConnection::brickShiftMenuEnd(%cl)
{
	if(isObject(%bsm = %cl.brickShiftMenu) && %bsm.superClass $= "BSMObject")
		%bsm.onUserEnd(%cl);

	cancel(%cl.brickShiftLoop);
	%cl.brickShiftMenu = "";
	%cl.brickShiftLoop = "";
	%cl.centerprint("", 0);
}

function GameConnection::brickShiftMenuStart(%cl, %id)
{
	%cl.brickShiftMenu = %id;
	%cl.brickShiftLoop = %cl.schedule(0, brickShiftMenuLoop, %id);

	if(%id.superClass $= "BSMObject")
		%id.onUserStart(%cl);
}

function GameConnection::brickShiftMenuLoop(%cl)
{
	cancel(%cl.brickShiftLoop);

	if(%cl.brickShiftMenu $= "")
	{
		%cl.brickShiftMenuEnd();
		return;
	}

	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserLoop(%cl);
	}
	
	%cl.brickShiftLoop = %cl.schedule(100, brickShiftMenuLoop);
}

function GameConnection::brickShiftMenuClear(%cl)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::CLR);
		// talk("clr");
	}
}

function GameConnection::brickShiftMenuPlant(%cl)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::PLT);
		// talk("plant");
	}
}

function GameConnection::brickShiftMenuMove(%cl, %dir)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::MOV, %dir);
		// talk("shift" SPC %dir);
	}
}

function GameConnection::brickShiftMenuRotate(%cl, %dir)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::ROT, %dir);
		// talk("rotate" SPC %dir);
	}
}

function BSMObject::getSource(%obj)
{
	%src = %obj;

	for(%i = 0; %i < 256; %i++)
	{
		if(!isObject(%src.parent))
			return %src;
		
		%src = %src.parent;
	}
}

function BSMObject::printToClient(%obj, %cl)
{
	if(getFieldCount(%obj.title) != 1 || getFieldCount(%obj.format) != 5)
	{
		warn("BSMObject::printToClient() - invalid BSM title or format field count");
		return;
	}

	%str = %obj.title @ "<font:" @ getField(%obj.format, 0) @ ">";

	%idx = %cl.selid;
	%act = %cl.actid;

	if(%act $= "")
		%act = -1;

	%cut = 3;

	for(%i = 0; %i < %obj.entryCount; %i++)
	{
		%entry = trim(getField(%obj.entry[%i], 0));
		if(%entry $= "")
		{
			warn("BSMObject::printToClient() - empty entry " @ %i);
			continue;
		}

		%entid = trim(getField(%obj.entry[%i], 1));

		// man I'm really good at making unreadable code
		if(%cl.selid < %obj.entryCount - %cut)
		{
			if(mAbs(%cl.selid - %i) > %cut && %cl.selid > %cut || %i > %cut * 2 && %cl.selid <= %cut)
				continue;
		}
		else
		{
			if(%i < %obj.entryCount - %cut * 2 - 1)
				continue;
		}

		%form = getField(%obj.format, (%act == %i || %obj.highlight[%entid] ? (%idx == %i ? 3 : 1) : (%idx == %i ? 2 : 4)));
		%str = %str @ "<br>" @ %form;
		%str = %str @ %entry;
	}

	%cl.longCenterPrint(%str, 1);
}

function BSMObject::onUserMove(%obj, %cl, %id, %move, %val)
{
	switch(%move)
	{
		case ($BSM::PLT):
			if(isObject(%next = %obj.submenu[%id]))
			{
				if(!%next.hideOnDeath || isObject(%cl.Player))
				{
					%obj.onUserEnd(%cl);
					%cl.brickShiftMenu = %next;
					%next.onUserStart(%cl);
				}
			}
			else
			{
				if(%id $= "closeMenu")
				{
					%cl.brickShiftMenuEnd();
				}
				else if(!%obj.blockSelect[%id] && !%obj.disableSelect)
				{
					if(%cl.actid != %cl.selid)
					{
						%pre = %cl.actid;
						%cl.actid = %cl.selid;
						%obj.onUserSelect(%cl, %pre, %cl.actid);
					}
					else
					{
						%pre = %cl.actid;
						%cl.actid = -1;
						%obj.onUserSelect(%cl, %pre, %cl.actid);
					}
				}
			}

		case ($BSM::MOV):
			%dir = getWord(%val, 1);

			%id = %cl.selid;
			%id -= %dir;
			%id = idxLoop(%id, 0, %obj.entryCount);

			%cl.selid = %id;

		case ($BSM::ROT):

		case ($BSM::CLR):
			%src = %obj.getSource();

			if(%src.getID() == %obj.getID())
			{
				%cl.brickShiftMenuEnd();
			}
			else
			{
				%obj.onUserEnd(%cl);
				%cl.brickShiftMenu = %obj.parent;
				%obj.parent.onUserStart(%cl);
			}
	}

	if(isObject(%next = %cl.brickShiftMenu))
		%next.onUserLoop(%cl);
}

function BSMObject::onUserSelect(%obj, %cl, %pre, %post)
{
	if(%pre > 0)
	{
		%cl.actid = -1;
	}
}

function BSMObject::onUserStart(%obj, %cl)
{
	%cl.selid = 0;
	%cl.actid = -1;
	%obj.onUserLoop(%cl);
}

function BSMObject::onUserEnd(%obj, %cl)
{
	%cl.selid = "";
	%cl.actid = "";
}

function BSMObject::onUserLoop(%obj, %cl)
{
	if(%obj.hideOnDeath && !isObject(%cl.Player))
	{
		%src = %obj.getSource();

		if(%src.getID() == %obj.getID() || %obj.parent.hideOnDeath)
		{
			%obj.onUserEnd(%cl);
			%cl.brickShiftMenuEnd();
		}
		else
		{
			%cl.brickShiftMenu = %obj.parent;
			%obj.parent.onUserStart(%cl);
		}
		return;
	}
	
	%obj.printToClient(%cl);
}

cleanup(bsmTestMenu);
cleanup(bsmTestSubmenu);

// example bsm objects:

new ScriptObject(bsmTestMenu)
{
	superClass = "BSMObject";

	title = "<font:arial:16>\c2Test Menu";
	format = "tahoma:14" TAB "\c2" TAB "<div:1>\c6" TAB "<div:1>\c2" TAB "\c7"; // font:size TAB selected TAB hovered TAB hovered selected TAB inactive

	entry[0] = "Go to sub menu" TAB "test1"; // title TAB id
	entry[1] = "Go to the menu with too much stuff" TAB "test2";
	entry[2] = "This is the third option" TAB "test3";
	entry[3] = "Close this menu" TAB "closeMenu";

	entryCount = 4;

	submenu["test1"] = bsmTestSubmenu;
	submenu["test2"] = bsmTestBigSubmenu;

	disableSelect = true;
};

new ScriptObject(bsmTestSubmenu)
{
	superClass = "BSMObject";

	parent = bsmTestMenu;

	title = "<font:arial:16>\c2Test Sub Menu";
	format = bsmTestMenu.format;

	entry[0] = "Back to test menu" TAB "test1";
	entry[1] = "Bottom print \"gamer\"" TAB "test2";

	entryCount = 2;

	submenu["test1"] = bsmTestMenu;

	hideOnDeath = true;
};

new ScriptObject(bsmTestBigSubmenu)
{
	superClass = "BSMObject";

	parent = bsmTestMenu;

	title = "<font:arial:16>\c2Big Test Sub Menu";
	format = bsmTestMenu.format;

	entry[0]  = "Back to test menu" TAB "test1";
	entry[1]  = "Pretty long thing 1"  TAB "test2";
	entry[2]  = "Pretty long thing 2"  TAB "test3";
	entry[3]  = "Pretty long thing 3"  TAB "test4";
	entry[4]  = "Pretty long thing 4"  TAB "test5";
	entry[5]  = "Pretty long thing 5"  TAB "test6";
	entry[6]  = "Pretty long thing 6"  TAB "test7";
	entry[7]  = "Pretty long thing 7"  TAB "test8";
	entry[8]  = "Pretty long thing 8"  TAB "test9";
	entry[9]  = "Pretty long thing 9"  TAB "test10";
	entry[10] = "Pretty long thing 10" TAB "test11";
	entry[11] = "Pretty long thing 11" TAB "test12";
	entry[12] = "Plastic Tubes: 235 [+6]" TAB "test13";
	entry[13] = "Gold Ingots: 943 [+30]" TAB "test14";
	entry[14] = "Metal Scrap: 943 [+3]" TAB "test15";

	entryCount = 15;

	submenu["test1"] = bsmTestMenu;

	blockSelect["test4"] = true;
};

function bsmTestSubmenu::onUserMove(%obj, %cl, %id, %move, %val)
{
	if(%id $= "test2" && %move == $BSM::PLT)
		%cl.bottomprint("gamer", 1, 1);
	else
		Parent::onUserMove(%obj, %cl, %id, %move, %val);
}