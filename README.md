# RconInteractionForMods

```
mkdir RIFM
```
```
wget -P /root/RIFM -O RconInteractionForMods https://github.com/DarkAt26/RconInteractionForMods/releases/download/V1.0/RconInteractionForMods.linux-x64
```
```
chmod +x RconInteractionForMods
```

```
./ RconInteractionForMods
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
```
sudo systemctl start RconInteractionForMods
```
```
sudo systemctl stop RconInteractionForMods
```
```
sudo systemctl restart RconInteractionForMods
```
```
sudo systemctl status RconInteractionForMods
```
