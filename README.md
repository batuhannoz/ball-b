# BallB

2D online soccer game


#### Build Game Server

/builds/linux
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

