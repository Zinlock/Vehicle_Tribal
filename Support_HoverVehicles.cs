// !! this code is atrocious !!

$HoverMask = $TypeMasks::StaticShapeObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::WaterObjectType;

if(!isFunction(FlyingVehicleData, onAdd))
	eval("function FlyingVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(FlyingVehicleData, onTrigger))
	eval("function FlyingVehicleData::onTrigger(%db, %obj, %trig, %val) { }");

package T2HoverVehicles
{
	function FlyingVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj) && %db.isHoverVehicle)
		{
			%obj.lastHover = getSimTime();
			%obj.hoverLoop();
		}
	}

	function FlyingVehicleData::onTrigger(%db, %obj, %trig, %val)
	{
		Parent::onTrigger(%db, %obj, %trig, %val);

		if(%trig == 2 && %db.hoverBrakes)
			%obj.hoverBrake = %val;
	}
};
schedule(0, 0, activatePackage, T2HoverVehicles); // for some reason the onTrigger function never gets called unless the package activation is delayed

function mFloatLerp(%from, %to, %at) // if you use this, to and from are flipped... whoops
{
	return %to + %at * (%from - %to);
}

function FlyingVehicle::hoverLoop(%obj)
{
	cancel(%obj.hover);

	%db = %obj.getDataBlock();

	// * step 1
	// check for the ground

	%vel = %obj.getVelocity();
	%xyvel = setWord(%vel, 2, 0);
	%pos = vectorAdd(%obj.getPosition(), vectorScale(%obj.getUpVector(), getWord(%db.massCenter, 2)));
	%end = VectorAdd(%pos, "0 0 -256");

	if(%vel $= "0 0 0")
		%obj.setVelocity("0 0 0.1");

	%ray = containerRayCast(%pos, %end, $HoverMask);

	%hit = %end;

	if(isObject(%ray))
	{
		%hit = posFromRaycast(%ray);
	}
	
	%dist = VectorDist(%pos, %hit);
	%sub = vectorSub(%hit, %pos);
	%dir = vectorNormalize(%sub);

	%tick = (getSimTime() - %obj.lastHover) / 32;

	%force = %db.hoverMinDownForce * %tick;
	%forceMax = %db.hoverMaxDownForce * %tick;
	%maxDist = %db.hoverMaxDistance;
	%minDist = %db.hoverMinDistance;

	// calculate down force

	%realEnd = mClampF(%maxDist - %minDist, 0, %maxDist);
	%mult = (mClampF(%dist - %minDist, 0, %realEnd) / %realEnd);
			
	%nvel = vectorScale(%dir, mFloatLerp(%forceMax, %force, %mult));
	%nz = getWord(%nvel, 2);
	%z = getWord(%sub, 2);

	%down = false;

	if(%dist > %minDist && %dist > %db.hoverCloseDistance && %obj.getWaterCoverage() <= 0.1)
	{
		if(getWord(%vel, 2) > %db.hoverMaxFallSpeed * -1)
		{
			// apply down force
			%nvel = vectorScale(%nvel, mClampF((getSimTime() - %obj.hoverDownTime) / (%db.hoverMaxDownTime * 1000), 0, 1));
			%obj.applyImpulse(%pos, %nvel);
			%down = true;
		}
	}
	else if(%db.hoverCloseUpForce > 0 && (%dist < %db.hoverCloseDistance || (%db.hoverOverWater && %obj.getWaterCoverage() > 0.1)))
	{
		// we're actually too close (or underwater); apply up force
		%obj.applyImpulse(%pos, vectorScale("0 0 1", (%db.hoverCloseUpForce) * %tick));
	}

	// * step 2
	// check for obstacles

	if(!%obj.hoverBrake)
	{
		if(vectorLen(%xyvel) > %db.hoverClimbSpeed)
		{
			%end = VectorAdd(%pos, vectorScale(vectorNormalize(%xyvel), %db.hoverClimbDistance));
			%ray = containerRayCast(%pos, %end, $HoverMask);

			if(!isObject(%ray))
				%ray = containerRayCast(%pos, vectorAdd(%end, "0 0 -2"), $HoverMask);

			if(isObject(%ray))
			{
				%normal = normalFromRaycast(%ray);
				%dot = VectorDot(%normal, "0 0 1");
				%ang = mRadToDeg(mAcos(%dot));

				if(%ang <= %db.hoverClimbAngle)
				{
					%force = %db.hoverClimbForce * %tick;

					%obj.applyImpulse(%pos, vectorScale("0 0 1", %force));

					if(%down)
						%down = false;
				}
			}
		}
	}
	
	// * step 3
	// break the fall
	
	if(!%obj.hoverBrake)
	{
		if(vectorLen(%obj.getVelocity()) > %db.hoverRecoverSpeed)
		{
			%end = VectorAdd(%pos, vectorScale(vectorNormalize(%vel), %db.hoverRecoverDistance));
			%ray = containerRayCast(%pos, %end, $HoverMask);

			if(isObject(%ray))
			{
				%normal = normalFromRaycast(%ray);
				%dot = VectorDot(%normal, "0 0 1");
				%ang = mRadToDeg(mAcos(%dot));

				if(%ang <= %db.hoverRecoverAngle)
				{
					%force = %db.hoverRecoverForce * %tick;

					%obj.applyImpulse(%pos, vectorScale(%normal, %force));
				}
			}
		}
	}

	// * step 4
	// apply brakes

	if(%obj.hoverBrake)
	{
		%force = -1 * mClampF(%db.hoverBrakeForce * %tick, 0, 1) * %db.mass;
		%obj.applyImpulse(%pos, setWord(vectorScale(%obj.getVelocity(), %force), 2, 0));
	}
	
	// * step 5
	// reset the down force timer if necessary

	if(!%down)
		%obj.hoverDownTime = "";
	else if(%obj.hoverDownTime $= "")
		%obj.hoverDownTime = getSimTime();

	%obj.lastHover = getSimTime();
	%obj.hover = %obj.schedule(50, hoverLoop);
}