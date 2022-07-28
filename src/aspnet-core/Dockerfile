#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.1-bullseye-slim AS base
WORKDIR /app

FROM eaglenos/ps-base AS build
WORKDIR /src
#COPY ["src/Guoxu.LabManager.Web.Host/Guoxu.LabManager.Web.Host.csproj", "src/Guoxu.LabManager.Web.Host/"]
#COPY ["src/Guoxu.LabManager.Web.Core/Guoxu.LabManager.Web.Core.csproj", "src/Guoxu.LabManager.Web.Core/"]
#COPY ["src/Guoxu.LabManager.Application/Guoxu.LabManager.Application.csproj", "src/Guoxu.LabManager.Application/"]
#COPY ["src/Guoxu.LabManager.Core/Guoxu.LabManager.Core.csproj", "src/Guoxu.LabManager.Core/"]
#COPY ["src/Guoxu.LabManager.EntityFrameworkCore/Guoxu.LabManager.EntityFrameworkCore.csproj", "src/Guoxu.LabManager.EntityFrameworkCore/"]
#RUN dotnet restore "src/Guoxu.LabManager.Web.Host/Guoxu.LabManager.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Guoxu.LabManager.Web.Host"
#RUN dotnet build "Guoxu.LabManager.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Guoxu.LabManager.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guoxu.LabManager.Web.Host.dll"]