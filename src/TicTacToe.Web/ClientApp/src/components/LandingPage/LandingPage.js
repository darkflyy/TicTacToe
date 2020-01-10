import React, { useState, useEffect } from "react";
import { Grid, Card, makeStyles } from "@material-ui/core";
import MaterialTable from "material-table";
import axios from "axios";
import { useHistory } from "react-router-dom";
import * as signalR from "@microsoft/signalr";
import Axios from "axios";
import { toast } from "react-toastify";

const useStyles = makeStyles(theme => ({
  card: {
    margin: theme.spacing(2)
  }
}));

export default function LandingPage(props) {
  const classes = useStyles();
  let history = useHistory();

  const [rooms, setRooms] = useState([]);
  const [channel, setChannel] = useState(null);

  useEffect(() => {
    if (channel !== null) {
      channel
        .start()
        .then(() => console.log("Connection started!"))
        .catch(err => {
          //do nothing
        });
      channel.on("created", data => {
        let roomList = [...rooms];
        roomList.push({
          roomId: data.roomId,
          numberOfPlayers: 0
        });
        setRooms(roomList);
      });
      channel.on("joined", data => {
        let index = rooms.findIndex(r => r.roomId === data.roomId);
        if (rooms.length !== 0 && index !== -1) {
          let roomList = [...rooms];
          roomList[index].numberOfPlayers = data.numberOfPlayers;
          setRooms(roomList);
        }
      });
      channel.on("left", data => {
        let index = rooms.findIndex(r => r.roomId === data.roomId);
        if (rooms.length !== 0 && index !== -1) {
          let roomList = [...rooms];
          if (data.numberOfPlayers <= 0) {
            roomList.splice(index, 1);
          } else {
            roomList[index].numberOfPlayers = data.numberOfPlayers;
          }
          setRooms(roomList);
        }
      });
    }
  }, [channel, rooms]);

  useEffect(() => {
    axios.get(process.env.REACT_APP_API_URL + "api/PlayingRooms/GetAll").then(resp => {
      if (resp.status === 200) {
        setRooms(resp.data);
      }
    });
    const hub = new signalR.HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_API_URL + "hubs/PlayingRoomList")
      .configureLogging(signalR.LogLevel.Error)
      .build();
    setChannel(hub);
    return function cleanup() {
      if (channel !== null) {
        channel.stop();
      }
    };
  }, []);

  return (
    <Grid xs="12" sm="12" md="12" xl="12">
      <Card className={classes.card}>
        <MaterialTable
          title="Room list"
          columns={[
            { title: "Room ID", field: "roomId" },
            { title: "Players", field: "numberOfPlayers" }
          ]}
          data={rooms}
          actions={[
            {
              icon: "add",
              tooltip: "Create room",
              isFreeAction: true,
              onClick: event => {
                Axios.get(process.env.REACT_APP_API_URL + "api/PlayingRooms/Create")
                  .then(resp => {
                    if (resp.status === 200) {
                      if (channel !== null) {
                        channel.stop();
                      }
                      history.push(`/room/${resp.data.roomId}`);
                    }
                  })
                  .catch(err => {
                    console.log(err.response);
                  });
              }
            },
            {
              icon: "play_circle_outlined",
              tooltip: "Join room",
              isFreeAction: false,
              onClick: (event, rowData) => {
                if (rowData.numberOfPlayers < 2) {
                  if (channel !== null) {
                    channel.stop();
                  }
                  history.push(`/room/${rowData.roomId}`);
                } else {
                  toast.error("Room is already full.");
                }
              }
            }
          ]}
        />
      </Card>
    </Grid>
  );
}
