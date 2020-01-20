# Remote controlled electronics
Remote control any device that is programmable and supports HTTP connections. PC, laptop, console, mobile, TV and more. Here's an idea: remote control a raspberry pi in turn controlling an at home, local Wi-Fi internet of things.

![](/docs/rceserver.png)

# Features
- Servers
	- can run distributed as they don't keep any session/connection/state
	- control interface is web based meaning it can be accessed from anywhere
- Workers
	- are quick and easy to write
	- can be written in anything that supports HTTP connections
	- are minimalistic, they just connect to the server and get/update/complete jobs
	- can be multi or single threaded doing all jobs at once or one at a time

# Worker Loop
Workers are very simple by design, as the whole idea is to allow adding a remote control capability quickly and easily to any device. Below are loops for single and multi threaded workers. Blue boxes represent http requests.

![](/docs/rceworkerflow.png)

# Worker Requests
Example worker requests in postman: [Postman Collection](/src/Workers/Postman/postman-worker.json "Postman Collection")

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