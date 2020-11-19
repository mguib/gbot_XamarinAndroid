package crc644aaa2ebaabd2d6a5;


public class Tef
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Gbot_XamarinAndroid.TEF.Tef, Gbot_XamarinAndroid", Tef.class, __md_methods);
	}


	public Tef ()
	{
		super ();
		if (getClass () == Tef.class)
			mono.android.TypeManager.Activate ("Gbot_XamarinAndroid.TEF.Tef, Gbot_XamarinAndroid", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
