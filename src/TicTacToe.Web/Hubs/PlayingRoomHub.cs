using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicTacToe.Infrastructure.Commands;
using TicTacToe.Infrastructure.Commands.PlayingRoom;

namespace TicTacToe.Web.Hubs
{
    public class PlayingRoomHub : HubBase
    {
        private readonly PlayingRoomListHub _roomListHub;

        public PlayingRoomHub(PlayingRoomListHub roomListHub, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            _roomListHub = roomListHub;
        }

        public async Task SendToGroupAsync(string groupName, string method, object data)
        {
            await Clients.Group(groupName).SendAsync(method, data);
        }
        public async Task Subscribe(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task Unsubscribe(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task PerformMove(Move command)
        {
            var context = Context.GetHttpContext();
            var id = context.Request.Query["access_token"];

            await DispatchAsync(command);

            if (command.Result.MoveState == "continue")
            {
                await SendToGroupAsync(id, "move", command.Result);
            }
            else if (command.Result.MoveState == "win")
            {
                await SendToGroupAsync(id, "win", command.Result);
            }
            else if (command.Result.MoveState == "tie")
            {
                await SendToGroupAsync(id, "tie", command.Result);
            }
            else if (command.Result.MoveState == "waiting")
            {
                await SendToGroupAsync(id, "waiting", command.Result);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            var id = context.Request.Query["access_token"];
            await Subscribe(id);

            var command = new JoinRoom();
            command.RoomId = Guid.Parse(id);
            command.ConnectionId = Context.ConnectionId;
            await DispatchAsync(command);

            await _roomListHub.SendToAllAsync("joined", new { roomId = command.Result.RoomId, numberOfPlayers = command.Result.NumberOfPlayers });
            await SendToGroupAsync(id, "joined", command.Result);

            if (command.Result.Players.Exists(x => x.ConnectionId == Context.ConnectionId))
            {
                await Clients.Caller.SendAsync("identity", command.Result.Players.Find(x => x.ConnectionId == Context.ConnectionId).Identity);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var context = Context.GetHttpContext();
            var id = context.Request.Query["access_token"];
            await Unsubscribe(id);

            var command = new LeaveRoom();
            command.RoomId = Guid.Parse(id);
            command.ConnectionId = Context.ConnectionId;
            await DispatchAsync(command);

            await _roomListHub.SendToAllAsync("left", new { roomId = command.Result.RoomId, numberOfPlayers = command.Result.NumberOfPlayers });
            await SendToGroupAsync(id, "left", command.Result);
        }
    }
}