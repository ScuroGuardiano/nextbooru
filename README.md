# ULTRA HORNY BOARD
Welcome to my super duper image board written in ASP.NET, Entity Framework and Angular!

## How to run this thingy
First make sure you have docker and docker compose installed. Then follow:
```sh
git clone https://github.com/scuroguardiano/ultra-horny-board.git
cd ultra-horny-board
# Postgres setup
cd pg-dev
sudo docker compose up -d
cd ..
# Update database
dotnet tool install --global dotnet-ef # if you don't have it
dotnet ef database update
# Project
dotnet run
```

> Note: You need aspnet runtime as well, on Arch based distros install with `sudo pacman -S aspnet-runtime`

Along with database you will have access to Adminer on port 9679.  
Edit `pg-dev/docker-compose.yml` to change ports.

If you want to use your own postgres you need to change database config in `appsettings.json` and `appsettings.Development.json`

## LICENSE
All files in this repository are licenced under GNU AFFERO GENERAL PUBLIC LICENSE version 3  
Full license text is available in `LICENSE` file.