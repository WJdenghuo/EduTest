FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY EduTest/EduTest.csproj EduTest/
COPY Edu.Tools/Edu.Tools.csproj Edu.Tools/
COPY Edu.Service/Edu.Service.csproj Edu.Service/
COPY Edu.Models/Edu.Models.csproj Edu.Models/
COPY Edu.Entity/Edu.Entity.csproj Edu.Entity/
RUN dotnet restore EduTest/EduTest.csproj
COPY . .
WORKDIR /src/EduTest
RUN dotnet build EduTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EduTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EduTest.dll"]
