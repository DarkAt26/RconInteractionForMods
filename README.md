# RconInteractionForMods

guide is basically just a step by step copy paste<br>
https://mod.io/g/pavlov/m/rcon-interaction-for-mods
# #1
you need full server/file access to install the code which lets the mod use the rcon commands<br>
the guide uses code which i created for which i dont take any responsibility if it causes you any inconveniences or damages<br>
you obviously can create your own code for that if wanted. look at the code i posted to basically see what it needs to work<br>

As root:

//create a folder named "RIFM"
```
mkdir RIFM
```

//go into the folder RIFM
```
cd RIFM
```
//download the compiled code (latest version) from my github, copied command is for linux-x64 servers, use another version if needed<br>
//(stop the service, rerun this command, start the service again to update the code)
```
wget -O RconInteractionForMods https://github.com/DarkAt26/RconInteractionForMods/releases/latest/download/RconInteractionForMods.linux-x64
```
//make the downloaded code an executeable
```
chmod +x RconInteractionForMods
```


//manually execute the code to let it create the two config files
```
./RconInteractionForMods
```
![grafik](https://github.com/user-attachments/assets/670217dc-971d-4e11-b69a-86f2ffad002a)
//edit the normal config file. keys must not contain '"'<br>
//viewkey is used to read some of the data such as executed commands,... example: http://000.000.000.000:0000/rifm?viewkey=YOURVIEWKEY <br>
//set accept non local requests to false if the code is not executed on the same machine as your pavlov server
```
nano rifm_config.json
```
//edit command config. there you can control which ugc can execute which rcon commands. only allow the commands if you trust that mod(maker) and only allow the commands actually needed
```
nano rifm_cmd_config.json
```
//manually execute the code again to make sure it loads the config data correctly and executes properly
```
./RconInteractionForMods
```
![grafik](https://github.com/user-attachments/assets/1820876a-332b-480e-910d-1da0c61556aa)

//leave the folder RIFM
```
cd
```

//open the HttpRequest_Port to allow the http requests to pass through
```
sudo ufw status
sudo ufw allow 8000(the port you chose)
sudo ufw status
```

//create a service to execute the code needed for the mod
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
//start the service
```
sudo systemctl start RconInteractionForMods
```
//stop the service
```
sudo systemctl stop RconInteractionForMods
```
//restart the service
```
sudo systemctl restart RconInteractionForMods
```
//get status of service
```
sudo systemctl status RconInteractionForMods
```
//to read the logs live
```
sudo journalctl -u RconInteractionForMods -f
```
//to read the full logs since creation
```
sudo journalctl -u RconInteractionForMods
```
# #2
the mod setup in rcon:<br>

//run the rcon command to add the rcon interactions for mods mod to your server
```
UGCAddMod UGC4000363
```
//rotate the map (wait a moment for the map to rotate)
```
RotateMap
```
//this will break other mods currently loaded so dont run this on an server with people playing
```
RIFM
```
//set the auth key for the mod
```
RIFM_AuthKey <your HttpRequest_AuthKey>
```
//set the ip 
```
RIFM_Ip <your HttpRequest_Ip>
```
//set the port
```
RIFM_Port <your HttpRequest_Port>
```
//rotate the map (wait a moment for the map to rotate)
```
RotateMap
```
//optionally run this command to remove the mod again.(so its not loaded when not in active use) this mod needs only be loaded on the server to be able to change the authkey,ip and port. when another mod uses this mod it should have it as a dependency.
```
UGCRemoveMod UGC4000363
```
