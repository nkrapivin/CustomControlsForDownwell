if (global.GamepadAsync && ds_map_exists(async_load, "event_type") && (ds_map_find_value(async_load, "event_type") == "gamepad discovered"))
{
    global.GamepadSlot = ds_map_find_value(async_load, "pad_index")
	gamepad_set_axis_deadzone(global.GamepadSlot, global.GamepadDeadzone)
}