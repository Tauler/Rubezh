﻿using FiresecAPI.Models;

namespace ServerFS2.Operations
{
	public static class TouchMemoryOperationHelper
	{
		public static void Operation(Device device, int operationNo)
		{
			USBManager.Send(device, 0x33, operationNo);
		}
	}
}
