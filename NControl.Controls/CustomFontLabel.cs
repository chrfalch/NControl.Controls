using System;
using Xamarin.Forms;
using System.Reflection;
using System.IO;

namespace NControl.Controls
{
	/// <summary>
	/// Custom font label.
	/// </summary>
	public abstract class CustomFontLabel: Label
	{		
		/// <summary>
		/// Returns font data as a stream of bytes.
		/// </summary>
		/// <value>The font data.</value>
		public abstract string FontDataResourcePath { get; }

		/// <summary>
		/// Gets the font data.
		/// </summary>
		/// <value>The font data.</value>
		public virtual byte[] FontData
		{
			get
			{				
				var assembly = this.GetType().GetTypeInfo ().Assembly;
				using(var s = assembly.GetManifestResourceStream (FontDataResourcePath))
				{
					if (s == null)
						throw new InvalidOperationException ("Could not find font resource " + FontDataResourcePath + ".");

					var data = new byte[s.Length];
					s.Read (data, 0, data.Length);
					return data;
				}
			}
		}
	}
}

