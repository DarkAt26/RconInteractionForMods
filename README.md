# RconInteractionForMods

```
mkdir RIFM
```
```
sudo nano /etc/systemd/system/RconInteractionForMods.service
```
```
[Unit]
Description=RconInteractionForMods

[Service]
Type=simple
WorkingDirectory=/root/RIFM
ExecStart=/root/RIFM/RconInteractionForMods

RestartSec=1
Restart=always
User=root

[Install]
WantedBy = multi-user.target
```
