import React, { useState, useEffect, useRef } from "react";
import { useHistory } from "react-router-dom";
import { Grid, Button, makeStyles, Typography } from "@material-ui/core";
import * as signalR from "@microsoft/signalr";
import RoomExpiredModal from "../RoomExpiredModal/RoomExpiredModal";
import color from "@material-ui/core/colors/amber";

const useStyles = makeStyles(theme => ({
  button: {
    minHeight: "80px",
    minWidth: "80px",
    fontSize: "30px",
    borderRadius: 0
  },
  turn: {
    marginBottom: "80px"
  },
  score: {
    marginTop: "80px"
  }
}));

export default function Board(props) {
  const classes = useStyles();
  const history = useHistory();
  const [identity, setIdentity] = useState("");
  const [gameState, setGameState] = useState([
    ["●", "●", "●"],
    ["●", "●", "●"],
    ["●", "●", "●"]
  ]);
  const [turn, setTurn] = useState(false);
  const [myScore, setMyScore] = useState(0);
  const [enemyScore, setEnemyScore] = useState(0);
  const [channel, setChannel] = useState(null);
  const [waiting, setWaiting] = useState(true);
  const [expired, setExpired] = useState(false);

  useEffect(() => {
    if (channel !== null) {
      channel
        .start()
        .then(() => console.log("Connection started!"))
        .catch(err => {
          //do nothing
        });
      channel.on("joined", data => {
        console.log("joined", data);
        updateGameState(data);
      });
      channel.on("left", data => {
        if (data.moveState === "waiting") {
          setWaiting(true);
        } else if (data.moveState === "expired") {
          setWaiting(false);
          setExpired(true);
        }
      });
      channel.on("identity", data => {
        setIdentity(data);
      });
      channel.on("win", data => {
        updateGameState(data);
      });
      channel.on("tie", data => {
        updateGameState(data);
      });
      channel.on("move", data => {
        updateGameState(data);
      });
    }
  }, [channel, identity, waiting, expired]);

  useEffect(() => {
    const hub = new signalR.HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_API_URL + "hubs/PlayingRoom", {
        accessTokenFactory: () => {
          return props.id;
        }
      })
      .configureLogging(signalR.LogLevel.Error)
      .build();
    setChannel(hub);
    return function cleanup() {
      try {
        channel.stop();
      } catch (err) {
        //do nothing
      }
    };
  }, []);

  const updateGameState = data => {
    try {
      setTurn(data.turn);
      console.log("update", data.moveState);
      if (data.moveState === "waiting") {
        setWaiting(true);
      } else {
        setWaiting(false);
      }
      setGameState(data.gameState);
      if (identity === "X") {
        setMyScore(data.xScore);
        setEnemyScore(data.oScore);
      } else if (identity === "O") {
        setMyScore(data.oScore);
        setEnemyScore(data.xScore);
      }
    } catch (err) {
      console.log(err);
    }
  };

  const performMove = (positionY, positionX) => {
    let state = [...gameState];
    if (turn === identity && state[positionY][positionX] === "●" && !waiting) {
      state[positionY][positionX] = identity;
      setTurn(false);
      setGameState(state);
      let data = {
        roomId: props.id,
        gameState: state,
        identity: identity
      };
      channel.invoke("PerformMove", data).catch(err => {
        console.log(err);
      });
    } else {
      //do nothing
    }
  };

  return (
    <Grid>
      <RoomExpiredModal open={expired} channel={channel} />
      <Grid className={classes.turn}>
        {waiting ? (
          <Typography variant="h6">Waiting for 2nd player to join...</Typography>
        ) : turn === identity ? (
          <Typography style={{ color: "#0c9463" }} variant="h6">
            Your turn!
          </Typography>
        ) : identity === "X" ? (
          <Typography style={{ color: "#fa163f" }} variant="h6">
            Player O's turn!
          </Typography>
        ) : (
          <Typography style={{ color: "#fa163f" }} variant="h6">
            Player X's turn!
          </Typography>
        )}
        <Typography variant="h6">You are: {identity === "" ? "Spectator" : identity}</Typography>
      </Grid>
      <Grid>
        <Button className={classes.button} id="0,0" variant="outlined" onClick={() => performMove(0, 0)}>
          {gameState[0][0]}
        </Button>
        <Button className={classes.button} id="0,1" variant="outlined" onClick={() => performMove(0, 1)}>
          {gameState[0][1]}
        </Button>
        <Button className={classes.button} id="0,2" variant="outlined" onClick={() => performMove(0, 2)}>
          {gameState[0][2]}
        </Button>
      </Grid>
      <Grid>
        <Button className={classes.button} id="1,0" variant="outlined" onClick={() => performMove(1, 0)}>
          {gameState[1][0]}
        </Button>
        <Button className={classes.button} id="1,1" variant="outlined" onClick={() => performMove(1, 1)}>
          {gameState[1][1]}
        </Button>
        <Button className={classes.button} id="1,2" variant="outlined" onClick={() => performMove(1, 2)}>
          {gameState[1][2]}
        </Button>
      </Grid>
      <Grid>
        <Button className={classes.button} id="2,0" variant="outlined" onClick={() => performMove(2, 0)}>
          {gameState[2][0]}
        </Button>
        <Button className={classes.button} id="2,1" variant="outlined" onClick={() => performMove(2, 1)}>
          {gameState[2][1]}
        </Button>
        <Button className={classes.button} id="2,2" variant="outlined" onClick={() => performMove(2, 2)}>
          {gameState[2][2]}
        </Button>
      </Grid>
      <Grid>
        <Typography className={classes.score} variant="h6">
          Score:
        </Typography>
        <Typography variant="h6">
          {identity === "" ? "Player X" : "You"}: {myScore}
        </Typography>
        <Typography variant="h6">
          {identity === "O" ? "Player X" : "Player O"}: {enemyScore}
        </Typography>
        <Button
          style={{ marginTop: "10px" }}
          color="primary"
          variant="outlined"
          onClick={() => {
            channel.stop();
            history.push("/");
          }}
        >
          Leave room
        </Button>
      </Grid>
    </Grid>
  );
}
