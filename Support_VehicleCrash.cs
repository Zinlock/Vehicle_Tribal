if(!isFunction(FlyingVehicleData, onImpact))
	eval("function FlyingVehicleData::onImpact(%db, %obj) { }");
	
if(!isFunction(WheeledVehicleData, onImpact))
	eval("function WheeledVehicleData::onImpact(%db, %obj) { }");

package T2VehicleImpacts
{
	function FlyingVehicleData::onImpact(%db, %obj, %col)
	{
		Parent::onImpact(%db, %obj, %col);

		if(isObject(%obj) && %db.minImpactDamage >= 0 && %db.maxImpactDamage > 0)
			%obj.impactCheck(%col, %obj.getVelocity());
	}
	
	function WheeledVehicleData::onImpact(%db, %obj, %col)
	{
		Parent::onImpact(%db, %obj, %col);

		if(isObject(%obj) && %db.minImpactDamage >= 0 && %db.maxImpactDamage > 0)
			%obj.impactCheck(%col, %obj.getVelocity());
	}
};
activatePackage(T2VehicleImpacts);

function FlyingVehicle::impactCheck(%obj, %col, %vec) { WheeledVehicle::impactCheck(%obj, %col, %vec); }

function WheeledVehicle::impactCheck(%obj, %col, %vec)
{
	if(!isObject(%obj) || getSimTime() - %obj.lastImpactTime < 100 || (isObject(%col) && %col.getType() & $TypeMasks::PlayerObjectType))
		return;

	%db = %obj.getDataBlock();

	%vel = vectorLen(%vec);

	%min = %db.minImpactSpeed;
	%max = %db.maxImpactSpeed;
	%rMax = mClampF(%max - %min, 0, %max);
	%mult = (mClampF(%vel - %min, 0, %rMax) / %rMax);

	%dmg = %db.minImpactDamage + %mult * (%db.maxImpactDamage - %db.minImpactDamage);

	%obj.schedule(0, damage, %obj, %obj.getPosition(), %dmg, $DamageType::Fall);
	%obj.lastImpactTime = getSimTime();
}