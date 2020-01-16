# Remote controlled electronics
Remote control any device that is programmable and supports HTTP connections. PC, laptop, console, mobile, TV and more. Here's an idea: remote control a raspberry pi in turn controlling an at home, local Wi-Fi internet of things.

<image>

# Main Characteristics
- Servers can run distributed as they don't keep any session/connection/state
- Control interface is web based meaning it can be accessed from anywhere
- Workers can be written in anything that supports HTTP connections
- Workers are minimalistic, they just connect to the server and get/update/complete jobs
- Workers can be multi or single threaded doing all jobs at once or one at a time

# Worker Loop
<text>
<image>

# Development
Settings needed for development, mirrored in secrets.json
```
{
  "ConnectionStrings":{
    "SignalR":"",
    "UsersDbContext":""
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