# Next version available
--- Next version at: [Remote Controlled Electronics v2](https://github.com/pre-alpha-final/rce2 "Remote Controlled Electronics v2") ---  

# Remote controlled electronics
Remote control any device that is programmable and supports HTTP connections. PC, laptop, console, mobile, TV etc. Here's an idea: remote control a raspberry pi in turn controlling an at home, local Wi-Fi internet of things.  

![](/docs/rceserver.png)

# Features
- Servers
	- can run distributed as they don't keep any session/connection/state
	- control interface is web based meaning it can be accessed from anywhere
- Workers
	- are quick and easy to write
	- can be written in any language that supports HTTP connections
	- are minimalistic, they just connect to the server and get/update/complete jobs
	- jobs can be run sequentially or in parallel

# Architecture
There is no direct connection between server and workers. Server pushes new jobs to the cloud and workers pull them from the cloud. Also no connection is made to the workers at any point. All worker communication is done with http requests which they themselves initiate. This means that as long as the worker has internet access no NAT or firewall will interfere with the communication.

![](/docs/rcearch.png)

# Worker Loop
Workers are very simple by design, as the whole idea is to allow adding a remote control capability quickly and easily to any device. Below are loops for sequential and parallel flows. Blue boxes represent http requests.

![](/docs/rceworkerflow.png)

# Worker Requests
Example worker requests in postman: [Postman Collection](/src/Workers/Postman/postman-worker.json "Postman Collection")

# Worker Examples
There are a few worker examples included with the project:
- Postman - postman collection with all requests required to simulate a worker
- RceSharpLib - c# library for workers
- CSharpParallel - c# example showing the parallel flow (using RceSharpLib)
- CSharpSequential - c# example showing the sequential flow (using RceSharpLib)
- JS - javascript easily embeddable in any webpage as iife
- PowerShellHappyPath - simplified example in PowerShell without error checks and retries
- BashHappyPath - (in development) simplified example in bash without error checks and retries
- XamarinForms - (in development) Xamarin Forms android app example
- CSharpMono - (in development) C# app runnable with mono
- RpzwTimer - Raspberry Pi Zero W Timer
- RpzwNeopixels - Raspberry Pi Zero W Neopixel demo

# Demo environment
For testing purposes log in here:  
**https://rceserver.azurewebsites.net**  
with these credentials:  
User: **demo@example.com**  
Password: **demo**  

Worker examples included in the project target this environment and user by default

# Development
Settings needed for development, mirrored in secrets.json
```
{
  "ConnectionStrings":{
    "SignalR":"",
    "UsersDbContext":"",
	"RceMessagesDb": ""
  },
  "SendGridApiKey":"",
  "ClientSecret":"",
  "Authority":"",
  "Domain":"",
  "IdentityJwk":"",
  "InstrumentationKey":""
}
```

# Registration
Registration is commented out by default in the controller.

License
----
MIT