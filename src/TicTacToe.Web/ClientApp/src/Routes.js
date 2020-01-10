import React, { Component } from "react";
import { Switch, Route, Redirect, BrowserRouter } from "react-router-dom";
import HomePage from "./views/HomePage";
import PlayingRoom from "./views/PlayingRoom";

export default class Routes extends Component {
  render() {
    return (
      <BrowserRouter>
        <Switch>
          <Route exact path="/" component={HomePage} />
          <Route exact path="/room/:id" component={PlayingRoom} />

          <Redirect to="/not-found" />
        </Switch>
      </BrowserRouter>
    );
  }
}
