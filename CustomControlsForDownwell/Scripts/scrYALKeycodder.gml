var _keymode, _keyname, _key_is_mouse;
_keyname = string_lower(argument0);
_keymode = argument1;
_key_is_mouse = (_keyname == "mb_left" || _keyname == "mb_right" || _keyname == "mb_middle" || _keyname == "mb_any" || _keyname == "mb_none");
if (_key_is_mouse) {
	switch (_keymode) {
		case "l": {
			global.KeyLeftAsMouse = true; break;
		}
		case "r": {
			global.KeyRightAsMouse = true; break;
		}
		case "s": {
			global.KeyChooseAsMouse = true; break;
		}
		case "p": {
			global.KeyCancelAsMouse = true; break;
		}
		default: break;
	}
}

return ds_map_find_value(global.keyboard_name_to_key, _keyname);