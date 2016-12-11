package md5926d11a07a422d062f782b4753c20148;


public class Runnable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("SilexSample.Runnable, SilexSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Runnable.class, __md_methods);
	}


	public Runnable () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Runnable.class)
			mono.android.TypeManager.Activate ("SilexSample.Runnable, SilexSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
