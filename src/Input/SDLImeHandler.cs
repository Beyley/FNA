using System;
using SDL3;

namespace Microsoft.Xna.Framework.Input
{
	/// <summary>
	/// Integrate IME to SDL2 platform.
	/// </summary>
	internal class SDLImeHandler : ImmService
	{
		public SDLImeHandler()
		{
		}

		public override event EventHandler<TextCompositionEventArgs> TextComposition;
		public override event EventHandler<TextInputEventArgs> TextInput;

		public static IntPtr WindowHandle { get; set; }

		public void OnTextInput(char charInput, Keys key)
		{
			if (TextInput != null)
				TextInput.Invoke(this, new TextInputEventArgs(charInput, key));
		}

		public void OnTextComposition(IMEString compositionText, int cursorPosition)
		{
			if (TextComposition != null)
				TextComposition.Invoke(this, new TextCompositionEventArgs(compositionText, cursorPosition));
		}

		public override void StartTextInput()
		{
			if (IsTextInputActive)
				return;

			SDL.SDL_StartTextInput(WindowHandle);
			IsTextInputActive = true;
		}

		public override void StopTextInput()
		{
			SDL.SDL_StopTextInput(WindowHandle);
			IsTextInputActive = false;
		}

		public override void SetTextInputRect(Rectangle rect)
		{
			if (!IsTextInputActive)
				return;

			SDL.SDL_Rect sdlRect = new()
			{
				x = rect.X,
				y = rect.Y,
				w = rect.Width,
				h = rect.Height,
			};

			// TODO: fill in cursor
			// the offset of the current cursor location relative to rect->x, in window coordinates.
			SDL.SDL_SetTextInputArea(WindowHandle, ref sdlRect, 0);
			FNAPlatform.SetTextInputRectangle(WindowHandle, rect);
		}
	}
}
