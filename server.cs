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
//   o Thundersword Bomber
//   o Jericho MPB
//   x Havoc Transport
//   o Vehicle Pad

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
// exec("./Vehicle_Thundersword.cs");  // todo
// exec("./Vehicle_Jericho.cs");       // todo (requires Weapon_Turrets)