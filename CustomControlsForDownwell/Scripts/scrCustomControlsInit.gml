global.KeyLeftAsMouse = false;
global.KeyRightAsMouse = false;
global.KeyChooseAsMouse = false;
global.KeyCancelAsMouse = false;
global.GamepadAsync = false;

ini_open("controls.ini");

// Keyboard.
global.KeyboardKeyLeft = scrYALKeycodder(ini_read_string("Keyboard", "Left", "left"), "l");
global.KeyboardKeyRight = scrYALKeycodder(ini_read_string("Keyboard", "Right", "right"), "r");
global.KeyboardKeyChoose = scrYALKeycodder(ini_read_string("Keyboard", "Choose", "shift"), "s");
global.KeyboardKeyCancel = scrYALKeycodder(ini_read_string("Keyboard", "Cancel", "escape"), "p");

// Gamepad.
global.GamepadKeyLeft = scrParseGpadConst(ini_read_string("Gamepad", "Left", gp_padl));
global.GamepadKeyRight = scrParseGpadConst(ini_read_string("Gamepad", "Right", gp_padr));
global.GamepadKeyUp = scrParseGpadConst(ini_read_string("Gamepad", "Up", gp_padu));
global.GamepadKeyDown = scrParseGpadConst(ini_read_string("Gamepad", "Down", gp_padd));
global.GamepadKeyChoose = scrParseGpadConst(ini_read_string("Gamepad", "Choose", gp_face1));
global.GamepadKeyCancel = scrParseGpadConst(ini_read_string("Gamepad", "Cancel", gp_face2));
global.GamepadDeadzone = real(ini_read_string("Gamepad", "Deadzone", "0.65"));
global.GamepadSlot = real(ini_read_string("Gamepad", "Slot", "0"));

// Gamepad Async System Event.
if (global.GamepadSlot > -1)
    gamepad_set_axis_deadzone(global.GamepadSlot, global.GamepadDeadzone);
else if (global.GamepadSlot == -1)
    global.GamepadAsync = true; // Deadzone will be set in the Async System Event.
	
// ResetCaption thing.
if (string_lower(ini_read_string("Misc", "RevertCaption", "false")) == "true")
    window_set_caption("Downwell");

ini_close();