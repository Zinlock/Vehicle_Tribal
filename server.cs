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

function GameConnection::longCenterPrint(%cl, %str, %time)
{
	if(strlen(%str) < 255)
		commandToClient(%cl, 'centerPrint', %str, %time);
	else
		commandToClient(%cl, 'centerPrint', '%2%3%4%5%6', %time, getSubStr(%str, 0, 255), getSubStr(%str, 255, 255), getSubStr(%str, 510, 255), getSubStr(%str, 765, 255), getSubStr(%str, 1020, 255));
}

function GameConnection::longBottomPrint(%cl, %str, %time, %hide)
{
	if(strlen(%str) < 255)
		commandToClient(%cl, 'bottomPrint', %str, %time, %hide);
	else
		commandToClient(%cl, 'bottomPrint', '%3%4%5%6%7', %time, %hide, getSubStr(%str, 0, 255), getSubStr(%str, 255, 255), getSubStr(%str, 510, 255), getSubStr(%str, 765, 255), getSubStr(%str, 1020, 255));
}

$trapStaticTypemask = $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType; // workaround for rebuilt

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

if($AddOn__Weapon_Turrets == 1 && isFile("Add-Ons/Weapon_Turrets/server.cs"))
{
	forceRequiredAddon("Weapon_Turrets");

	exec("./Support_BrickShiftMenu.cs");
	exec("./Vehicle_Pad.cs");
	exec("./Vehicle_Jericho.cs");
}