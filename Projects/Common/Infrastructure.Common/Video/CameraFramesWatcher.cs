﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using FiresecAPI.Models;

namespace Infrastructure.Common.Video
{
	public class CameraFramesWatcher
	{
		Thread VideoThread { get; set; }
		private MjpegCamera MjpegCamera { get; set; }
		private readonly int _imagesBufferSize;
		private readonly int _imagesBufferFrameInterval;
		private int _imagesBufferIndex;
		public List<CameraFrame> CameraFrames { get; private set; }
		private readonly Object _loker;
		private List<StandbyFrames> StandbyFramesList { get; set; }
		private readonly string _videoArchivePath;
		public CameraFramesWatcher(Camera camera, int imagesBufferSizeBeforeEvent = 10, int imagesBufferFrameInterval = 1)
		{
			_videoArchivePath = Path.Combine(AppDataFolderHelper.GetServerAppDataPath(), "VideoArchive");
			StandbyFramesList = new List<StandbyFrames>();
			_imagesBufferSize = imagesBufferSizeBeforeEvent;
			_imagesBufferFrameInterval = imagesBufferFrameInterval;
			MjpegCamera = new MjpegCamera(camera);
			_loker = new object();

			//var dir = new DirectoryInfo(_videoArchivePath);
			//if (!Directory.Exists(dir.FullName))
			//    dir.Create();
		}

		private void BmpToCameraFramesWatcher(Bitmap bmp)
		{
			var dateTime = DateTime.Now;
			lock (_loker)
			{
				if ((CameraFrames != null) && (CameraFrames.Count == _imagesBufferSize) &&
				    (dateTime - CameraFrames.Max().DateTime > TimeSpan.FromSeconds(_imagesBufferFrameInterval)))
				{
					var newbmp = new Bitmap(bmp);
					var cameraFrame = new CameraFrame(newbmp, dateTime);
					_imagesBufferIndex = _imagesBufferIndex%_imagesBufferSize;
					CameraFrames[_imagesBufferIndex] = cameraFrame;
					_imagesBufferIndex++;
					if ((StandbyFramesList != null) && (StandbyFramesList.Count > 0))
					{
						StandbyFramesList.RemoveAll(x => x.Index == x.FramesCount);
						foreach (var standbyFrames in StandbyFramesList)
						{
							var dir = new DirectoryInfo(Path.Combine(_videoArchivePath, standbyFrames.Guid.ToString()));
							if (!Directory.Exists(dir.FullName))
								dir.Create();
							standbyFrames.Index++;
							var dateString = DateTime.Now.ToString("HH-mm-ss-ff", CultureInfo.InvariantCulture);
							string fileName = dir.FullName + "\\" + "afterEvent - " + standbyFrames.Index + " (" + dateString + ") " + ".jpg";
							var bmpToSave = new Bitmap(bmp);
							bmpToSave.Save(fileName, ImageFormat.Jpeg);
							bmpToSave.Dispose();
						}
					}
				}
			}
		}

		void InitializeCameraFramesWatcher()
		{
			_imagesBufferIndex = 0;
			CameraFrames = new List<CameraFrame>(_imagesBufferSize);
			for (int i = 0; i < _imagesBufferSize; i++)
				CameraFrames.Add(new CameraFrame(new Bitmap(100, 100), new DateTime()));
		}

		public void Save(Guid guid, int imagesBufferSizeAfterEvent)
		{
			var dir = new DirectoryInfo(Path.Combine(_videoArchivePath, guid.ToString()));
			if (Directory.Exists(dir.FullName))
				Directory.Delete(dir.FullName, true);
			dir.Create();
			int i = 1;
			lock (_loker)
			{
				CameraFrames.Sort();
				foreach (var cameraFrame in CameraFrames)
				{
					var dateString = cameraFrame.DateTime.ToString("HH-mm-ss-ff", CultureInfo.InvariantCulture);
					string fileName = dir.FullName + "\\" + i + " (" + dateString + ") " + ".jpg";
					if (cameraFrame.DateTime.Ticks == 0)
						continue;
					var bmp = new Bitmap(cameraFrame.Bitmap);
					bmp.Save(fileName, ImageFormat.Jpeg);
					bmp.Dispose();
					i++;
				}
			}
			StandbyFramesList.Add(new StandbyFrames(guid, imagesBufferSizeAfterEvent));
		}

		public void StartVideo()
		{
			InitializeCameraFramesWatcher();
			MjpegCamera.FrameReady += BmpToCameraFramesWatcher;
			VideoThread = new Thread(MjpegCamera.StartVideo);
			VideoThread.Start();
		}
		public void StopVideo()
		{
			MjpegCamera.FrameReady -= BmpToCameraFramesWatcher;
			MjpegCamera.StopVideo();
		}
	}

	public class StandbyFrames
	{
		public Guid Guid { get; private set; }
		public int FramesCount { get; private set; }
		public int Index { get; set; }
		public StandbyFrames(Guid guid, int framesCount)
		{
			Guid = guid;
			FramesCount = framesCount;
			Index = 0;
		}
	}
}