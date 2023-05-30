// Vehicle_Tribal by Oxy (260031)
// Tribes 2 vehicles remade for Blockland

forceRequiredAddon("Vehicle_Jeep");
forceRequiredAddon("Vehicle_Tank");
forceRequiredAddon("Weapon_Gun");
forceRequiredAddon("Weapon_Rocket_Launcher");

function tv(%td)
{
	exec("./server.cs");

	if(%td)
		transmitDatablocks();
}

// todo: 
//   x Wildcat Gravcycle
//   x Beowulf Tank
//   x Shrike Scout
//   x Thundersword Bomber
//   o Jericho MPB
//   x Havoc Transport
//   o Vehicle Pad

function GameConnection::longCenterPrint(%cl, %str, %time)
{
	if(strlen(%str) < 240)
		commandToClient(%cl, 'centerPrint', %str, %time);
	else
		commandToClient(%cl, 'centerPrint', '%3%4%5%6%7', %time, getSubStr(%str, 0, 240), getSubStr(%str, 240, 240), getSubStr(%str, 480, 240), getSubStr(%str, 720, 240), getSubStr(%str, 960, 240));
}

function GameConnection::longBottomPrint(%cl, %str, %time, %hide)
{
	if(strlen(%str) < 240)
		commandToClient(%cl, 'bottomPrint', %str, %time, %hide);
	else
		commandToClient(%cl, 'bottomPrint', '%3%4%5%6%7', %time, %hide, getSubStr(%str, 0, 240), getSubStr(%str, 240, 240), getSubStr(%str, 480, 240), getSubStr(%str, 720, 240), getSubStr(%str, 960, 240));
}

exec("./ballistics.cs");
exec("./common.cs");
exec("./Support_HoverVehicles.cs");
exec("./Support_VehicleEffects.cs");
exec("./Support_VehicleCrash.cs");
exec("./Support_EnergyShield.cs");
exec("./Support_VehicleGuns.cs");
exec("./Support_VehiclePrint.cs");
exec("./Support_ArmingDelayFix.cs");

exec("./Vehicle_Wildcat.cs");
exec("./Vehicle_Wildcat_Grav.cs");
exec("./Vehicle_Shrike.cs");
exec("./Vehicle_Beowulf.cs");
exec("./Vehicle_Havoc.cs");
exec("./Vehicle_Thundersword.cs");

if($AddOn__Weapon_Turrets && isFile("Add-Ons/Weapon_Turrets/server.cs"))
{
	forceRequiredAddon("Weapon_Turrets");

	exec("./Vehicle_Pad.cs");
	// exec("./Vehicle_Jericho.cs");       // todo 
}