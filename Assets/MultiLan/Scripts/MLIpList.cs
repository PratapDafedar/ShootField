using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace MultiPlayer {
	class MLIpList : MonoBehaviour {		
		
	    [StructLayout(LayoutKind.Sequential)]
	    public struct _SERVER_INFO_100  {
	        internal int sv100_platform_id;
	        [MarshalAs(UnmanagedType.LPWStr)]
	        internal string sv100_name;
	    }
	
	    [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
	    public static extern int NetApiBufferFree(IntPtr pBuf);
	
	    [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
	    public static extern int NetServerEnum(
	        string ServerNane,
	        int dwLevel,
	        ref IntPtr pBuf,
	        int dwPrefMaxLen,
	        out int dwEntriesRead,
	        out int dwTotalEntries,
	        int dwServerType,
	        string domain,
	        out int dwResumeHandle
	        );
	
	    const int LVL_100 = 100;
	    const int MAX_PREFERRED_LENGTH = -1;
	    const int SV_TYPE_WORKSTATION = 1;
	    const int SV_TYPE_SERVER = 2;
	
		
		/* This functions returns a string array which contains the IP of all active computer of the network
		 * (It include our own IP too)*/
	    public static List<String> GetComputers()  {
			
	        List<string> computers = new List<string>();
	        IntPtr buffer = IntPtr.Zero, tmpBuffer = IntPtr.Zero;
	        int entriesRead, totalEntries, resHandle;
	        int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));
	
	        try {
	            int ret = NetServerEnum(null, LVL_100, ref buffer, MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out resHandle);
	            // If the NetServerEnum() function success :
				if (ret == 0) {
	                for (int i = 0; i < totalEntries; i++)
	                {
	                    tmpBuffer = new IntPtr((int)buffer + (i * sizeofINFO));
	
	                    _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));
						// Add each IP on the computers list
						computers.Add(Dns.GetHostAddresses(svrInfo.sv100_name)[0].ToString());
	                } 
	            } else { // Else : throw the Windows System error generated			
					throw new Win32Exception(ret);					
				}
	        } finally {					
	            NetApiBufferFree(buffer);
	        }
			// Transform the ArrayList on a string array and return it
	        return computers;
	    }	
	
	}

}	 
