# RconInteractionForMods

# GUIDE NOT READY YET, WILL BE COMPLETED IN THE NEXT COUPLE DAYS 
https://mod.io/g/pavlov/m/rcon-interaction-for-mods
# #1
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
![grafik](https://github.com/user-attachments/assets/670217dc-971d-4e11-b69a-86f2ffad002a)

```
nano rifm_config.json
```
```
nano rifm_cmd_config.json
```
```
./RconInteractionForMods
```
![grafik](https://github.com/user-attachments/assets/1820876a-332b-480e-910d-1da0c61556aa)

```
cd
```

```
sudo ufw status
sudo ufw allow 8000(the port you chose)
sudo ufw status
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

# #2
