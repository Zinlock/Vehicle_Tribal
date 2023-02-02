if(!isFunction(Armor, onMount))
	eval("function Armor::onMount(%db, %pl, %col, %node) { }");

package T2VehiclePrint
{
	function Armor::onMount(%db, %pl, %col, %node)
	{
		Parent::onMount(%db, %pl, %col, %node);

		if(isObject(%pl))
			%pl.vehiclePrintLoop(%node);
	}
};
activatePackage(T2VehiclePrint);

function mFloatLerp(%from, %to, %at)
{
    return %to + %at * (%from - %to);
}

function RGBLerp(%from, %to, %at)
{
    %r = mFloatLerp(getWord(%from, 0), getWord(%to, 0), %at);
    %g = mFloatLerp(getWord(%from, 1), getWord(%to, 1), %at);
    %b = mFloatLerp(getWord(%from, 2), getWord(%to, 2), %at);

    return (%r SPC %g SPC %b);
}

function hex2rgba(%hex)
{
	%len = strLen(%hex);

	if(%len > 0)
	{
		%rh = getSubStr(%hex, 0, 2);
		%r = eval("return 0x" @ %rh @ ";");
		%str = %r / 255;
	}

	if(%len > 2)
	{
		%gh = getSubStr(%hex, 2, 2);
		%g = eval("return 0x" @ %gh @ ";");
		%str = %str SPC %g / 255;
	}

	if(%len > 4)
	{
		%bh = getSubStr(%hex, 4, 2);
		%b = eval("return 0x" @ %bh @ ";");
		%str = %str SPC %b / 255;
	}

	if(%len > 6)
	{
		%ah = getSubStr(%hex, 6, 2);
		%a = eval("return 0x" @ %ah @ ";");
		%str = %str SPC %a / 255;
	}

	return %str;
}

function rgb2hex( %rgb )
{
    %r = comp2hex( 255 * getWord( %rgb, 0 ) );
    %g = comp2hex( 255 * getWord( %rgb, 1 ) );
    %b = comp2hex( 255 * getWord( %rgb, 2 ) );

    return %r @ %g @ %b;
}

function comp2hex( %comp )
{
    %left = mFloor( %comp / 16 );
    %comp = mFloor( %comp - %left * 16 );

    %left = getSubStr( "0123456789ABCDEF", %left, 1 );
    %comp = getSubStr( "0123456789ABCDEF", %comp, 1 );

    return %left @ %comp;
}

function AIPlayer::vehiclePrintLoop(%pl, %node) {}

function Player::vehiclePrintLoop(%pl, %node)
{
	cancel(%pl.vehiclePrint);

	if(%pl.getDamagePercent() >= 1.0)
		return;

	%obj = %pl.getObjectMount();

	if(!isObject(%obj))
		return;

	%db = %obj.getDataBlock();

	if(!%db.useVehiclePrints[%node])
		return;
	
	%cl = %pl.Client;

	if(!isObject(%cl))
		return;
	
	%hcolMax = hex2rgba("44FF44");
	%hcolMin = hex2rgba("FF0000");

	%hp = 1 - %obj.getDamagePercent();

	%hcol = rgb2hex(RGBLerp(%hcolMax, %hcolMin, %hp));

	%str = "<color:FFFFFF><font:arial bold:16><spush><spush>HULL <color:" @ %hcol @ "><font:arial:16>" @ mCeil(%hp * 100) @ "%<just:right><color:44FF44>" @ mFloor(vectorLen(%obj.getVelocity())) @ "u/s <spop>SPD<br>";

	if(%db.useEnergyPrints)
	{
		%ecolMax = hex2rgba("4499FF");
		%ecolMin = hex2rgba("FF44EE");

		%erg = %obj.getEnergyLevel() / %db.maxEnergy;

		%ecol = rgb2hex(RGBLerp(%ecolMax, %ecolMin, %erg));

		%estr = "<just:left>ERG <color:" @ %ecol @ "><font:arial:16>" @ mCeil(%erg * 100) @ "%";
	}
	
	if(%obj.useExtraPrints)
		%hstr = "<just:right><color:" @ stripMLControlChars(%obj.extraPrintColor) @ "><font:arial:16>" @ stripMLControlChars(%obj.extraPrintText) @ " <spop>" @ stripMLControlChars(%obj.extraPrintLabel);

	commandToClient(%cl, 'bottomPrint', %str @ %estr @ %hstr, 1, 1);

	%pl.vehiclePrint = %pl.schedule(100, vehiclePrintLoop, %node);
}