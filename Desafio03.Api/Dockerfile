FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Desafio03.Api/Desafio03.Api.csproj", "Desafio03.Api/"]
COPY ["Desafio03.Application/Desafio03.Application.csproj", "Desafio03.Application/"]
COPY ["Desafio03.Domain/Desafio03.Domain.csproj", "Desafio03.Domain/"]
COPY ["Desafio03.Infrastructure/Desafio03.Infrastructure.csproj", "Desafio03.Infrastructure/"]
RUN dotnet restore "Desafio03.Api/Desafio03.Api.csproj"

COPY . .
WORKDIR "/src/Desafio03.Api"
RUN dotnet publish "Desafio03.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Desafio03.Api.dll"]
