package crc64787ad53d6e92e92d;


public class Associar
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
		mono.android.Runtime.register ("Gbot_XamarinAndroid.SAT.SatPages.Associar, Gbot_XamarinAndroid", Associar.class, __md_methods);
	}


	public Associar ()
	{
		super ();
		if (getClass () == Associar.class)
			mono.android.TypeManager.Activate ("Gbot_XamarinAndroid.SAT.SatPages.Associar, Gbot_XamarinAndroid", "", this, new java.lang.Object[] {  });
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
