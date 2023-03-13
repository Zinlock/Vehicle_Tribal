// function by conan
// Full ballistics script in Weapon_Turrets
// edited to ignore players

function vCalculateFutureGravityPosition(%pos, %vel, %time, %gravity)
{
	%xy = getWords(%vel, 0, 1);
	%z = getWords(%vel, 2);

	%xyPos = vectorAdd(vectorScale(%xy, %time), %pos);
	%zDelta = (%z * %time) - (%gravity * %time * %time);
	%finalPos = vectorAdd(%xyPos, "0 0 " @ %zDelta);

	%masks = $TypeMasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::TerrainObjectType;
	%ray = containerRaycast(vectorAdd(%pos, "0 0 0.01"), %finalPos, %masks);
	%hit = getWord(%ray, 0);
	%hitloc = getWords(%ray, 1, 3);
	if (isObject(%hit))
	{
			%finalPos = %hitloc;
	}
	// echo(%hit SPC " | " @ %hitloc @ " | " @ %finalPos);
	return %finalPos;
}