Replace 
	IPHostEntry ipHostInfo = Dns.GetHostEntry("studenthostsvr");

with
	IPHostEntry ipHostInfo = Dns.GetHostEntry("127.0.0.1");

when running the server locally.

Locations:
ClientForm.cs: Line 113
LoginForm.cs: Line 33
NewAccountForm.cs: Line 32