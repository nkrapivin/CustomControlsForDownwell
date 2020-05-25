ini_open("controls.ini");
global.KeyboardKeyLeft = scrYALKeycodder(ini_read_string("Keyboard", "Left", "left"));
global.KeyboardKeyRight = scrYALKeycodder(ini_read_string("Keyboard", "Right", "right"));
global.KeyboardKeyChoose = scrYALKeycodder(ini_read_string("Keyboard", "Choose", "shift"));
global.KeyboardKeyCancel = scrYALKeycodder(ini_read_string("Keyboard", "Cancel", "escape"));
global.GamepadKeyLeft = scrParseGpadConst(ini_read_string("Gamepad", "Left", gp_padl));
global.GamepadKeyRight = scrParseGpadConst(ini_read_string("Gamepad", "Right", gp_padr));
global.GamepadKeyUp = scrParseGpadConst(ini_read_string("Gamepad", "Up", gp_padu));
global.GamepadKeyDown = scrParseGpadConst(ini_read_string("Gamepad", "Down", gp_padd));
global.GamepadKeyChoose = scrParseGpadConst(ini_read_string("Gamepad", "Choose", gp_face1));
global.GamepadKeyCancel = scrParseGpadConst(ini_read_string("Gamepad", "Cancel", gp_face2));
global.GamepadDeadzone = real(ini_read_string("Gamepad", "Deadzone", "0.65"));
global.GamepadSlot = real(ini_read_string("Gamepad", "Slot", "0"));
global.GamepadAsync = false;
if (global.GamepadSlot > -1)
    gamepad_set_axis_deadzone(global.GamepadSlot, global.GamepadDeadzone);
else if (global.GamepadSlot == -1)
    global.GamepadAsync = true; // Deadzone will be set in the Async System Event.
if (string_lower(ini_read_string("Misc", "RevertCaption", "false")) == "true")
    window_set_caption("Downwell");
ini_close();