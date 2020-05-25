// Taken from Downwell Steam release, cleaned up and modified for use with Custom Controls Mod.
global.padUp = gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyChoose);
global.padUpHeld = gamepad_button_check(global.GamepadSlot, global.GamepadKeyChoose);
global.padUpRel = gamepad_button_check_released(global.GamepadSlot, global.GamepadKeyChoose);
global.padCancel = gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyCancel);
if (global.isPaused || global.death)
{
    if (!global.padLeft && !global.padRight)
    {
        padLeftPressed = (gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyLeft) || gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyUp));
        padRightPressed = (gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyRight) || gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyDown));
    }
    else
    {
        padLeftPressed = false;
        padRightPressed = false;
    }
    global.padLeft = (gamepad_button_check(global.GamepadSlot, global.GamepadKeyLeft) || gamepad_button_check(global.GamepadSlot, global.GamepadKeyUp));
    global.padRight = (gamepad_button_check(global.GamepadSlot, global.GamepadKeyRight) || gamepad_button_check(global.GamepadSlot, global.GamepadKeyDown));
    if (gamepad_axis_value(global.GamepadSlot, gp_axislh) != 0)
    {
        if (gamepad_axis_value(global.GamepadSlot, gp_axislh) > 0)
        {
            global.padRight = true;
            if (!analogPressed)
            {
                padRightPressed = true;
                analogPressed = true;
            }
        }
        else
        {
            global.padLeft = true;
            if (!analogPressed)
            {
                padLeftPressed = true;
                analogPressed = true;
            }
        }
    }
    else if (gamepad_axis_value(global.GamepadSlot, gp_axislv) != 0)
    {
        if (gamepad_axis_value(global.GamepadSlot, gp_axislv) > 0)
        {
            global.padRight = true;
            if (!analogPressed)
            {
                padRightPressed = true;
                analogPressed = true;
            }
        }
        else
        {
            global.padLeft = true;
            if (!analogPressed)
            {
                padLeftPressed = true;
                analogPressed = true;
            }
        }
    }
    else
        analogPressed = false;
}
else
{
    global.padLeft = gamepad_button_check(global.GamepadSlot, global.GamepadKeyLeft);
    padLeftPressed = gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyLeft);
    global.padRight = gamepad_button_check(global.GamepadSlot, global.GamepadKeyRight);
    padRightPressed = gamepad_button_check_pressed(global.GamepadSlot, global.GamepadKeyRight);
	if (global.padCancel && !global.pauseInput)
		global.pauseInput = true; // Allow to pause the game from gamepad, woohoo!
    if (gamepad_axis_value(global.GamepadSlot, gp_axislh) != 0)
    {
        if (gamepad_axis_value(global.GamepadSlot, gp_axislh) > 0)
        {
            global.padRight = true;
            if (!analogPressed)
            {
                padRightPressed = true;
                analogPressed = true;
            }
        }
        else
        {
            global.padLeft = true;
            if (!analogPressed)
            {
                padLeftPressed = true;
                analogPressed = true;
            }
        }
    }
    else
        analogPressed = false;
}
global.dUp = global.padUp || (global.dTouchUp || vkUpPressed);
global.dUpHeld = global.padUpHeld || (global.dTouchUpHeld || vkUpHeld);
global.dUpRel = global.padUpRel || (global.dTouchUpRel || vkUpReleased);
global.dLeft = global.padLeft || (global.dTouchLeft || vkLeftHeld);
global.dLeftPressed = padLeftPressed || (global.dTouchLeftPressed || vkLeftPressed);
global.dRight = global.padRight || (global.dTouchRight || vkRightHeld);
global.dRightPressed = padRightPressed || (global.dTouchRightPressed || vkRightPressed);
global.anyInput = global.dUp || (global.dLeftPressed || global.dRightPressed);
vkLeftPressed = false;
vkRightPressed = false;
vkUpPressed = false;
vkUpReleased = false;