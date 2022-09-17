package main

import (
	"context"
	"crypto/rand"
	"fmt"
	"github.com/docker/docker/api/types"
	"github.com/docker/docker/api/types/container"
	"github.com/docker/docker/client"

	"github.com/docker/go-connections/nat"
	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
	"io"
	"net/http"
	"strconv"
)

type MatchResponse struct {
	Port    int    `json:"port"`
	MatchID string `json:"match_id"`
}

type JoinMatchRequest struct {
	MatchID string `json:"match_id"`
}

type MatchOverRequest struct {
	MatchID string `json:"match_id"`
}

type Match struct {
	Port        int      `json:"port"`
	MatchID     string   `json:"match_id"`
	Players     []Player `json:"players"`
	ContainerID string   `json:"container_id"`
	IsPublic    bool     `json:"is_public"`
}

type Player struct {
	Score   int    `json:"score"`
	MatchID string `json:"match_id"`
	Name    string `json:"name"`
}

var (
	Matches        = []Match{}
	DockerCli      *client.Client
	PortIndex      = 7777
	ContainerReady = make(chan Match)
)

func main() {
	var err error
	DockerCli, err = client.NewClientWithOpts(client.FromEnv)
	if err != nil {
		fmt.Println(err)
	}

	app := fiber.New()
	app.Use(cors.New())

	routes := app.Group("/") // TODO log
	{
		client := routes.Group("/") // TODO authorize client
		{
			client.Get("/host_public", HostPublic)
			client.Get("/host_private", HostPrivate)
			client.Get("/search_match", SearchMatch)
			client.Post("/join_match", JoinMatch)
		}

		game := routes.Group("/") // TODO authorize game server
		{
			game.Get("/match_over", MatchOver) // TODO remove container
			game.Get("/ready", Ready)
		}
	}
	err = app.Listen(":3000")
	if err != nil {
		fmt.Println(err)
	}
}

// TODO Start Container && Add Match to Matches && Redirect Player Specific port && IsPublic = true

func HostPublic(ctx *fiber.Ctx) error {
	var match = Match{
		Port:        PortIndex,
		MatchID:     EncodeToString(6),
		Players:     nil,
		ContainerID: "",
		IsPublic:    true,
	}

	resp, err := DockerCli.ContainerCreate(context.Background(), &container.Config{
		ExposedPorts: nat.PortSet{
			"7777": struct{}{},
		},
		Image: "ball2d",
	}, &container.HostConfig{
		PortBindings: nat.PortMap{
			"7777": []nat.PortBinding{
				{
					HostIP:   "0.0.0.0",
					HostPort: strconv.Itoa(PortIndex),
				},
			},
		},
	}, nil, nil, "ball2d"+strconv.Itoa(PortIndex))
	if err != err {
		fmt.Println(err)
	}
	match.ContainerID = resp.ID

	err = DockerCli.ContainerStart(context.Background(), resp.ID, types.ContainerStartOptions{})
	if err != err {
		fmt.Println(err)
	}

	PortIndex++

	Matches = append(Matches, match)

	return ctx.Status(http.StatusOK).JSON(&MatchResponse{
		Port:    match.Port,
		MatchID: match.MatchID,
	})
}

// TODO Start Container && Add Match to Matches && Redirect Player Specific port && IsPublic = false

func HostPrivate(ctx *fiber.Ctx) error {
	resp, err := DockerCli.ContainerCreate(context.Background(), &container.Config{
		ExposedPorts: nat.PortSet{
			"7777": struct{}{},
		},
		Image: "ball2d",
	}, &container.HostConfig{
		PortBindings: nat.PortMap{
			"7777": []nat.PortBinding{
				{
					HostIP:   "0.0.0.0",
					HostPort: strconv.Itoa(PortIndex),
				},
			},
		},
	}, nil, nil, "ball2d"+strconv.Itoa(PortIndex))
	if err != err {
		fmt.Println(err)
	}

	err = DockerCli.ContainerStart(context.Background(), resp.ID, types.ContainerStartOptions{})
	if err != err {
		fmt.Println(err)
	}

	match := <-ContainerReady
	match.Port = PortIndex
	match.ContainerID = resp.ID
	match.IsPublic = false

	PortIndex++

	Matches = append(Matches, match)

	return ctx.Status(http.StatusOK).JSON(&MatchResponse{
		Port:    match.Port,
		MatchID: match.MatchID,
	})
}

// TODO Redirect to Match by MatchID

func JoinMatch(ctx *fiber.Ctx) error {
	var req JoinMatchRequest
	err := ctx.BodyParser(&req)
	if err != nil {
		fmt.Println(err)
	}

	for _, match := range Matches {
		if match.MatchID == req.MatchID {
			return ctx.Status(http.StatusOK).JSON(&MatchResponse{
				Port:    match.Port,
				MatchID: match.MatchID,
			})
		}
	}
	ctx.Status(http.StatusNotFound)
	return nil
}

func Ready(ctx *fiber.Ctx) error {
	fmt.Println("ready")
	var match = Match{
		MatchID: EncodeToString(6),
	}
	ContainerReady <- match
	return ctx.Status(http.StatusOK).JSON(&MatchResponse{
		MatchID: match.MatchID,
	})
}

// TODO Close the Container of the Match

func MatchOver(ctx *fiber.Ctx) error {
	var req MatchOverRequest
	err := ctx.BodyParser(&req)
	if err != nil {
		fmt.Println(err)
	}

	for _, match := range Matches {
		if match.MatchID == req.MatchID {
			err := DockerCli.ContainerRemove(context.Background(), match.ContainerID, types.ContainerRemoveOptions{})
			if err != nil {
				fmt.Println(err)
			}
		}
	}
	ctx.Status(http.StatusOK)
	return nil
}

// TODO Redirect to Random Match if IsPublic == true

func SearchMatch(ctx *fiber.Ctx) error {
	return nil
}

func EncodeToString(max int) string {
	b := make([]byte, max)
	n, err := io.ReadAtLeast(rand.Reader, b, max)
	if n != max {
		panic(err)
	}
	for i := 0; i < len(b); i++ {
		b[i] = table[int(b[i])%len(table)]
	}
	return string(b)
}

var table = [...]byte{'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'}
