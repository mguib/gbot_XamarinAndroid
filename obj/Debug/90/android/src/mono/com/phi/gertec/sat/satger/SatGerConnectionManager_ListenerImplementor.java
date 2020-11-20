package mono.com.phi.gertec.sat.satger;


public class SatGerConnectionManager_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.phi.gertec.sat.satger.SatGerConnectionManager.Listener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_startIoManager:()V:GetStartIoManagerHandler:Com.Phi.Gertec.Sat.Satger.SatGerConnectionManager/IListenerInvoker, SatLib\n" +
			"";
		mono.android.Runtime.register ("Com.Phi.Gertec.Sat.Satger.SatGerConnectionManager+IListenerImplementor, SatLib", SatGerConnectionManager_ListenerImplementor.class, __md_methods);
	}


	public SatGerConnectionManager_ListenerImplementor ()
	{
		super ();
		if (getClass () == SatGerConnectionManager_ListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Phi.Gertec.Sat.Satger.SatGerConnectionManager+IListenerImplementor, SatLib", "", this, new java.lang.Object[] {  });
	}


	public void startIoManager ()
	{
		n_startIoManager ();
	}

	private native void n_startIoManager ();

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
