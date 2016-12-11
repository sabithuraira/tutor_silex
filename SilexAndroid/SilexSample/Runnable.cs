using System;

namespace SilexSample
{
	class Runnable : Java.Lang.Object, Java.Lang.IRunnable
	{
		Action run;
		public Runnable(Action run)
		{
			this.run = run;
		}

		public void Run()
		{
			run();
		}
	}
}