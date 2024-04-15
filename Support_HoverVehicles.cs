// !! this code is atrocious !!

$HoverMask = $TypeMasks::StaticShapeObjectType | $TypeMasks::FxBrickObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::WaterObjectType;

if(!isFunction(FlyingVehicleData, onAdd))
	eval("function FlyingVehicleData::onAdd(%db, %obj) { }");

if(!isFunction(FlyingVehicleData, onTrigger))
	eval("function FlyingVehicleData::onTrigger(%db, %obj, %trig, %val) { }");

if(!isFunction(Armor, onUnMount))
	eval("function Armor::onUnMount(%db, %pl, %col, %node) { }");

package T2HoverVehicles
{
	function FlyingVehicleData::onAdd(%db, %obj)
	{
		Parent::onAdd(%db, %obj);

		if(isObject(%obj))
		{
			if(%db.isHoverVehicle)
			{
				%obj.lastHover = getSimTime();
				%obj.hoverLoop();
			}
			else if(%db.flyingHover)
			{
				%obj.lastHover = getSimTime();
				%obj.altHoverLoop();
			}
		}
	}

	function FlyingVehicleData::onTrigger(%db, %obj, %trig, %val)
	{
		Parent::onTrigger(%db, %obj, %trig, %val);

		if(%trig == 2 && %db.hoverBrakes)
			%obj.hoverBrake = %val;
	}
	
	function Armor::onUnMount(%db, %pl, %col, %node)
	{
		Parent::onUnMount(%db, %pl, %col, %node);

		if(%node == 0)
			%col.hoverBrake = false;
	}
};
schedule(0, 0, activatePackage, T2HoverVehicles); // for some reason the onTrigger function never gets called unless the package activation is delayed

function mFloatLerp(%from, %to, %at) // if you use this, to and from are flipped... whoops
{
	return %to + %at * (%from - %to);
}

function FlyingVehicle::onUpBoost(%db, %obj, %val) { }
function FlyingVehicle::onBoost(%db, %obj, %val) { }

function FlyingVehicle::altHoverLoop(%obj) // used for actual flying vehicles, not compatible with the other hover system
{
	cancel(%obj.hover);
	
	%db = %obj.getDataBlock();

	%vel = %obj.getVelocity();
	%pos = vectorAdd(%obj.getPosition(), vectorScale(%obj.getUpVector(), getWord(%db.massCenter, 2)));
	%end = VectorAdd(%pos, vectorScale("0 0 -1", %db.minHoverDist));

	if(%vel $= "0 0 0")
		%obj.setVelocity("0 0 0.1");

	%ray = containerRayCast(%pos, %end, $HoverMask);

	%hit = false;

	if(isObject(%ray))
		%hit = true;

	%tick = (getSimTime() - %obj.lastHover) / 32;

	if(!%obj.hoverBrake)
	{
		if(vectorLen(%vel) <= %db.maxHoverSpeed && !%hit)
		{
			%force = %db.hoverDownForce * %tick;

			%obj.applyImpulse(%pos, "0 0 " @ (-%force));
		}
	}
	else // flying vehicles use the brake key as boost
	{
		%drain = %db.boostEnergyDrain * %tick;

		if(%obj.getEnergyLevel() >= %db.minBoostEnergy)
		{
			if(getSimTime() - %obj.lastHoverEmpty > 100)
			{
				if(vectorLen(%vel) <= %db.maxBoostUpSpeed)
				{
					%force = %db.boostUpForce * %tick;
					%obj.applyImpulse(%pos, vectorScale(%obj.getUpVector(), %force));
				}
				else if(vectorLen(%vel) <= %db.maxBoostSpeed)
				{
					%force = %db.boostForce * %tick;
					%obj.applyImpulse(%pos, vectorScale(%obj.getForwardVector(), %force));
				}
			}
			else %obj.lastHoverEmpty = getSimTime();

			%obj.setEnergyLevel(%obj.getEnergyLevel() - %drain);
		}
		else %obj.lastHoverEmpty = getSimTime();
	}
	
	%obj.lastHover = getSimTime();
	%obj.hover = %obj.schedule(50, altHoverLoop);
}

function FlyingVehicle::hoverLoop(%obj)
{
	cancel(%obj.hover);

	%db = %obj.getDataBlock();

	// * step 1
	// check for the ground

	%isGrav = %db.isAntiGrav;

	if(%isGrav)
	{
		if(%obj.lastGroundVector $= "")
			%obj.lastGroundVector = vectorScale(vectorNormalize(%db.defaultGravity), -1);
	}
	else if(%obj.lastGroundVector $= "")
		%obj.lastGroundVector = "0 0 1";

	%vel = %obj.getVelocity();
	%xyvel = setWord(%vel, 2, 0);
	%pos = vectorAdd(%obj.getPosition(), vectorScale(%obj.getUpVector(), getWord(%db.massCenter, 2)));
	%end = VectorAdd(%pos, vectorScale(%obj.lastGroundVector, -256));

	if(%vel $= "0 0 0")
		%obj.setVelocity("0 0 0.1");

	%ray = containerRayCast(%pos, %end, $HoverMask);

	%hit = %end;

	if(isObject(%ray))
		%hit = posFromRaycast(%ray);
	
	%dist = VectorDist(%pos, %hit);
	%sub = vectorSub(%hit, %pos);
	%dir = vectorNormalize(%sub);

	%tick = (getSimTime() - %obj.lastHover) / 32;

	%force = %db.hoverMinDownForce * %tick;
	%forceMax = %db.hoverMaxDownForce * %tick;
	%maxDist = %db.hoverMaxDistance;
	%minDist = %db.hoverMinDistance;

	%upVector = "0 0 1";

	if(%isGrav)
	{
		%ray2 = containerRayCast(%pos, vectorAdd(%pos, vectorScale(%obj.getUpVector(), -%db.antiGravDistance)), $HoverMask);

		if(isObject(%ray2))
		{
			%upVector = normalFromRaycast(%ray2);
			%obj.lastGroundTime = getSimTime();
			%obj.lastGroundVector = %upVector;

			// %obj.addVelocity("0 0 " @ (%tick * mClampF(1 - (VectorDot(%upVector, "0 0 1") + 1) / 2, 0, 1)));
			%angle = mRadToDeg(mAcos(VectorDot(%upVector, "0 0 1")));
			%down = vectorScale(%upVector, -1);
			%angMult = ((%angle / 180) + 1) / 2;
			%obj.addVelocity(vectorAdd(vectorScale(%down, %angMult * %db.antiGravStickForce), "0 0 " @ %angMult * %db.antiGravForce));
		}
		else
		{
			if(getSimTime() - %obj.lastGroundTime >= %db.antiGravDecayTime * 1000)
			{
				%upVector = vectorScale(vectorNormalize(%db.defaultGravity), -1);
				%obj.lastGroundVector = %upVector;
			}
			else
				%upVector = %obj.lastGroundVector;
		}

		%dev = vectorSub(%obj.getUpVector(), %upVector);

		// drawArrow(%pos, %obj.getUpVector(), "1 1 0 0.5", 10, 0.25).schedule(500, delete);
		// drawArrow(%pos, %upVector, "0 1 0 0.5", 10, 0.25).schedule(500, delete);
		// drawArrow(%pos, %dev, "1 0 0 0.5", 10, 0.25).schedule(500, delete);

		if(vectorLen(%dev) > 0.02)
		{
			%obj.applyImpulse(vectorAdd(%pos, vectorNormalize(%dev)), vectorScale(%obj.getUpVector(), vectorLen(%dev) * %db.gravityForce));
			%obj.applyImpulse(vectorAdd(%pos, vectorScale(vectorNormalize(%dev), -1)), vectorScale(%obj.getUpVector(), vectorLen(%dev) * -%db.gravityForce));
		}
	}

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
		%obj.applyImpulse(%pos, vectorScale(%upVector, (%db.hoverCloseUpForce) * %tick));
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
				%ray = containerRayCast(%pos, vectorAdd(%end, vectorScale(%upVector, -2)), $HoverMask);

			if(isObject(%ray))
			{
				%normal = normalFromRaycast(%ray);
				%dot = VectorDot(%normal, %upVector);
				%ang = mRadToDeg(mAcos(%dot));

				if(%ang <= %db.hoverClimbAngle)
				{
					%force = %db.hoverClimbForce * %tick;

					%obj.applyImpulse(%pos, vectorScale(%upVector, %force));

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
				%dot = VectorDot(%normal, %upVector);
				%ang = mRadToDeg(mAcos(%dot));

				if(%ang <= %db.hoverRecoverAngle)
				{
					%force = %db.hoverRecoverForce * %tick;

					%obj.applyImpulse(%pos, vectorScale(%normal, %force));

					if(%isGrav)
					{
						%upVector = normalFromRaycast(%ray);
						%obj.lastGroundVector = %upVector;
					}
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