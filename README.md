# RconInteractionForMods

# GUIDE NOT READY YET, WILL BE COMPLETED IN THE NEXT COUPLE DAYS 

```
mkdir RIFM
```
```
cd RIFM
```
```
wget -O RconInteractionForMods https://github.com/DarkAt26/RconInteractionForMods/releases/latest/download/RconInteractionForMods.linux-x64
```
```
chmod +x RconInteractionForMods
```

```
./RconInteractionForMods
```
```
nano rifm_config.json
```
```
nano rifm_cmd_config.json
```
```
./RconInteractionForMods
```

```
cd
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
