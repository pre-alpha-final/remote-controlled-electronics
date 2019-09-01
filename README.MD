# Remote controlled electronics
A new spin on a standard "command and control" scheme. A web hosted (azure) command server controlling net core workers on raspberry pi in turn controlling whatever (an at home, local wi-fi IOT for instance).

# Development
Settings needed for development, mirrored in secrets.json
```
{  
  "ConnectionStrings":{
    "UsersDbContext":""
  },
  "SendGridApiKey":"",
  "ClientSecret":"",
  "Authority":"",
  "Domain":"",
  "IdentityJwk":""
}
```

# Registration
Registration is commented out by default in the controller.

License
----
MIT