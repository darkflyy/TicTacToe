import React from "react";
import Layout from "../components/Layout/Layout";
import Board from "../components/Board/Board";

export default function PlayingRoom(props) {
  return (
    <Layout>
      <Board id={props.match.params.id} />
    </Layout>
  );
}
