// Vehicle_Tribal by Oxy (260031)
// Tribes 2 vehicles remade for Blockland

forceRequiredAddon("Vehicle_Jeep");

function tv(%td)
{
	exec("./server.cs");

	if(%td)
		transmitDatablocks();
}

// todo: 
//   x Wildcat Gravcycle
//   o Beowulf Tank
//   o Shrike Scout
//   o Thundersword Bomber
//   o Jericho MPB
//   o Havoc Transport
//   o Vehicle Pad

exec("./common.cs");
exec("./Support_HoverVehicles.cs");
exec("./Support_VehicleEffects.cs");
exec("./Support_VehicleCrash.cs");
exec("./Support_EnergyShield.cs");
// exec("./Support_VehicleGuns.cs");   // todo
exec("./Support_VehiclePrint.cs");

exec("./Vehicle_Wildcat.cs");
// exec("./Vehicle_Beowulf.cs");       // todo
// exec("./Vehicle_Shrike.cs");        // todo
// exec("./Vehicle_Thundersword.cs");  // todo
// exec("./Vehicle_Jericho.cs");       // todo (requires Weapon_Turrets)
// exec("./Vehicle_Havoc.cs");         // todo