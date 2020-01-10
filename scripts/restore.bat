echo Restoring TicTacToe.Core:
cd ../src/TicTacToe.Core/
dotnet restore
echo Restoring TicTacToe.Infrastructure:
cd ../TicTacToe.Infrastructure/
dotnet restore
echo Restoring TicTacToe.Web:
cd ../TicTacToe.Web/
dotnet restore
echo Restoring TicTacToe.Web.ClientApp:
cd ClientApp
setlocal
call npm.cmd install
endlocal
cd ../../../scripts
