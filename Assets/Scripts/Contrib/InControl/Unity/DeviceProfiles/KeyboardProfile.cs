using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class KeyboardProfile : UnityInputDeviceProfile
	{
		public KeyboardProfile()
		{
			Name = "Keyboard";
			Meta = "";

			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.0f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "Spacebar",
					Target = InputControlType.Action1,
					Source = "space"
				},
				new InputControlButtonMapping()
				{
					Handle = "Z Key",
					Target = InputControlType.Action2,
					Source = "z"
				},
				new InputControlButtonMapping()
				{
					Handle = "X Key",
					Target = InputControlType.Action3,
					Source = "x"
				},
				new InputControlButtonMapping()
				{
					Handle = "C Key",
					Target = InputControlType.Action4,
					Source = "c"
				}
			};

			AnalogMappings = new InputControlAnalogMapping[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys X",
					Target = InputControlType.LeftStickX,
					Source = "left right"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys Y",
					Target = InputControlType.LeftStickY,
					Source = "down up"
				},
				
				new InputControlAnalogMapping()
				{
					Handle = "AD Keys",
					Target = InputControlType.LeftStickX,
					Source = "a d"
				},
				new InputControlAnalogMapping()
				{
					Handle = "WS Keys",
					Target = InputControlType.LeftStickY,
					Source = "s w"
				}
			};
		}
	}
}

