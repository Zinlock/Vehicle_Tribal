datablock FlyingVehicleData(T2WildcatGravVehicle : T2WildcatVehicle)
{
	uiName = "T2: Gravity Wildcat";

	autoInputDamping = 0.9;
	autoAngularForce = 0;

	minImpactSpeed = 25;
	minImpactDamage = 5;
	maxImpactSpeed = 90;
	maxImpactDamage = 50;

	isAntiGrav = true; // enables anti gravity thingy
	antiGravDecay = true; // revert back to default gravity if in midair
	antiGravStickForce = 1.0; // downwards force to apply against surfaces
	antiGravForce = 1.5; // vertical force to apply against gravity
	antiGravDecayTime = 1; // grace period before reverting to default gravity
	antiGravDistance = 16; // max distance to look for a surface
	defaultGravity = "0 0 -1"; // default gravity direction
	gravityForce = 150; // force to recenter vehicle with
};

function T2WildcatGravVehicle::onEngineLowSpeed(%db, %obj)
{
	%obj.unMountImage(0);
	%obj.unMountImage(1);
	%obj.unMountImage(2);
}

function T2WildcatGravVehicle::onEngineMedSpeed(%db, %obj)
{
	%obj.unMountImage(0);
	%obj.unMountImage(1);
	%obj.mountImage(T2WildcatJetImage, 2);
}

function T2WildcatGravVehicle::onEngineHighSpeed(%db, %obj)
{
	%obj.mountImage(T2Contrail2Image, 0);
	%obj.mountImage(T2Contrail3Image, 1);
	%obj.mountImage(T2WildcatJetImage, 2);
}