#include <winreg.h>

class REGISTRY
{
public:
   HKEY key;
   char Data[512];


   bool OpenKey( HKEY baseKey, char *KeyName )
   {
      if( RegOpenKeyEx( baseKey, KeyName, NULL, KEY_QUERY_VALUE | KEY_ENUMERATE_SUB_KEYS, &key ) == ERROR_SUCCESS )
	  	  return(true);
	  else
		  return(false);
   }

   char * GetKeyVal( char* ValueName )
   {
	   if( key )
	   {
	      int Len = 512;
          RegQueryValueEx( key, ValueName, NULL, NULL, (LPBYTE)&Data, (LPDWORD)&Len);
	      return(Data);
	   }
	   return(NULL);
   }
   void CloseKey()
   {
      if( key )
      RegCloseKey( key );
   }

   REGISTRY()
   {
       key = NULL;
   }

};


// NoXBot-GUI.cpp : Defines the entry point for the application.
//

/*
winreg.h

LONG RegCloseKey(
  HKEY hKey   // handle to key to close
);*/


/*
RegOpenKeyEx
The RegOpenKeyEx function opens the specified key. 

LONG RegOpenKeyEx(
  HKEY hKey,         // handle to open key
  LPCTSTR lpSubKey,  // address of name of subkey to open
  DWORD ulOptions,   // reserved
  REGSAM samDesired, // security access mask
  PHKEY phkResult    // address of handle to open key
);
 
Parameters
hKey 
Handle to a currently open key or any of the following predefined reserved handle values: 
HKEY_CLASSES_ROOT
HKEY_CURRENT_CONFIG
HKEY_CURRENT_USER
HKEY_LOCAL_MACHINE
HKEY_USERS
Windows NT: HKEY_PERFORMANCE_DATA 
Windows 95 and Windows 98: HKEY_DYN_DATA 

lpSubKey 
Pointer to a null-terminated string containing the name of the subkey to open. If this parameter is NULL or a pointer to an empty string, the function will open a new handle to the key identified by the hKey parameter. In this case, the function will not close the handles previously opened. 
ulOptions 
Reserved; must be zero. 
samDesired 
Specifies an access mask that describes the desired security access for the new key. This parameter can be a combination of the following values: Value Meaning 
KEY_ALL_ACCESS Combination of KEY_QUERY_VALUE, KEY_ENUMERATE_SUB_KEYS, KEY_NOTIFY, KEY_CREATE_SUB_KEY, KEY_CREATE_LINK, and KEY_SET_VALUE access. 
KEY_CREATE_LINK Permission to create a symbolic link. 
KEY_CREATE_SUB_KEY Permission to create subkeys. 
KEY_ENUMERATE_SUB_KEYS Permission to enumerate subkeys. 
KEY_EXECUTE Permission for read access. 
KEY_NOTIFY Permission for change notification. 
KEY_QUERY_VALUE Permission to query subkey data. 
KEY_READ Combination of KEY_QUERY_VALUE, KEY_ENUMERATE_SUB_KEYS, and KEY_NOTIFY access. 
KEY_SET_VALUE Permission to set subkey data. 
KEY_WRITE Combination of KEY_SET_VALUE and KEY_CREATE_SUB_KEY access. 


phkResult 
Pointer to a variable that receives a handle to the opened key. When you no longer need the returned handle, call the RegCloseKey function to close it. 
Return Values
If the function succeeds, the return value is ERROR_SUCCESS.

If the function fails, the return value is a nonzero error code defined in WINERROR.H. You can use the FormatMessage function with the FORMAT_MESSAGE_FROM_SYSTEM flag to get a generic description of the error.

Remarks
Unlike the RegCreateKeyEx function, the RegOpenKeyEx function does not create the specified key if the key does not exist in the registry. 

QuickInfo
  Windows NT: Requires version 3.1 or later.
  Windows: Requires Windows 95 or later.
  Windows CE: Requires version 1.0 or later.
  Header: Declared in winreg.h.
  Import Library: Use advapi32.lib.
  Unicode: Implemented as Unicode and ANSI versions on Windows NT
*/
/*
RegQueryValueEx
The RegQueryValueEx function retrieves the type and data for a specified value name associated with an open registry key. 

LONG RegQueryValueEx(
  HKEY hKey,           // handle to key to query
  LPTSTR lpValueName,  // address of name of value to query
  LPDWORD lpReserved,  // reserved
  LPDWORD lpType,      // address of buffer for value type
  LPBYTE lpData,       // address of data buffer
  LPDWORD lpcbData     // address of data buffer size
);
 
Parameters
hKey 
Handle to a currently open key or any of the following predefined reserved handle values: 
HKEY_CLASSES_ROOT
HKEY_CURRENT_CONFIG
HKEY_CURRENT_USER
HKEY_LOCAL_MACHINE
HKEY_USERS
Windows NT: HKEY_PERFORMANCE_DATA 
Windows 95 and Windows 98: HKEY_DYN_DATA 

lpValueName 
Pointer to a null-terminated string containing the name of the value to query. 
If lpValueName is NULL or an empty string, "", the function retrieves the type and data for the key's unnamed or default value, if any. 

Windows 95 and Windows 98: Every key has a default value that initially does not contain data. On Windows 95, the default value type is always REG_SZ. On Windows 98, the type of a key's default value is initially REG_SZ, but RegSetValueEx can specify a default value with a different type. 

Windows NT: Keys do not automatically have an unnamed or default value. Unnamed values can be of any type. 

lpReserved 
Reserved; must be NULL. 
lpType 
Pointer to a variable that receives the type of data associated with the specified value. The value returned through this parameter will be one of the following: Value Meaning 
REG_BINARY Binary data in any form. 
REG_DWORD A 32-bit number. 
REG_DWORD_LITTLE_ENDIAN A 32-bit number in little-endian format. This is equivalent to REG_DWORD.
In little-endian format, a multi-byte value is stored in memory from the lowest byte (the "little end") to the highest byte. For example, the value 0x12345678 is stored as (0x78 0x56 0x34 0x12) in little-endian format.

Windows NT, Windows 95, and Windows 98 are designed to run on little-endian computer architectures. A user may connect to computers that have big-endian architectures, such as some UNIX systems. 
 
REG_DWORD_BIG_ENDIAN A 32-bit number in big-endian format. 
In big-endian format, a multi-byte value is stored in memory from the highest byte (the "big end") to the lowest byte. For example, the value 0x12345678 is stored as (0x12 0x34 0x56 0x78) in big-endian format.
 
REG_EXPAND_SZ A null-terminated string that contains unexpanded references to environment variables (for example, "%PATH%"). It will be a Unicode or ANSI string depending on whether you use the Unicode or ANSI functions. To expand the environment variable references, use theExpandEnvironmentStrings function. 
REG_LINK A Unicode symbolic link. 
REG_MULTI_SZ An array of null-terminated strings, terminated by two null characters. 
REG_NONE No defined value type. 
REG_RESOURCE_LIST A device-driver resource list. 
REG_SZ A null-terminated string. It will be a Unicode or ANSI string depending on whether you use the Unicode or ANSI functions. 


The lpType parameter can be NULL if the type is not required. 

lpData 
Pointer to a buffer that receives the value's data. This parameter can be NULL if the data is not required. 
lpcbData 
Pointer to a variable that specifies the size, in bytes, of the buffer pointed to by the lpData parameter. When the function returns, this variable contains the size of the data copied to lpData. 
If the data has the REG_SZ, REG_MULTI_SZ or REG_EXPAND_SZ type, then lpcbData will also include the size of the terminating null character. 

The lpcbData parameter can be NULL only if lpData is NULL. 

If the buffer specified by lpData parameter is not large enough to hold the data, the function returns the value ERROR_MORE_DATA, and stores the required buffer size, in bytes, into the variable pointed to by lpcbData. 

If lpData is NULL, and lpcbData is non-NULL, the function returns ERROR_SUCCESS, and stores the size of the data, in bytes, in the variable pointed to by lpcbData. This lets an application determine the best way to allocate a buffer for the value's data. 

Window NT: If hKey specifies HKEY_PERFORMANCE_DATA and the lpData buffer is too small, RegQueryValueEx returns ERROR_MORE_DATA but lpcbData does not return the required buffer size. This is because the size of the performance data can change from one call to the next. In this case, you must increase the buffer size and call RegQueryValueEx again passing the updated buffer size in the lpcbData parameter. Repeat this until the function succeeds. You need to maintain a separate variable to keep track of the buffer size, because the value returned by lpcbData is unpredictable. 

Return Values
If the function succeeds, the return value is ERROR_SUCCESS.

If the function fails, the return value is a nonzero error code defined in WINERROR.H. You can use the FormatMessage function with the FORMAT_MESSAGE_FROM_SYSTEM flag to get a generic description of the error.

Remarks
The key identified by hKey must have been opened with KEY_QUERY_VALUE access. To open the key, use the RegCreateKeyEx or RegOpenKeyEx function. 

If the value data has the REG_SZ, REG_MULTI_SZ or REG_EXPAND_SZ type, and the ANSI version of this function is used (either by explicitly calling RegQueryValueExA or by not defining UNICODE before including the WINDOWS.H file), this function converts the stored Unicode string to an ANSI string before copying it to the buffer pointed to by lpData. 

Window NT: When calling the RegQueryValueEx function with hKey set to the HKEY_PERFORMANCE_DATA handle and a value string of a specified object, the returned data structure sometimes has unrequested objects. Don't be surprised; this is normal behavior. When calling the RegQueryValueEx function, you should always expect to walk the returned data structure to look for the requested object. 

QuickInfo
  Windows NT: Requires version 3.1 or later.
  Windows: RequE:ires Windows 95 or later.
  Windows C Requires version 1.0 or later.
  Header: Declared in winreg.h.
  Import Library: Use advapi32.lib.
  Unicode: Implemented as Unicode and ANSI versions on Windows NT.

HKEY_LOCAL_MACHINE\SOFTWARE\Westwood\Nox
*/