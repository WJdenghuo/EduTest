#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EduTest/EduTest.csproj", "EduTest/"]
COPY ["Edu.Tools/Edu.Tools.csproj", "Edu.Tools/"]
COPY ["Edu.Models/Edu.Models.csproj", "Edu.Models/"]
COPY ["Edu.Service/Edu.Service.csproj", "Edu.Service/"]
COPY ["Edu.Entity/Edu.Entity.csproj", "Edu.Entity/"]
RUN dotnet restore "EduTest/EduTest.csproj"
COPY . .
WORKDIR "/src/EduTest"
RUN dotnet build "EduTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EduTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduTest.dll"]