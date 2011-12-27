using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly : AssemblyTitle( "MessageBroker" )]
[assembly : AssemblyDescriptionAttribute("Message broker extension for Ninject")]
[assembly : AssemblyConfiguration( "" )]
[assembly : AssemblyTrademark( "" )]
[assembly : AssemblyCulture( "" )]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly : Guid( "3c4be3cc-64e0-43a6-ab57-4fe8675962be" )]

#if !NO_PARTIAL_TRUST
[assembly: AllowPartiallyTrustedCallers]
#endif