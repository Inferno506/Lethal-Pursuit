using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public class SpaceshipKeyboardProfile1 : UnityInputDeviceProfile
	{
		public SpaceshipKeyboardProfile1()
		{
			Name = "Keyboard";
			Meta = "A keyboard profile for steering a spaceship.";

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
				new InputControlMapping
				{
					Handle = "Spacebar",
					Target = InputControlType.RightTrigger,
					Source = KeyCodeButton( KeyCode.Space )
				},
				new InputControlMapping
				{
					Handle = "Left Shift",
					Target = InputControlType.LeftTrigger,
					Source = KeyCodeButton( KeyCode.LeftShift )
				},
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "AD Keys X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
				},
				new InputControlMapping
				{
					Handle = "SW Keys Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
				},
			};
		}
	}
}

