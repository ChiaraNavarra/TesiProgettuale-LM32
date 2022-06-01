#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TesiMagistraleLM32/TesiMagistraleLM32.csproj", "TesiMagistraleLM32/"]
RUN dotnet restore "TesiMagistraleLM32/TesiMagistraleLM32.csproj"
COPY . .
WORKDIR "/src/TesiMagistraleLM32"
RUN dotnet build "TesiMagistraleLM32.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TesiMagistraleLM32.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TesiMagistraleLM32.dll"]