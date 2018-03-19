FROM microsoft/aspnetcore-build:2.0 as dotnet-build
WORKDIR /build
COPY Echo.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /dist

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=dotnet-build /dist .
CMD [ "dotnet", "Echo.dll" ]