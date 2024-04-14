using System.Collections.Concurrent;

namespace FribergFastigheterApi.HelperClasses
{
	/// <summary>
	/// A class that supports downloading files in bulk.
	/// </summary>
	/// <!-- Author: Jimmie -->
	/// <!-- Co Authors: -->
	public class BulkFileDownloader
	{
		#region Fields

		/// <summary>
		/// The folder to save the downloaded files in.
		/// </summary>
		private string _downloadFolderPath = "";

		/// <summary>
		/// Files waiting to be downloaded.
		/// </summary>
		ConcurrentBag<string> _queuedFilesToDownload = new ConcurrentBag<string>();

		/// <summary>
		/// A semaphore used to support concurrent actions.
		/// </summary>
		private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

        #endregion

        #region Constructors

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="fileUrls">A collection of urls for the file to download.</param>
		/// <param name="downloadFolderPath">The folder to save the downloaded files in.</param>
        public BulkFileDownloader(List<string> fileUrls, string downloadFolderPath)
        {
			_queuedFilesToDownload = new ConcurrentBag<string>(fileUrls.ToHashSet());
			_downloadFolderPath = downloadFolderPath;
		}

        #endregion

        #region Methods

		/// <summary>
		/// Downloades the file.
		/// </summary>
		/// <returns>A <see cref="Task"/>.</returns>
		public async Task DownloadFiles()
		{			
			List<Task> downloadTasks = new List<Task>();
			int numFilesToDownload = _queuedFilesToDownload.Count;
			int numFilesDownloaded = 0;

			if (!Directory.Exists(_downloadFolderPath))
			{
				Directory.CreateDirectory(_downloadFolderPath);
			}

			for (int i = 0; i < 2; i++)
			{
				downloadTasks.Add(Task.Run(
				async () =>
				{
					using (HttpClient httpClient = new HttpClient())
					{
						while (_queuedFilesToDownload.TryTake(out string? targetUrl))
						{
							using (var webStream = await httpClient.GetStreamAsync(targetUrl))
							{
								var tempImageFilePath = Path.GetTempFileName();
								using (var fileStream = new FileStream(tempImageFilePath, FileMode.Create))
								{
									await webStream.CopyToAsync(fileStream);
                                }
								File.Move(tempImageFilePath, Path.Combine(_downloadFolderPath, Path.GetFileName(targetUrl)), overwrite: true);
								await _semaphoreSlim.WaitAsync();
								Interlocked.Increment(ref numFilesDownloaded);
								await Console.Out.WriteLineAsync($"Finished {numFilesDownloaded} of {numFilesToDownload} files.");
								_semaphoreSlim.Release();
							}
						}
					}
				}));
			}

			await Task.WhenAll(downloadTasks);
		}

		#endregion
	}
}
