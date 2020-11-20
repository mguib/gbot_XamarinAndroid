package crc64ae865b010eb896ee;


public class InterfaceData
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.phi.gertec.sat.satger.SatGerLib.OnDataReceived
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onError:(Ljava/lang/Exception;)V:GetOnError_Ljava_lang_Exception_Handler:Com.Phi.Gertec.Sat.Satger.SatGerLib/IOnDataReceivedInvoker, SatLib\n" +
			"n_onReceivedDataUpdate:(Ljava/lang/String;)V:GetOnReceivedDataUpdate_Ljava_lang_String_Handler:Com.Phi.Gertec.Sat.Satger.SatGerLib/IOnDataReceivedInvoker, SatLib\n" +
			"";
		mono.android.Runtime.register ("Gbot_XamarinAndroid.SAT.ServiceSAT.InterfaceData, Gbot_XamarinAndroid", InterfaceData.class, __md_methods);
	}


	public InterfaceData ()
	{
		super ();
		if (getClass () == InterfaceData.class)
			mono.android.TypeManager.Activate ("Gbot_XamarinAndroid.SAT.ServiceSAT.InterfaceData, Gbot_XamarinAndroid", "", this, new java.lang.Object[] {  });
	}


	public void onError (java.lang.Exception p0)
	{
		n_onError (p0);
	}

	private native void n_onError (java.lang.Exception p0);


	public void onReceivedDataUpdate (java.lang.String p0)
	{
		n_onReceivedDataUpdate (p0);
	}

	private native void n_onReceivedDataUpdate (java.lang.String p0);

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
