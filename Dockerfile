FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS builder
WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_12.x |  bash -
RUN apt-get install -y nodejs

COPY ./ ./
RUN dotnet publish "./src/TicTacToe.Web/TicTacToe.Web.csproj" --output "./dist" --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=builder /src/dist/ .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "TicTacToe.Web.dll"]