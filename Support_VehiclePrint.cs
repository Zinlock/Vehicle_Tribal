if(!isFunction(Armor, onMount))
	eval("function Armor::onMount(%db, %pl, %col, %node) { }");

if(!isFunction(FlyingVehicleData, onDriverLeave))
	eval("function FlyingVehicleData::onDriverLeave(%db, %obj, %src) { }");
	
if(!isFunction(WheeledVehicleData, onDriverLeave))
	eval("function WheeledVehicleData::onDriverLeave(%db, %obj, %src) { }");

package T2VehiclePrint
{
	function FlyingVehicleData::onDriverLeave(%db, %obj, %src)
	{
		Parent::onDriverLeave(%db, %obj, %src);
	}
	
	function WheeledVehicleData::onDriverLeave(%db, %obj, %src)
	{
		Parent::onDriverLeave(%db, %obj, %src);
	}

	function Armor::onMount(%db, %pl, %col, %node)
	{
		Parent::onMount(%db, %pl, %col, %node);

		if(isObject(%pl))
			
	}
};
activatePackage(T2VehiclePrint);

