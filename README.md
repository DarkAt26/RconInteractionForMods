# RconInteractionForMods

```
mkdir RIFM
```
```
wget -P /root/RIFM -O RconInteractionForMods https://github.com/DarkAt26/RconInteractionForMods/releases/latest/download/RconInteractionForMods.linux-x64
```
```
chmod +x RconInteractionForMods
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
sudo systemctl start httpRcon
```
```
sudo systemctl stop httpRcon
```
```
sudo systemctl restart httpRcon
```
```
sudo systemctl status httpRcon
```
