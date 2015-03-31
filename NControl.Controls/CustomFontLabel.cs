using System;
using Xamarin.Forms;
using System.Reflection;
using System.IO;

namespace NControl.Controls
{
	/// <summary>
	/// Custom font label.
	/// </summary>
	public class CustomFontLabel: Label
	{		
		/// <summary>
		/// Returns font data as a stream of bytes.
		/// </summary>
		/// <value>The font data.</value>
		public string FontDataResourcePath { get;set; }

		/// <summary>
		/// Gets or sets the name of the font data assembly.
		/// </summary>
		/// <value>The name of the font data assembly.</value>
		public Assembly FontDataAssembly { get; set; }

		/// <summary>
		/// Gets the font data.
		/// </summary>
		/// <value>The font data.</value>
		public virtual byte[] FontData
		{
			get
			{
				// Load assembly
				Assembly assembly = null;
				assembly = FontDataAssembly ?? this.GetType ().GetTypeInfo ().Assembly;

				var data = GetBytesFromResourceStream (assembly, FontDataResourcePath);
				if (data != null)
					return data;				

				throw new InvalidOperationException ("Could not find font data embedded as a resource: " + FontDataResourcePath);
			}
		}

		/// <summary>
		/// Gets the bytes from resource stream.
		/// </summary>
		/// <returns>The bytes from resource stream.</returns>
		/// <param name="assembly">Assembly.</param>
		/// <param name="streamPath">Stream path.</param>
		private byte[] GetBytesFromResourceStream(Assembly assembly, string streamPath)
		{
			using(var s = assembly.GetManifestResourceStream (streamPath))
			{
				if (s == null)
					return null;

				var data = new byte[s.Length];
				s.Read (data, 0, data.Length);
				return data;
			}
		}
	}
}

