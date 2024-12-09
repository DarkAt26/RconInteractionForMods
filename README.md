# RconInteractionForMods

```
[Unit]
Description=httpRcon

[Service]
Type=simple
WorkingDirectory=/root
ExecStart=/root/RconInteractionForMods

RestartSec=1
Restart=always
User=root

[Install]
WantedBy = multi-user.target
```
