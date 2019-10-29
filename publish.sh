#!/bin/bash

dotnet build
dotnet publish -r linux-arm
rsync -u -v -t /mnt/c/src/other/network-monitor/NetworkMonitor.WorkerService/bin/Debug/netcoreapp3.0/linux-arm/publish/* pi@raspberrypi:/home/pi/networkmonitor/
ssh -t pi@raspberrypi 'sudo systemctl restart networkmonitor.service;sudo systemctl status networkmonitor.service'