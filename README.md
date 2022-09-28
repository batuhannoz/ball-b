# BallB

2D online soccer game

![](https://github.com/batuhannoz/ball-b/blob/main/pictures/GameScreenshot.png)

### Unity Documentation

[Netcode Documentation](https://docs-multiplayer.unity3d.com/netcode/current/about)

[Netcode Multiplayer](https://www.youtube.com/watch?v=stJ4SESQwJQ&t=130s)

### YouTube Tutorials

[Netcode Multiplayer](https://www.youtube.com/watch?v=stJ4SESQwJQ&t=130s)

[Global Matchmaking](https://www.youtube.com/watch?v=fdkvm21Y0xE&t=591s)

[Server Reconciliation && Client Prediction](https://www.youtube.com/watch?v=TFLD9HWOc2k)


#### Open Docker Permissions To All Users

```bash
sudo chmod 666 /var/run/docker.sock
```

#### Build Game Server

/builds/linux_server
```bash
docker build -t ball2d .
```

#### Build Backend

/backend
```bash
docker build -t backend .
```

#### Run Backend
```bash
docker run -d -p 3000:3000 backend
```

#### To Delete Running Containers
```bash
docker rm -f $(docker ps -a -q)
```

#### Send Game Files To Server
```bash
put -R /home/batuhan/Desktop/ball-b/builds/linux_server .
```

