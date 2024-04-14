using FribergFastigheterApi.HelperClasses;

namespace BulkImageDownloader
{
	/// <summary>
	/// Main class.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	internal class Program
	{
		/// <summary>
		/// The main method.
		/// </summary>
		/// <param name="args">The argument list.</param>
		/// <returns>A <see cref="Task"/>.</returns>
		static async Task Main(string[] args)
		{
			if (args.Count() == 0)
			{
				Console.WriteLine("No argument was provided.");
				ExitOnKeyPress();
			}
			else
			{
				string filePath = args.First();

				if (!File.Exists(filePath))
				{
					Console.WriteLine("The input file doesn't exists.");
					Console.WriteLine();
					ExitOnKeyPress();
				}

				List<string> fileUrls = File.ReadAllLines(filePath).Where(x => Uri.IsWellFormedUriString(x, UriKind.Absolute)).ToList();

				if (fileUrls.Count == 0)
				{
					Console.WriteLine("The input file didn't contain any valid urls.");
					Console.WriteLine();
					ExitOnKeyPress();
				}

				var downloader = new BulkFileDownloader(fileUrls, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads"));
				await downloader.DownloadFiles();
			}

			ExitOnKeyPress();
		}

		/// <summary>
		/// Exits the program on key press.
		/// </summary>
		static private void ExitOnKeyPress()
		{
			Console.ReadKey();
			Environment.Exit(0);
		}
	}
}
