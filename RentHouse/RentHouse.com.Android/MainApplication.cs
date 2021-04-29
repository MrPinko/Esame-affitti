﻿#if DEBUG
using System;
using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;

[Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
[MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyAnXdxwBRXLMregctVbDqIp3pyljS6aybA")]
public class MainApplication : Application
{
	public MainApplication(IntPtr handle, JniHandleOwnership transer)
	  : base(handle, transer)
	{
	}

	public override void OnCreate()
	{
		base.OnCreate();
		CrossCurrentActivity.Current.Init(this);
	}
}