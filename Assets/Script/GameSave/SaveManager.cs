using System.IO;
using UnityEngine;

namespace ROC
{
	/// 
	/// Manager class for load/saving save files.
	/// 
	internal static class SaveManager
	{
		/// 
		/// Save an instance as a JSON file.
		/// 
		/// <param name="path">Path to the file.</param>
		/// <param name="content">Instance of <see cref="object"/> to convert into JSON format.</param>
		internal static void Save(string path, object content)
		{
			var fileContent = JsonUtility.ToJson(content);
			File.WriteAllText(Path.Combine(Application.dataPath, path), fileContent);
		}

		/// 
		/// Load a JSON file content as an instance of <see cref="object"/>.
		/// 
		/// <param name="path">Path to the file.</param>
		/// <typeparam name="T">The type of object you want to convert and receive from JSON content.</typeparam>
		/// <returns>An instance of type <see cref="T"/> from JSON file content.</returns>
		internal static T Load<T>(string path)
		{
			var fileContent = File.ReadAllText(Path.Combine(Application.dataPath, path));
			return JsonUtility.FromJson<T>(fileContent);
		}
	}
}