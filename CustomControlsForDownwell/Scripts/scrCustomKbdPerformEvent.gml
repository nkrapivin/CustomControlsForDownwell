// != vk_ stuff is to prevent double-presses.
if (keyboard_check_pressed(global.KeyboardKeyLeft) && global.KeyboardKeyLeft != vk_left) event_perform(ev_keypress, vk_left);
if (keyboard_check_released(global.KeyboardKeyLeft) && global.KeyboardKeyLeft != vk_left) event_perform(ev_keyrelease, vk_left);

if (keyboard_check_pressed(global.KeyboardKeyRight) && global.KeyboardKeyRight != vk_right) event_perform(ev_keypress, vk_right);
if (keyboard_check_released(global.KeyboardKeyRight) && global.KeyboardKeyRight != vk_right) event_perform(ev_keyrelease, vk_right);

if (keyboard_check_pressed(global.KeyboardKeyChoose) && global.KeyboardKeyChoose != vk_shift) event_perform(ev_keypress, vk_shift);
if (keyboard_check_released(global.KeyboardKeyChoose) && global.KeyboardKeyChoose != vk_shift) event_perform(ev_keyrelease, vk_shift);

if (keyboard_check_pressed(global.KeyboardKeyCancel) && global.KeyboardKeyCancel != vk_escape) event_perform(ev_keypress, vk_escape);

if (keyboard_check(vk_shift) && keyboard_check_pressed(ord("R")) && keyboard_check(vk_control))
{
    audio_stop_all();
	game_restart();
}